using L2L.WebApi.Controllers;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using L2L.Entities;
using L2L.WebApi.Utilities;

namespace L2L.WebApi.Services
{
    public class SearchService : BaseService
    {
        private const int maxPerCategory = 10;
        public SearchService(BaseApiController controller)
            : base(controller)
        {
        }

        public IEnumerable<SearchModel> Search(
            string search1, string search2 = "", string search3 = "")
        {
            try
            {
                List<SearchModel> list = null;

                var userList = SearchUsers(search1, search2, search3);
                var quizzList = SearchQuizz(search1, search2, search3);
                var quizzClass = SearchQuizzClass(search1, search2, search3);

                list = userList.Concat(quizzList).Concat(quizzClass).ToList();

                foreach (var item in list)
                {
                    if (item.BirthDate != null)
                        item.Age = DateTimeUtil.GetAge(item.BirthDate);
                }

                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public List<SearchModel> SearchUsers(
            string search1, string search2 = "", string search3 = "")
        {
            List<string> searchList = new List<string>();
            List<SearchModel> list = null;

            if (string.IsNullOrEmpty(search3) == false)
            {
                list = _uow.Users.GetAll()
                    .Where(u =>
                        u.UserName.Contains(search1) ||
                        u.UserName.Contains(search2) ||
                        u.UserName.Contains(search3) ||
                        u.Profile.LastName.Contains(search1) ||
                        u.Profile.LastName.Contains(search2) ||
                        u.Profile.LastName.Contains(search3) ||
                        u.Profile.FirstName.Contains(search1) ||
                        u.Profile.FirstName.Contains(search2) ||
                        u.Profile.FirstName.Contains(search3)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
                searchList.Add(search3.ToLower());
            }
            else if (string.IsNullOrEmpty(search2) == false)
            {
                list = _uow.Users.GetAll()
                    .Where(u =>
                        u.UserName.Contains(search1) ||
                        u.UserName.Contains(search2) ||
                        u.Profile.LastName.Contains(search1) ||
                        u.Profile.LastName.Contains(search2) ||
                        u.Profile.FirstName.Contains(search1) ||
                        u.Profile.FirstName.Contains(search2)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
            }
            else
            {
                list = _uow.Users.GetAll()
                    .Where(u =>
                        u.UserName.Contains(search1) ||
                        u.Profile.LastName.Contains(search1) ||
                        u.Profile.FirstName.Contains(search1)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
            }

            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    item.SearchType = SearchTypeEnum.User;

                    if (item.UserId == _currentUser.Id)
                        item.IsQuizzmate = true;

                    if (item.IsQuizzmate == false)
                    {
                        item.UserFullName = "";
                        bool isFound = false;
                        foreach (var search in searchList)
                        {
                            if (item.UserName.ToLower().Contains(search))
                                isFound = true;
                        }
                        if (isFound == false)
                            list.Remove(item);
                    }
                }
            }

            return list;
        }

        public List<SearchModel> SearchQuizz(
            string search1, string search2 = "", string search3 = "")
        {
            List<string> searchList = new List<string>();
            List<SearchModel> list = null;

            if (string.IsNullOrEmpty(search3) == false)
            {
                list = _uow.Quizzes.GetAll()
                    .Where(q =>
                        q.IsLive == true &&
                        q.IsDeleted == false && (
                        q.Title.Contains(search1) ||
                        q.Title.Contains(search2) ||
                        q.Title.Contains(search3) ||
                        q.Description.Contains(search1) ||
                        q.Description.Contains(search2) ||
                        q.Description.Contains(search3)) ||
                        q.Owner.UserName.Contains(search1) ||
                        q.Owner.UserName.Contains(search2) ||
                        q.Owner.UserName.Contains(search3) ||
                        q.Owner.Profile.FirstName.Contains(search1) ||
                        q.Owner.Profile.FirstName.Contains(search2) ||
                        q.Owner.Profile.FirstName.Contains(search3) ||
                        q.Owner.Profile.LastName.Contains(search1) ||
                        q.Owner.Profile.LastName.Contains(search2) ||
                        q.Owner.Profile.LastName.Contains(search3) ||
                        q.QuizzTags.Contains(search1) ||
                        q.QuizzTags.Contains(search2) ||
                        q.QuizzTags.Contains(search3)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
                searchList.Add(search3.ToLower());
            }
            else if (string.IsNullOrEmpty(search2) == false)
            {
                list = _uow.Quizzes.GetAll()
                    .Where(q =>
                        q.IsLive == true &&
                        q.IsDeleted == false && (
                        q.Title.Contains(search1) ||
                        q.Title.Contains(search2) ||
                        q.Description.Contains(search1) ||
                        q.Description.Contains(search2)) ||
                        q.Owner.UserName.Contains(search1) ||
                        q.Owner.UserName.Contains(search2) ||
                        q.Owner.Profile.FirstName.Contains(search1) ||
                        q.Owner.Profile.FirstName.Contains(search2) ||
                        q.Owner.Profile.LastName.Contains(search1) ||
                        q.Owner.Profile.LastName.Contains(search2) ||
                        q.QuizzTags.Contains(search1) ||
                        q.QuizzTags.Contains(search2)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
            }
            else
            {
                list = _uow.Quizzes.GetAll()
                    .Where(q =>
                        q.IsLive == true &&
                        q.IsDeleted == false && (
                        q.Title.Contains(search1) ||
                        q.Description.Contains(search1)) ||
                        q.Owner.UserName.Contains(search1) ||
                        q.Owner.Profile.FirstName.Contains(search1) ||
                        q.Owner.Profile.LastName.Contains(search1) ||
                        q.QuizzTags.Contains(search1)
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
            }

            //if (list != null && list.Count > 0)
            //{
            //    foreach (var item in list)
            //    {
            //        item.SearchType = SearchTypeEnum.Quizz;

            //        SetAge(item);
            //        if (item.QuizzAuthorId == _currentUser.Id)
            //            item.IsQuizzmate = true;
            //        item.QuizzAuthor = item.IsQuizzmate ? item.QuizzAuthorFullName : item.QuizzAuthorUserName;
            //        item.QuizzAuthorFullName = "";
            //        item.QuizzAuthorUserName = "";
            //    }
            //}

            if (list != null && list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    item.SearchType = SearchTypeEnum.Quizz;

                    SetAge(item);
                    if (item.QuizzAuthorId == _currentUser.Id)
                        item.IsQuizzmate = true;

                    if (item.IsQuizzmate == false)
                    {
                        bool isFound = false;
                        foreach (var search in searchList)
                        {
                            if (item.QuizzName != null && item.QuizzName.ToLower().Contains(search))
                                isFound = true;

                            if (item.QuizzDescription != null && item.QuizzDescription.ToLower().Contains(search))
                                isFound = true;

                            if (item.QuizzTags != null && item.QuizzTags.ToLower().Contains(search))
                                isFound = true;

                        }
                        if (isFound == false)
                        {
                            list.Remove(item);
                            continue;
                        }
                    }
                   
                    item.QuizzAuthor = item.IsQuizzmate ? item.QuizzAuthorFullName : item.QuizzAuthorUserName;
                    item.QuizzAuthorFullName = "";
                    item.QuizzAuthorUserName = "";
                }
            }

            return list;
        }

        public List<SearchModel> SearchQuizzClass(
            string search1, string search2 = "", string search3 = "")
        {
            List<string> searchList = new List<string>();
            List<SearchModel> list = null;

            if (string.IsNullOrEmpty(search3) == false)
            {
                list = _uow.QuizzClasses.GetAll()
                    .Where(q =>
                        q.IsDeleted == false && (
                        q.ClassName.Contains(search1) ||
                        q.ClassName.Contains(search2) ||
                        q.ClassName.Contains(search3) ||
                        q.Description.Contains(search1) ||
                        q.Description.Contains(search2) ||
                        q.Description.Contains(search3) ||
                        q.Tags.Contains(search1) ||
                        q.Tags.Contains(search2) ||
                        q.Tags.Contains(search3) ||
                        q.Teacher.UserName.Contains(search1) ||
                        q.Teacher.UserName.Contains(search2) ||
                        q.Teacher.UserName.Contains(search3) ||
                        q.Teacher.Profile.FirstName.Contains(search1) ||
                        q.Teacher.Profile.FirstName.Contains(search2) ||
                        q.Teacher.Profile.FirstName.Contains(search3) ||
                        q.Teacher.Profile.LastName.Contains(search1) ||
                        q.Teacher.Profile.LastName.Contains(search2) ||
                        q.Teacher.Profile.LastName.Contains(search3) 
                        )
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
                searchList.Add(search3.ToLower());
            }
            else if (string.IsNullOrEmpty(search2) == false)
            {
                list = _uow.QuizzClasses.GetAll()
                    .Where(q =>
                        q.IsDeleted == false && (
                        q.ClassName.Contains(search1) ||
                        q.ClassName.Contains(search2) ||
                        q.Description.Contains(search1) ||
                        q.Description.Contains(search2) ||
                        q.Tags.Contains(search1) ||
                        q.Tags.Contains(search2) ||
                        q.Teacher.UserName.Contains(search1) ||
                        q.Teacher.UserName.Contains(search2) ||
                        q.Teacher.Profile.FirstName.Contains(search1) ||
                        q.Teacher.Profile.FirstName.Contains(search2) ||
                        q.Teacher.Profile.LastName.Contains(search1) ||
                        q.Teacher.Profile.LastName.Contains(search2)
                        )
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
                searchList.Add(search2.ToLower());
            }
            else
            {
                list = _uow.QuizzClasses.GetAll()
                    .Where(q =>
                        q.IsDeleted == false && (
                        q.ClassName.Contains(search1) ||
                        q.Description.Contains(search1) ||
                        q.Tags.Contains(search1) ||
                        q.Teacher.UserName.Contains(search1) ||
                        q.Teacher.Profile.FirstName.Contains(search1) ||
                        q.Teacher.Profile.LastName.Contains(search1)
                        )
                    )
                    .Take(maxPerCategory)
                    .ProjectTo<SearchModel>(new { userId = _currentUser.Id })
                    .ToList();

                searchList.Add(search1.ToLower());
            }

            if (list != null && list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var item = list[i];
                    item.SearchType = SearchTypeEnum.Quizzroom;

                    if (item.TeacherId == _currentUser.Id)
                    {
                        item.IsQuizzmate = true;
                        item.IsTeacher = true;
                    }

                    if (item.IsQuizzmate == false)
                    {
                        bool isFound = false;
                        foreach (var search in searchList)
                        {
                            if (item.TeacherUserName != null && item.TeacherUserName.ToLower().Contains(search))
                                isFound = true;
                            if (item.ClassName != null && item.ClassName.ToLower().Contains(search))
                                isFound = true;
                            if (item.Description != null && item.Description.ToLower().Contains(search))
                                isFound = true;
                            if (item.QcTags != null && item.QcTags.ToLower().Contains(search))
                                isFound = true;
                        }
                        if (isFound == false)
                        {
                            list.Remove(item);
                            continue;
                        }
                    }

                    item.TeacherName = (bool)item.IsQuizzmate ? item.TeacherFullName : item.TeacherUserName;
                    item.TeacherUserName = "";
                    item.TeacherFullName = "";
                }
            }

            return list;
        }
    }
}