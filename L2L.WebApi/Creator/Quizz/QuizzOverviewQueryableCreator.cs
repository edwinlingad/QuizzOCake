using L2L.Data;
using L2L.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.Entities.Enums;
using L2L.Entities;
using L2L.WebApi.Models;
using AutoMapper.QueryableExtensions;

namespace L2L.WebApi.Creator
{
    public class QuizzOverviewQueryableCreator
    {
        QuizzOverviewQueryParam _query;
        IQueryable<Quizz> _queryable;
        IQueryable<QuizzOverviewModel> _qoQueryable2;
        User _currentUser;
        public QuizzOverviewQueryableCreator(
            ApplicationUnit uow,
            QuizzOverviewQueryParam query,
            User currentUser)
        {
            _query = query;
            _queryable = uow.Quizzes.GetAll()
                .Where(q => q.IsDeleted == false);
            _currentUser = currentUser;
        }

        public int GetIQueryable(out IQueryable<QuizzOverviewModel> queryable)
        {
            int totalCount = 0;

            AddIsLiveCondition();
            AddCategory();
            AddGradeLevel();
            AddUser();
            AddSearchString();
            totalCount = _queryable.Count();
            ConvertToQuizzOverview();
            AddSort();
            AddSkip();

            queryable = _qoQueryable2;
            return totalCount;
        }

        private void AddIsLiveCondition()
        {
            if(_query.AvailOnly == 1)
            {
                _queryable = _queryable.Where(q => q.IsLive == true);
                return;
            }

            if (_query.UserId == _currentUser.Id)
            {
                // Do not put the Where q.IsLive condition
            }
            else
            {
                if (_currentUser.AsUserDependents != null && _currentUser.AsUserDependents.Count != 0)
                {
                    foreach (var item in _currentUser.AsUserDependents)
                    {
                        if (item.ChildId == _query.UserId)
                            return;
                    }
                }
                _queryable = _queryable.Where(q => q.IsLive == true);
            }
        }

        private void AddUser()
        {
            if (_query.UserId != 0)
            {
                _queryable = _queryable.Where(q => q.OwnerId == _query.UserId);
            }
        }

        private void AddSearchString()
        {
            if (string.IsNullOrEmpty(_query.SearchString) == false)
            {
                var strList = _query.SearchString.Split(' ');
                switch (strList.Length)
                {
                    case 1:
                        {
                            var str1 = strList[0];
                            _queryable = _queryable.Where(q => q.Title.Contains(str1) || q.Description.Contains(str1));
                            break;
                        }
                    case 2:
                        {
                            var str1 = strList[0];
                            var str2 = strList[1];
                            _queryable = _queryable.Where(q => q.Title.Contains(str1) || q.Description.Contains(str1)
                                || q.Title.Contains(str2) || q.Description.Contains(str2));
                        }
                        break;
                    case 3:
                        {
                            var str1 = strList[0];
                            var str2 = strList[1];
                            var str3 = strList[2];
                            _queryable = _queryable.Where(q => q.Title.Contains(str1) || q.Description.Contains(str1)
                                || q.Title.Contains(str2) || q.Description.Contains(str2)
                                || q.Title.Contains(str3) || q.Description.Contains(str3));
                            break;
                        }
                }
                //_queryable = _queryable.Where(q => q.Title.Contains(_query.SearchString) || q.Description.Contains(_query.SearchString));
                //_queryable = _queryable.Where(q => );
            }
        }

        private void ConvertToQuizzOverview()
        {
            //_qoQueryable2 = _queryable.Select(q => new QuizzOverviewModel
            //{
            //    Id = q.Id,
            //    Title = q.Title,
            //    Description = q.Description,
            //    IsLive = q.IsLive,
            //    IsBuiltIn = q.IsBuiltIn,
            //    Category = q.Category,
            //    NumQuestions = q.Tests.FirstOrDefault().Questions.Count,
            //    NumLikes = q.QuizRating.QuizUpvotes.Where(r => r.UpVote == 1).Count(),
            //    OwnerId = q.OwnerId,
            //    OwnerName = q.Owner.UserName,
            //    ReviewerId = q.Reviewers.FirstOrDefault().Id,
            //    TestId = q.Tests.FirstOrDefault().Id,
            //    IsBookmarked = false,   // TODO
            //    IsLiked = q.QuizRating.QuizUpvotes.Where(r => r.UserId == _currentUser.Id && r.UpVote == 1).Count() == 1,
            //    QuizzRatingId = q.QuizRating.Id,
            //    Difficulty = q.Difficulty,
            //    GradeLevelMin = q.GradeLevelMin,
            //    GradeLevelMax = q.GradeLevelMax,

            //    Created = q.Created,
            //    Modified = q.Modified
            //});

            _qoQueryable2 = _queryable.ProjectTo<QuizzOverviewModel>(new { userId = _currentUser.Id });
        }

