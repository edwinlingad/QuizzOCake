namespace L2L.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using L2L.Entities;

    public sealed class Configuration : DbMigrationsConfiguration<L2L.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(L2L.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //var dependents = context.Users.Select(p => p.AsUserDependents.Select(q => q.Child)).ToList();
            //foreach (var item in dependents)
            //{
            //    if (item.Count() >= 0)
            //    {
            //        foreach (var deps in item)
            //        {
            //            var perm = new DependentPermission
            //            {
            //                User = deps
            //            };
            //            context.DependentPermissions.AddOrUpdate(perm);
            //        }

            //    }
            //}

            //var deps2 = context.Dependents.ToList();
            //foreach (var d in deps2)
            //{
            //    d.IsPrimary = true;
            //    context.Dependents.AddOrUpdate(d);
            //}

            //#region Question Author Update

            //var tests = context.Tests
            //    .Include(t => t.Quizz)
            //    .Include(t => t.Questions);

            //foreach (var test in tests.ToList())
            //{
            //    var authorId = test.Quizz.OwnerId;
            //    foreach (var question in test.Questions)
            //    {
            //        question.AuthorId = authorId;
            //        context.Questions.AddOrUpdate(question);
            //    }
            //}

            //#endregion

            //var questions = context.Questions.ToList();
            //foreach (var question in questions)
            //{
            //    question.IsFlashCard = true;
            //    context.Questions.AddOrUpdate(question);
            //}

            //var users = context.Users.ToList();
            //foreach (var item in users)
            //{
            //    //if (item.DailyPointsQuizzSelf == null)
            //    //    item.DailyPointsQuizzSelf = "";
            //    //if (item.DailyPointsQuizzOthers == null)
            //    //    item.DailyPointsQuizzOthers = "";
            //    //if (item.DailyPointsDate == null)
            //    //    item.DailyPointsDate = "";
            //    if (item.DailySpecialPointsQuizzStrList == null)
            //        item.DailySpecialPointsQuizzStrList = "";

            //    if (item.BadgeStrList == null)
            //        item.BadgeStrList = "1,";

            //    context.Users.AddOrUpdate(item);
            //}

            //var friendRequest = new FriendRequest
            //{
            //    RequestFromId = 1,
            //    RequestToId = 1,
            //    IsNew = false,
            //    IsAccepted = true
            //};
            //context.FriendRequests.AddOrUpdate(friendRequest);

            //var newNotifications = context.NewNotifications.ToList();
            //foreach (var item in newNotifications)
            //{
            //    item.FriendRequest = friendRequest;
            //    context.NewNotifications.AddOrUpdate(item);
            //}

            //var qnList = context.QuickNotes.ToList();
            //foreach (var item in qnList)
            //{
            //    if (item.TextContent == null || item.TextContent.Length == 0)
            //    {
            //        if (item.Notes != null && item.Notes.Length != 0)
            //        {
            //            item.TextContent = item.Notes;
            //            item.Notes = "";
            //            context.QuickNotes.AddOrUpdate(item);
            //        }
            //    }
            //}

            //var qaList = context.QuizQandAQuestions.ToList();
            //foreach (var item in qaList)
            //{
            //    if (item.TextContent == null || item.TextContent.Length == 0)
            //    {
            //        if (item.Question != null && item.Question.Length != 0)
            //        {
            //            item.TextContent = item.Question;
            //            item.Question = "";
            //            context.QuizQandAQuestions.AddOrUpdate(item);
            //        }
            //    }
            //}

            //var mcList = context.QuizMultipleChoiceQuestions.ToList();
            //foreach (var item in mcList)
            //{
            //    if (item.TextContent == null || item.TextContent.Length == 0)
            //    {
            //        if (item.Question != null && item.Question.Length != 0)
            //        {
            //            item.TextContent = item.Question;
            //            item.Question = "";
            //            context.QuizMultipleChoiceQuestions.AddOrUpdate(item);
            //        }
            //    }
            //}

            //var mcsList = context.MultiChoiceSameQuestions.ToList();
            //foreach (var item in mcsList)
            //{
            //    if (item.TextContent == null || item.TextContent.Length == 0)
            //    {
            //        if (item.Question != null && item.Question.Length != 0)
            //        {
            //            item.TextContent = item.Question;
            //            item.Question = "";
            //            context.MultiChoiceSameQuestions.AddOrUpdate(item);
            //        }
            //    }
            //}

            //var flashCardList = context.TextFlashCards.ToList();
            //foreach (var item in flashCardList)
            //{
            //    if (item.TextContent == null || item.TextContent.Length == 0)
            //    {
            //        if (item.Title != null && item.Title.Length != 0)
            //        {
            //            item.TextContent = item.Title;
            //            item.Title = "";
            //            context.TextFlashCards.AddOrUpdate(item);
            //        }
            //    }
            //}

            //var quizzClassMembers = context.QuizzClassMembers
            //    .Where(qcm => qcm.QuizzClass.IsDeleted == false)
            //    .Include(qcm => qcm.Student)
            //    .Include(qcm => qcm.QuizzClass)
            //    .ToList();
            //foreach (var item in quizzClassMembers)
            //{
            //    if(item.Student.UserType == Entities.Enums.UserTypeEnum.Child)
            //    {
                    
            //        var parent = quizzClassMembers
            //            .Where(qcm => qcm.QuizzClassId == item.QuizzClassId && qcm.DependentId == item.StudentId)
            //            .FirstOrDefault();

            //        if(parent == null)
            //        {
            //            var parentIds = context.Users
            //                .Where(u => u.Id == item.StudentId)
            //                .Select(u => u.AsChildDependsOn.Select(d => d.UserId))
            //                .FirstOrDefault();
            //            foreach (var parentId in parentIds)
            //            {
            //                var model = new QuizzClassMember
            //                {
            //                    IsNew = true,
            //                    IsNewInviteAccepted = false,
            //                    QuizzClassId = item.QuizzClassId,
            //                    StudentId = parentId,
            //                    DependentId = item.StudentId,
            //                    IsParent = true
            //                };

            //                context.QuizzClassMembers.AddOrUpdate(model);
            //            }
            //        }
            //    }
            //}
        }
    }
}
