#pragma warning disable
using System.Reflection;
using System.Text;

namespace HubbaGolfAdmin.Shared
{
    public static class Utils
    {
        /// <summary>
        /// convert null prop string = [] and number = 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myObject"></param>
        public static void NullToEmpty<T>(this T myObject) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var info in properties)
            {
                Type propertyType = Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType;

                if (info.GetValue(myObject, null) == null)
                {
                    if (info.PropertyType == typeof(string))
                    {
                        info.SetValue(myObject, string.Empty, null);
                        continue;
                    }
                    if (info.PropertyType == typeof(int))
                    {
                        info.SetValue(myObject, 0, null);
                        continue;
                    }
                    if (info.PropertyType == typeof(decimal))
                    {
                        info.SetValue(myObject, 0, null);
                        continue;
                    }
                }
            }
        }

        public static bool IsNumeric(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.All(char.IsDigit);
        }

        /// <summary>
        /// est time like facebook
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static string TimeAgo(DateTime FromDate, DateTime ToDate)
        {
            TimeSpan span = ToDate - FromDate;

            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                {
                    years += 1;
                }

                return String.Format("about {0} {1} ago", years, years == 1 ? "year" : "years");
            }

            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                {
                    months += 1;
                }

                return String.Format("about {0} {1} ago", months, months == 1 ? "month" : "months");
            }

            if (span.Days > 0)
            {
                return String.Format("about {0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");
            }

            if (span.Hours > 0)
            {
                return String.Format("about {0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");
            }

            if (span.Minutes > 0)
            {
                return String.Format("about {0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            }

            if (span.Seconds > 5)
            {
                return String.Format("about {0} seconds ago", span.Seconds);
            }

            if (span.Seconds <= 5)
            {
                return "recently !.";
            }

            return string.Empty;
        }

        /// <summary>
        /// count date from ToDate - FromDate
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static double CalculatorDays(DateTime FromDate, DateTime ToDate)
        {
            // Calculate the difference
            TimeSpan difference = ToDate - FromDate;

            // Get the total number of days
            double totalDays = difference.TotalDays;
            return totalDays;
        }
    }
}
