using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.Entities.Enums;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    public class GradeLevelController : BaseApiController
    {
    //    private string elemFgColor = "";
    //    private string elemBgColor = "";
    //    private string midFgColor = "";
    //    private string midBgColor = "";
    //    private string hsFgColor = "";
    //    private string hsBgColor = "";
        public GradeLevelController()
        {
            InitializeGradeLevels();
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, gradeLevelList);
        }

        private void InitializeGradeLevels()
        {
            QuizzGradeLevelEnum min;
            QuizzGradeLevelEnum max;
            foreach (var item in gradeLevelList)
            {
                min = max = item.GradeLevel;
                var count = Uow.Quizzes.GetAll()
                    .Where(q =>
                        (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                        (min >= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax) ||
                        (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max <= q.GradeLevelMax) ||
                        (min <= q.GradeLevelMin && min <= q.GradeLevelMax && max >= q.GradeLevelMin && max >= q.GradeLevelMax)
                        ).Count();
                item.NumQuizz = count;
            }
        }

        private List<GradeLevelModel> gradeLevelList = new List<GradeLevelModel>() {
            new GradeLevelModel {
                Name = "Unassigned",
                GradeLevel = QuizzGradeLevelEnum.Unassigned,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Pre K",
                GradeLevel = QuizzGradeLevelEnum.PreK,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Kindergarden",
                GradeLevel = QuizzGradeLevelEnum.K,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 1",
                GradeLevel = QuizzGradeLevelEnum.Grade1,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 2",
                GradeLevel = QuizzGradeLevelEnum.Grade2,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 3",
                GradeLevel = QuizzGradeLevelEnum.Grade3,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 4",
                GradeLevel = QuizzGradeLevelEnum.Grade4,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 5",
                GradeLevel = QuizzGradeLevelEnum.Grade5,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 6",
                GradeLevel = QuizzGradeLevelEnum.Grade6,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 7",
                GradeLevel = QuizzGradeLevelEnum.Grade7,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 8",
                GradeLevel = QuizzGradeLevelEnum.Grade8,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 9",
                GradeLevel = QuizzGradeLevelEnum.Grade9,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 10",
                GradeLevel = QuizzGradeLevelEnum.Grade10,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 11",
                GradeLevel = QuizzGradeLevelEnum.Grade11,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Grade 12",
                GradeLevel = QuizzGradeLevelEnum.Grade12,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "College",
                GradeLevel = QuizzGradeLevelEnum.College,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
            new GradeLevelModel {
                Name = "Professional",
                GradeLevel = QuizzGradeLevelEnum.Professional,
                FgColor = "#FFFFFF",
                BgColor = "#000000"
            },
        };
    }
}
