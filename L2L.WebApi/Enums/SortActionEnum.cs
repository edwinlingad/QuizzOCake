using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Enums
{
    public enum SortByEnum
    {
        DateCreated = 0,
        DateModified,
        NumLikes,
        NumPeopleTakenTest,
        NumQuestions,
        Creator
    }

    public enum SortTypeEnum
    {
        Ascending = 0,
        Descending
    }
}