        private void AddCategory()
        {
            string category = _query.Category;
            if (String.IsNullOrEmpty(category) == false)
            {
                int quizzCategoryEnum;

                int tmpVal;
                if (Int32.TryParse(category, out tmpVal) == false)
                    return;
                if (tmpVal == -1)
                    return;
                quizzCategoryEnum = (int)tmpVal;

                _queryable = _queryable.Where(q => q.Category == quizzCategoryEnum);
            }
        }

        private void AddGradeLevel()
        {
            string gradeLevelMin = _query.GradeLevelMin;
            string gradeLevelMax = _query.GradeLevelMax;
            if (String.IsNullOrEmpty(gradeLevelMin) == false || String.IsNullOrEmpty(gradeLevelMax) == false)
            {
                QuizzGradeLevelEnum min;
                QuizzGradeLevelEnum max;
                int tmpVal;
                if (Int32.TryParse(gradeLevelMin, out tmpVal) == false)
                    tmpVal = -1;
                min = (QuizzGradeLevelEnum)tmpVal;

                if (Int32.TryParse(gradeLevelMax, out tmpVal) == false)
                    tmpVal = (int)QuizzGradeLevelEnum.MaxGradeLevel;

                if (tmpVal == -1)
                    tmpVal = (int)QuizzGradeLevelEnum.MaxGradeLevel;

                max = (QuizzGradeLevelEnum)tmpVal;

                //_queryable = _queryable.Where(q => min >= q.GradeLevelMin && max <= q.GradeLevelMax);
                _queryable = _queryable.Where(q =>
                    (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                    (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax) ||
                    (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                    (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax)
                    );
            }
        }

        private void AddSort()
        {
            switch (_query.SortType)
            {
                case L2L.WebApi.Enums.SortTypeEnum.Ascending:
                    OrderAscending();
                    break;
                case L2L.WebApi.Enums.SortTypeEnum.Descending:
                    OrderDescending();
                    break;
                default:
                    break;
            }
        }

        private void OrderAscending()
        {
            switch (_query.SortBy)
            {
                case L2L.WebApi.Enums.SortByEnum.DateCreated:
                    _qoQueryable2 = _qoQueryable2.OrderBy(q => q.Created);
                    break;
                case L2L.WebApi.Enums.SortByEnum.DateModified:
                    _qoQueryable2 = _qoQueryable2.OrderBy(q => q.Modified);
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumLikes:
                    _qoQueryable2 = _qoQueryable2.OrderBy(q => q.NumLikes);
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumPeopleTakenTest:
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumQuestions:
                    _qoQueryable2 = _qoQueryable2.OrderBy(q => q.NumQuestions);
                    break;
                case L2L.WebApi.Enums.SortByEnum.Creator:
                    _qoQueryable2 = _qoQueryable2.OrderBy(q => q.OwnerName);
                    break;
                default:
                    break;
            }
        }

        private void OrderDescending()
        {
            switch (_query.SortBy)
            {
                case L2L.WebApi.Enums.SortByEnum.DateCreated:
                    _qoQueryable2 = _qoQueryable2.OrderByDescending(q => q.Created);
                    break;
                case L2L.WebApi.Enums.SortByEnum.DateModified:
                    _qoQueryable2 = _qoQueryable2.OrderByDescending(q => q.Modified);
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumLikes:
                    _qoQueryable2 = _qoQueryable2.OrderByDescending(q => q.NumLikes);
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumPeopleTakenTest:
                    break;
                case L2L.WebApi.Enums.SortByEnum.NumQuestions:
                    _qoQueryable2 = _qoQueryable2.OrderByDescending(q => q.NumQuestions);
                    break;
                case L2L.WebApi.Enums.SortByEnum.Creator:
                    _qoQueryable2 = _qoQueryable2.OrderByDescending(q => q.OwnerName);
                    break;
                default:
                    break;
            }
        }

        private void AddSkip()
        {
            _qoQueryable2 = _qoQueryable2.Skip((_query.PageNum - 1) * _query.NumPerPage)
                .Take(_query.NumPerPage);
        }
    }
}