using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.Entities.Enums;
using System.Data.Entity;

namespace L2L.WebApi.Services
{
    public class DailyRewardService : BaseService
    {
        private int _numGenerateDailyRewardPerDay = 7;
        private Random _random;

        public DailyRewardService(BaseApiController controller)
            : base(controller)
        {
            _random = new Random();
        }

        public IList<DailyReward> GetDailyRewards()
        {
            var clientToday = _svcContainer.UserSvc.GetClientToday();
            DateTime today = DateTimeUtil.GetClientDay(clientToday);
            QuizzGradeLevelEnum levelMin;
            QuizzGradeLevelEnum levelMax;
            var currentUserLevel = GetUserLevel(out levelMin, out levelMax);

            var list = _uow.DailyRewards.GetAll()
                .Where(d => d.Level == currentUserLevel && d.ForDate == today)
                .ToList();

            if (list == null || list.Count == 0)
                list = GenerateDailyRewardForLevel(currentUserLevel, levelMin, levelMax, today);

            return list;
        }

        private List<DailyReward> GenerateDailyRewardForLevel(
            DailyRewardLevel currentUserLevel, QuizzGradeLevelEnum min, QuizzGradeLevelEnum max, DateTime today)
        {
            var list = new List<DailyReward>();

            CreateDailyReward(0, list, currentUserLevel, min, max, today);
            CreateDailyReward(1, list, currentUserLevel, min, max, today);
            CreateDailyReward(2, list, currentUserLevel, min, max, today);

            var catList = GetIncludedCategories();
            int numQuizz = _numGenerateDailyRewardPerDay - list.Count;

            do
            {
                if (catList.Count == 0)
                    break;

                int catIdx = _random.Next(catList.Count);
                int categoryValue = catList[catIdx].CategoryValue;
                catList.RemoveAt(catIdx);

                if (CreateDailyReward(categoryValue, list, currentUserLevel, min, max, today))
                    numQuizz--;
                else
                    continue;

            } while (numQuizz > 0);

            _uow.SaveChanges();

            list = _uow.DailyRewards.GetAll()
                .Where(d => d.Level == currentUserLevel && d.ForDate == today)
                .ToList();

            return list;
        }

