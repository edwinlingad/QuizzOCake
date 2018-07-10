(function () {
    l2lApp.filter("letter", function () {
        return function (num: number): string {
            if (num === undefined)
                return "";
            return String.fromCharCode(num + 97) + ".";
        }
    });

    l2lApp.filter("toBirthDate", function () {
        var month: string[] = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        return function (data: string): string {
            if (data === undefined)
                return "";

            var date: Date = new Date(data);
            return month[date.getMonth()] + " " + date.getDate() + ", " + date.getFullYear();
        }
    });

    l2lApp.filter('formatDate', function () {

        function getDiffFromToday(date: Date): number {
            var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
            var today = new Date();

            var diffDays = Math.round(Math.abs((today.getTime() - date.getTime()) / (oneDay)));

            return diffDays;
        }

        return function (data: number): string {
            if (data === undefined)
                return "";

            var date = new Date(data);
            var diffDate = getDiffFromToday(date);
            var retString: string;

            if (diffDate > 365) {
                var numYears = Math.floor(diffDate / 365);
                if (numYears == 1)
                    retString = "1 year ago";
                else
                    retString = numYears + " years ago";
            }
            else if (diffDate > 30) {
                var numMonths = Math.floor(diffDate / 30);
                if (numMonths == 1)
                    retString = "1 month ago";
                else
                    retString = numMonths + " months ago";
            }
            else if (diffDate > 7) {
                var numWeeks = Math.floor(diffDate / 7);
                if (numWeeks == 1)
                    retString = "1 week ago";
                else
                    retString = numWeeks + " weeks ago";
            } else {

                if (diffDate == 0) {
                    var today = new Date();
                    var todayHour = today.getHours();
                    var todayMinutes = today.getMinutes();
                    var hour = date.getHours();
                    var minutes: number = date.getMinutes();
                    var minStr: string = minutes < 10 ? "0" + minutes.toString() : minutes.toString();

                    if (hour > todayHour)
                        retString = "yesterday ";
                    else if (hour == todayHour) {
                        if (minutes > todayMinutes)
                            retString = "yesterday ";
                        else
                            retString = "today ";
                    } else
                        retString = "today ";

                    if (hour <= 12)
                        retString = retString + "at " + (hour) + ":" + minStr + " AM";
                    else
                        retString = retString + "at " + (hour - 12) + ":" + minStr + " PM";
                }
                else if (diffDate == 1)
                    retString = "1 day ago";
                else
                    retString = diffDate + " days ago";
            }

            return retString;
        }
    });

    l2lApp.filter("gradeLevelEnumToString", function () {
        return function (data: number): string {
            if (data !== undefined)
                return list.quizzGradeLevels[data].name;
            return "";
        }
    });

    l2lApp.filter("toPercent", function () {
        return function (data: number): string {
            if (data === undefined)
                return "";
            return Math.round(data * 100).toString() + "%";
        }
    });

    l2lApp.filter("formatNumber", function () {
        return function (data: number): string {
            return data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        };
    });

    l2lApp.filter("ceiling", function () {
        return function (data: number): string {
            if (data === undefined)
                return "";
            return Math.ceil(data).toString();
        }
    });

    l2lApp.filter("settingsNumQ", function () {
        return function (data: number): string {
            if (data === undefined)
                return "";
            return data == 0 ? "All" : data.toString();
        }
    });

    l2lApp.filter("settingsOrder", function () {
        return function (data: boolean): string {
            if (data === undefined)
                return "";
            return data ? "Ordered" : "Shuffled";
        }
    });

    l2lApp.filter("otherQuizzers", function () {
        return function (data: number): string {
            if (data === undefined)
                return "";
            return data <= 1 ? "1 other Quizzer" : data.toString() + " other Quizzers";
        }
    });

    l2lApp.filter("quizzerCount", function () {
        return function (data: number): string {
            if (data === undefined)
                return "";
            return data == 1 ? "1 Quizzer" : data.toString() + " Quizzers";
        }
    });

    l2lApp.filter("dueDate", function () {
        var month: string[] = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        return function (data: any): string {
            if (data === undefined)
                return "";

            var date: Date = new Date(data);

            var addString: string = "";
            var diffDay: number = util.getDiffDays(date);
            if (diffDay == 0)
                addString = "( today )";
            if (diffDay == 1)
                addString = "( tomorrow )";

            return month[date.getMonth()] + " " + date.getDate() + ", " + date.getFullYear() + " " + addString;
        }
    });

    l2lApp.filter("truncateLSBItem", function () {
        var maxLength: number = 27;
        return function (data: string): string {
            if (data === undefined)
                return "";
            return data.length > maxLength ? data.substring(0, maxLength) + "..." : data;
        }
    });

    trustAsHtml.$inject = ["$sce"];
    function trustAsHtml($sce) {
        return $sce.trustAsHtml;
    }

    l2lApp.filter('trustAsHtml', trustAsHtml);
})();

