using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;

namespace L2L.WebApi.Services
{
    public enum BadgeEnum
    {
        SignUp,
        FirstQuizz,
        QuizzWith25Questions,
        QuizzWith50Questions,
        QuizzWith100Questions,
        // 5
        QuizzWith200Questions,
        QuizzWith10Reviewers,
        QuizzWith25Reviewers,
        QuizzWIth50Reviewers,
        QuizzWith100Reviewers
        // 10
    }

    public class BadgeService : BaseService, IResource
    {
        public const int MaxBadgeTypeCount = 50;
        public BadgeService(BaseApiController controller)
            : base(controller)
        {
        }

        public void SetBadge(BadgeEnum badgeEnum, bool callSaveChanges = true)
        {
            var badgeIntArr = HelperUtil.GetIntArrayFromString(_currentUser.BadgeStrList, MaxBadgeTypeCount);
            int idx = (int)badgeEnum;
            if(badgeIntArr[idx] == 0)
            {
                badgeIntArr[idx] = 1;
                var user = _uow.Users.GetById(_currentUser.Id);
                user.BadgeStrList = _currentUser.BadgeStrList = HelperUtil.GetStrFromIntArray(badgeIntArr);

                if (callSaveChanges)
                    _uow.SaveChanges();
            }
        }

        private string[] _badgeDescription =
        {
            "Sign Up",
            //"First Quizz published",
            //"Quizz with 25 Questions",
            //"Quizz with 50 Questions",
            //"Quizz with 100 Questions",
            //// 5
            //"Quizz with 200 Questions",
            //"Quizz with 10 Reviewers",
            //"Quizz with 25 Reviewers",
            //"Quizz with 50 Reviewers",
            //"Quizz with 100 Reviewers"
            // 10            
        };

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            return _badgeDescription;
        }

        public object Get(int id)
        {
            throw new NotImplementedException();
        }

        public object Post(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }
    }
}