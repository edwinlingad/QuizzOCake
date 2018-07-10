using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public enum QuizzPointsEnum
    {
        DailySpecialQuizz,
        QuizzTakeSelf,
        QUizzTakeOthers,

        QuizzPointsMax
    }

    public class QuizzPointsService : BaseService
    {
        public const int MaxDailySpecialQuizzTake = 3;
        public const int MaxQuizzTakeSelf = 3;
        public const int MaxQuizzTakeOthers = 3;
        public const int MaxDailyQuizzTypeCount = 10;
        private static Dictionary<QuizzPointsEnum, int> _quizPointsMap = new Dictionary<QuizzPointsEnum, int>()
        {
            { QuizzPointsEnum.DailySpecialQuizz, 3 },
            { QuizzPointsEnum.QuizzTakeSelf, 1 },
            { QuizzPointsEnum.QUizzTakeOthers, 1 }
        };

        public int GetPointsFor(QuizzPointsEnum type)
        {
            return _quizPointsMap[type];
        }

        public QuizzPointsService(BaseApiController controller)
            : base(controller)
        {
        }

        public int GetDailyRewardPoint(int score, int total)
        {
            double value = (double)score / (double)total;
            if (value >= 0.80)
                return _quizPointsMap[QuizzPointsEnum.DailySpecialQuizz];

            return 0;
        }

        public User AddCurrentUserPoints(QuizzPointsEnum type, bool callSaveChanges = true)
        {
            return AddUserPoints(type, _currentUser.Id, callSaveChanges);
        }

        public User AddUserPoints(QuizzPointsEnum type, int userId, bool callSaveChanges = true)
        {
            int points = _quizPointsMap[type];
            var user = _uow.Users.GetById(userId);

            user.Points = _currentUser.Points = user.Points + points;
            user.DailyPoints = _currentUser.DailyPoints = user.DailyPoints + points;

            AddDailyPoints(type, user, points);

            if (callSaveChanges)
                _uow.SaveChanges();

            return user;
        }

        private void AddDailyPoints(QuizzPointsEnum type, User user, int points)
        {
            int idx = (int)type;
            int[] intArr = HelperUtil.GetIntArrayFromString(user.DailyPointsAllStrList, MaxDailyQuizzTypeCount);
            intArr[idx] += points;

            user.DailyPointsAllStrList = _currentUser.DailyPointsAllStrList = HelperUtil.GetStrFromIntArray(intArr);           
        }
    }
}