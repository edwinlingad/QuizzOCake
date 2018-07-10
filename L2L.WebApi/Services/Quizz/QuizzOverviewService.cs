using L2L.WebApi.Controllers;
using L2L.WebApi.Enums;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.Entities.Enums;
using L2L.WebApi.Utilities;
using L2L.Entities;
using L2L.WebApi.Creator;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Services
{
    public class QuizzOverviewQueryParam
    {
        public string Category { get; set; }
        public string GradeLevelMin { get; set; }
        public string GradeLevelMax { get; set; }
        public int PageNum { get; set; }
        public int NumPerPage { get; set; }
        public SortByEnum SortBy { get; set; }
        public SortTypeEnum SortType { get; set; }
        public string SearchString { get; set; }
        public int UserId { get; set; }
        public int AvailOnly { get; set; }
        public int DailyRewardsOnly { get; set; }
    }

    public class QuizzOverviewService : BaseService
    {
        public QuizzOverviewService(BaseApiController controller)
            : base(controller)
        {
        }

        public QuizzOverviewListResult GetQuizzOverviewModels(QuizzOverviewQueryParam query)
        {
            var result = new QuizzOverviewListResult();

            if (query.DailyRewardsOnly == 0)
            {
                QuizzOverviewQueryableCreator creator = new QuizzOverviewQueryableCreator(_uow, query, _currentUser);
                IQueryable<QuizzOverviewModel> querable;

                result.TotalCount = creator.GetIQueryable(out querable);
                result.PageNum = query.PageNum;
                result.NumPerPage = query.NumPerPage;
                result.List = querable.ToList();

                UpdateWithDailyReward(result.List);
            }
            else
            {
                var list = GetDailyRewardQuizzes();
                result.TotalCount = list.Count;
                result.PageNum = 1;
                result.NumPerPage = list.Count;
                result.List = list;
            }

            UpdateModelList(result.List);

            return result;
        }

        private void UpdateWithDailyReward(IList<QuizzOverviewModel> list)
        {
            var drList = _svcContainer.DailyRewardSvc.GetDailyRewards();
            _svcContainer.DailyRewardSvc.UpdateWithDailyReward(drList, list);
        }

        public QuizzOverviewModel GetQuizzOverviewModel(int id)
        {
            var model = _uow.Quizzes.GetAll()
                .Include(q => q.Owner.Profile)
                .Include(q => q.Tests)
                .Include(q => q.Reviewers)
                .Where(q => q.Id == id)
                .ProjectTo<QuizzOverviewModel>(new {userId = _currentUser.Id})
                .FirstOrDefault();

            List<QuizzOverviewModel> qoList = new List<QuizzOverviewModel>();
            qoList.Add(model);
            UpdateWithDailyReward(qoList);

            UpdateModel(model);

            return model;
        }

        public void UpdateModelList(IEnumerable<QuizzOverviewModel> list)
        {
            foreach (var item in list)
            {
                UpdateModel(item);
            }
        }

        public void UpdateModel(QuizzOverviewModel model)
        {
            SetAge(model);

            if (model.OwnerId == _currentUser.Id)
            {
                model.IsQuizzmate = true;
                model.IsOwner = true;
                model.CanEdit = true;
            }
            else
            {
                var childIds = _currentUser.AsUserDependents.Select(ud => ud.ChildId)
                    .ToList();
                model.CanEdit = childIds.Contains(model.OwnerId);
            }

            model.OwnerName = model.IsQuizzmate ? model.OwnerFullName : model.OwnerUserName;
            model.OwnerUserName = "";
            model.OwnerFullName = "";
        }

        private List<QuizzOverviewModel> GetDailyRewardQuizzes()
        {
            var list = _svcContainer.DailyRewardSvc.GetAvailableDailyRewardQuizzes();

            return list;
        }
    }
}