        private bool CreateDailyReward(
            int category, List<DailyReward> list, DailyRewardLevel currentUserLevel, QuizzGradeLevelEnum min, QuizzGradeLevelEnum max, DateTime today)
        {
            int qzCount = _uow.Quizzes.GetAll()
                   .Where(q => q.Category == category && q.IsLive == true && (
                        (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                        (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax) ||
                        (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                        (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax)))
                   .Count();

            if (qzCount == 0)
                return false;

            int quizzIdx = _random.Next(qzCount);
            int quizzId = _uow.Quizzes.GetAll()
                .Where(q => q.Category == category && q.IsLive == true && (
                    (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                    (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax) ||
                    (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                    (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax)))
                .OrderByDescending(q => q.Modified)
                .Skip(quizzIdx)
                .Take(1)
                .Select(q => q.Id)
                .FirstOrDefault();

            if (quizzId == 0)
                return false;

            DailyReward reward = new DailyReward
            {
                ForDate = today,
                Level = currentUserLevel,
                QuizzId = quizzId
            };

            _uow.DailyRewards.Add(reward);
            list.Add(reward);

            return true;
        }

        private List<QuizzCategory> GetIncludedCategories()
        {
            var list = _uow.QuizzCategories.GetAll()
                .Where(c => c.IsIncludedInDailyReward == true)
                .ToList();

            return list;
        }

        private DailyRewardLevel GetUserLevel(out QuizzGradeLevelEnum min, out QuizzGradeLevelEnum max)
        {
            DailyRewardLevel level = DailyRewardLevel.CollegeAndProf;
            min = QuizzGradeLevelEnum.College;
            max = QuizzGradeLevelEnum.Professional;
            int age = (int)Math.Floor(DateTime.Now.Subtract(_currentUser.Profile.BirthDate).TotalDays / 365);

            if (age <= 5)
            {
                level = DailyRewardLevel.KBelow;
                min = QuizzGradeLevelEnum.PreK;
                max = QuizzGradeLevelEnum.K;
            }
            else if (age <= 8)
            {
                level = DailyRewardLevel.Grade1To3;
                min = QuizzGradeLevelEnum.Grade1;
                max = QuizzGradeLevelEnum.Grade3;
            }

            else if (age <= 11)
            {
                level = DailyRewardLevel.Grade4To6;
                min = QuizzGradeLevelEnum.Grade4;
                max = QuizzGradeLevelEnum.Grade6;
            }
            else if (age <= 14)
            {
                level = DailyRewardLevel.Grade7to9;
                min = QuizzGradeLevelEnum.Grade7;
                max = QuizzGradeLevelEnum.Grade9;
            }
            else if (age <= 17)
            {
                level = DailyRewardLevel.Grade10To12;
                min = QuizzGradeLevelEnum.Grade10;
                max = QuizzGradeLevelEnum.Grade12;
            }

            return level;
        }

        public List<QuizzOverviewModel> GetAvailableDailyRewardQuizzes()
        {
            try
            {
                var qtSpecialList = HelperUtil.GetIntArrayFromString(_currentUser.DailySpecialPointsQuizzStrList);
                List<QuizzOverviewModel> list = new List<QuizzOverviewModel>();
                if (qtSpecialList.Length < QuizzPointsService.MaxDailySpecialQuizzTake)
                {
                    var drList = _svcContainer.DailyRewardSvc.GetDailyRewards();
                    var drIds = drList.Select(dr => dr.QuizzId)
                        .ToArray();

                    list = _uow.Quizzes.GetAll()
                        .Where(q => drIds.Contains(q.Id))
                        .ProjectTo<QuizzOverviewModel>()
                        .ToList();

                    UpdateWithDailyReward(drList, list);

                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i].DailyReward.IsTaken)
                            list.RemoveAt(i);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return new List<QuizzOverviewModel>();
            }
            
        }

        public void UpdateWithDailyReward(
            IEnumerable<DailyReward> dailyRewardList, IEnumerable<QuizzOverviewModel> qoList)
        {
            var qtSpecialList = HelperUtil.GetIntArrayFromString(_currentUser.DailySpecialPointsQuizzStrList);
            var qtOwnerList = HelperUtil.GetIntArrayFromString(_currentUser.DailyNormalPointsQuizzSelfStrList);
            var qtOthersList = HelperUtil.GetIntArrayFromString(_currentUser.DailyNormalPointsQuizzOthersStrList);

            foreach (var item in qoList)
            {
                DailyRewardModel model = null;
                var reward = dailyRewardList.Where(dr => dr.QuizzId == item.Id)
                    .FirstOrDefault();

                if (reward != null)
                    model = GetSpecialDailyRewardModel(qtSpecialList, reward, qtOwnerList, qtOthersList, item);
                else
                    model = GetNormalDailRewardModel(qtOwnerList, qtOthersList, item);

                item.DailyReward = model;
            }
        }

        private DailyRewardModel GetNormalDailRewardModel(int[] qtOwnerList, int[] qtOthersList, QuizzOverviewModel item)
        {
            DailyRewardModel model = new DailyRewardModel()
            {
                DailyRewardId = 0
            };
            if (_svcContainer.QuizzSvc.IsAuthor(item.Id, _currentUser.Id))
            {
                model.AvailablePoints = _svcContainer.QuizzPointsSvc.GetPointsFor(QuizzPointsEnum.QuizzTakeSelf);

                if (qtOwnerList.Length < QuizzPointsService.MaxQuizzTakeSelf &&
                    qtOwnerList.Contains(item.Id) == false)
                    model.IsTaken = false;
                else
                    model.IsTaken = true;
            }
            else
            {
                model.AvailablePoints = _svcContainer.QuizzPointsSvc.GetPointsFor(QuizzPointsEnum.QUizzTakeOthers);

                if (qtOthersList.Length < QuizzPointsService.MaxQuizzTakeOthers &&
                    qtOthersList.Contains(item.Id) == false)
                    model.IsTaken = false;
                else
                    model.IsTaken = true;
            }

            return model;
        }

        private DailyRewardModel GetSpecialDailyRewardModel(int[] qtSpecialList, DailyReward reward, int[] qtOwnerList, int[] qtOthersList, QuizzOverviewModel item)
        {
            DailyRewardModel model = new DailyRewardModel
            {
                DailyRewardId = reward.Id,
                IsTaken = false,
                Points = 0,
                AvailablePoints = _svcContainer.QuizzPointsSvc.GetPointsFor(QuizzPointsEnum.DailySpecialQuizz)
            }; ;

            bool qtSpecialListHasId = qtSpecialList.Contains(reward.Id);
            if (qtSpecialList.Length < QuizzPointsService.MaxDailySpecialQuizzTake)
                model.IsTaken = qtSpecialListHasId;
            else
            {
                if (qtSpecialListHasId)
                    model.IsTaken = true;
                else
                    model = GetNormalDailRewardModel(qtOwnerList, qtOthersList, item);
            }

            return model;
        }

        public void CheckAddDailyRewardPoints(TestLogModel testLog, bool callSaveChanges = true)
        {
            var list = GetDailyRewards();
            var reward = list.Where(r => r.QuizzId == testLog.QuizzId)
                .FirstOrDefault();

            if (reward != null)
                CheckAddSpecialDailyReward(testLog, reward, callSaveChanges);
            else
                CheckAddNormalDailyReward(testLog, callSaveChanges);

        }

        private void CheckAddSpecialDailyReward(TestLogModel testLog, DailyReward reward, bool callSaveChanges)
        {
            var rewardIdList = HelperUtil.GetIntArrayFromString(_currentUser.DailySpecialPointsQuizzStrList);
            User user = null;

            bool qtRewardListHasId = rewardIdList.Contains(reward.Id);
            if (qtRewardListHasId == false)
            {
                if (rewardIdList.Length < QuizzPointsService.MaxDailySpecialQuizzTake)
                {
                    var points = ComputRewardPonts(testLog.Score, testLog.Total);

                    if (points != 0)
                        user = _svcContainer.QuizzPointsSvc.AddCurrentUserPoints(QuizzPointsEnum.DailySpecialQuizz, callSaveChanges);
                    else
                        user = _uow.Users.GetById(_currentUser.Id);

                    user.DailySpecialPointsQuizzStrList = _currentUser.DailySpecialPointsQuizzStrList = user.DailySpecialPointsQuizzStrList + reward.Id.ToString() + ",";
                    _uow.Users.Update(user);
                    _svcContainer.UserSvc.UpdateCurrentUserInSession();

                    if (callSaveChanges)
                        _uow.SaveChanges();
                }
                else
                {
                    CheckAddNormalDailyReward(testLog, callSaveChanges);
                }
            }
        }

        private void CheckAddNormalDailyReward(TestLogModel testLog, bool callSaveChanges)
        {
            var quizzTakenSelf = _currentUser.DailyNormalPointsQuizzSelfStrList.Split(',');
            var quizzTakenOthers = _currentUser.DailyNormalPointsQuizzOthersStrList.Split(',');
            var quizzIdStr = testLog.QuizzId.ToString();

            var canGetPoints = CanGetPoints(testLog.Score, testLog.Total);
            User user = null;

            if (_svcContainer.QuizzSvc.IsAuthor(testLog.QuizzId, _currentUser.Id))
            {
                if (quizzTakenSelf.Length < QuizzPointsService.MaxQuizzTakeSelf &&
                    quizzTakenSelf.Contains(quizzIdStr) == false)
                {
                    if (canGetPoints)
                        user = _svcContainer.QuizzPointsSvc.AddCurrentUserPoints(QuizzPointsEnum.QuizzTakeSelf, callSaveChanges);
                    else
                        user = _uow.Users.GetById(_currentUser.Id);
                    _currentUser.DailyNormalPointsQuizzSelfStrList = user.DailyNormalPointsQuizzSelfStrList = user.DailyNormalPointsQuizzSelfStrList + quizzIdStr + ",";
                    _uow.Users.Update(user);
                    _svcContainer.UserSvc.UpdateCurrentUserInSession();

                    if (callSaveChanges)
                        _uow.SaveChanges();
                }
            }
            else
            {
                if (quizzTakenOthers.Length < QuizzPointsService.MaxQuizzTakeOthers &&
                    quizzTakenOthers.Contains(quizzIdStr) == false)
                {
                    if (canGetPoints)
                        user = _svcContainer.QuizzPointsSvc.AddCurrentUserPoints(QuizzPointsEnum.QUizzTakeOthers, callSaveChanges);
                    else
                        user = _uow.Users.GetById(_currentUser.Id);
                    _currentUser.DailyNormalPointsQuizzOthersStrList = user.DailyNormalPointsQuizzOthersStrList = user.DailyNormalPointsQuizzOthersStrList + quizzIdStr + ",";
                    _uow.Users.Update(user);
                    _svcContainer.UserSvc.UpdateCurrentUserInSession();

                    if (callSaveChanges)
                        _uow.SaveChanges();
                }
            }
        }

        private bool CanGetPoints(int score, int total)
        {
            double value = (double)score / (double)total;
            if (value >= 0.80)
                return true;
            return false;
        }

        private int ComputRewardPonts(int score, int total)
        {
            return _svcContainer.QuizzPointsSvc.GetDailyRewardPoint(score, total);
        }

        public void CheckResetDailyReward(string clientToday)
        {
            if (_currentUser.DailyPointsDate != clientToday)
            {
                ResetUserDailyReward(clientToday, _currentUser);

                var user = _uow.Users.GetById(_currentUser.Id);
                ResetUserDailyReward(clientToday, user);
                _uow.SaveChanges();
            }
        }

        private void ResetUserDailyReward(string clientToday, User user)
        {
            user.DailyPointsDate = clientToday;
            user.DailyPoints = 0;
            user.DailyNormalPointsQuizzSelfStrList = "";
            user.DailyNormalPointsQuizzOthersStrList = "";
            user.DailySpecialPointsQuizzStrList = "";
            user.DailyPointsAllStrList = "";
        }
    }
}