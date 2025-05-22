using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public static class DateTimeHelper
    {
        public const string YYYYMMDD = "yyyyMMdd";

        public const string YYYY_MM_DD = "yyyy-MM-dd";

        public const string DD_MM_YYYY = "dd-MM-yyyy";

        public const string MM_DD_YYYY = "MM-dd-yyyy";
        public const string MM_DD_YYYY2 = "MM/dd/yyyy";

        public const string YYYYMM = "yyyyMM";

        public const string YYYY_MM = "yyyy-MM";

        public const string MM_YYYY = "MM-yyyy";

        public const string DD_MM_YYYY_VN = "dd/MM/yyyy";
        public const string MM_YYYY_VN = "MM/yyyy";


        public const string HHmmss = "HH:mm:ss";

        public const string HHmm = "HH:mm";

        public const string FORMAT_DATETIME_UNIVERSAL = "O";

        /// <summary>
        /// MinDate: 1900-01-01
        /// </summary>
        public static readonly DateTime MIN = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        /// <summary>
        /// MinDate: 1900-01-01: Kind là Local
        /// </summary>
        public static readonly DateTime MIN_LOCAL = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local);

        public static readonly DateTime UNIX_START_VALUE = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// MaxDate: 3000-01-01: Kind là Local
        /// </summary>
        public static readonly DateTime MAX_LOCAL = new DateTime(3000, 1, 1, 0, 0, 0, DateTimeKind.Local);
        /// <summary>
        /// MaxDate: 3000-01-01
        /// </summary>
        public static readonly DateTime MAX = new DateTime(3000, 1, 1, 0, 0, 0, DateTimeKind.Utc);



        public static DateTime Now
        {
            get
            {
                return LocalNow;
            }
        }

        public static DateTime UtcNow { get { return DateTime.UtcNow; } }

        public static DateTime LocalNow { get { return DateTime.Now; } }


        /// <summary>
        /// Trả về ngày hiện thời trên thiết bị theo UTC, các trường thời gian = 0
        /// </summary>
        /// <returns></returns>
        public static DateTime UtcToday { get { return DateTime.UtcNow.Date; } }

        /// <summary>
        /// Trả về ngày hiện thời trên thiết bị theo UTC, các trường thời gian = 0
        /// </summary>
        /// <returns></returns>
        public static DateTime LocalToday { get { return DateTime.Now.Date; } }


        public static DateTime Today
        {
            get
            {
                return LocalToday;
            }
        }

        /// <summary>
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToLocalTime(DateTime dt)
        {
            return dt.ToLocalTime();
        }

        /// <summary>
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToUtcTime(this DateTime dt)
        {
            return dt.ToUniversalTime();
        }

        public static string ToUniversalString(this DateTime dt)
        {
            var s = dt.ToString(FORMAT_DATETIME_UNIVERSAL);
            return s;
        }

        /// <summary>
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sourceTimeZone"></param>
        /// <returns></returns>
        public static DateTime ToUtcTime(this DateTime dt, string sourceTimeZone)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
            return ToUtcTime(dt, zone);
        }

        /// <summary>
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sourceTimeZone"></param>
        /// <returns></returns>
        public static DateTime ToUtcTime(this DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dt, DateTimeKind.Unspecified), sourceTimeZone);
        }

        /// <summary>
        /// Trả về một đối tượng mới với cùng các giá trị thời gian nhưng trường Kind được đặt về Utc.
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime SetUtcKind(this DateTime dt)
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        /// <summary>
        /// Trả về một đối tượng mới với cùng các giá trị thời gian nhưng trường Kind được đặt về Local.
        /// Lưu ý: tham số đầu vào "dt" không bị thay đổi.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime SetLocalKind(this DateTime dt)
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Local);
        }


        /// <summary>
        /// Tạo đối tượng DateTime với kiểu UTC.
        /// Phải sử dụng hàm này thay <code>new DateTime()</code> mỗi khi có thể.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime Create(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local);
        }

        public static DateTime? Parse(string value, params string[] formats)
        {
            DateTime v;
            if (DateTime.TryParseExact(value, formats, null, System.Globalization.DateTimeStyles.None, out v))
            {
                return v;
            }

            return null;
        }

        public static bool IsValid(this DateTime d)
        {
            return d > MIN && d < MAX;
        }

        public static DateTime Refine(this DateTime d)
        {
            if (d < MIN)
            {
                return d.Kind == DateTimeKind.Utc ? MIN : MIN_LOCAL;
            }

            if (d > MAX)
            {
                return d.Kind == DateTimeKind.Utc ? MAX : MAX_LOCAL;
            }

            return d;
        }

        /// <summary>
        /// Giá trị nằm ngoài khoảng MIN >= d || d >= MAX
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsNull(this DateTime d)
        {
            if (d.Kind == DateTimeKind.Utc)
            {
                return d <= MIN || d >= MAX;
            }

            return d <= MIN_LOCAL || d >= MAX_LOCAL;
        }

        public static DateTime GetMax(this DateTime d1, DateTime d2)
        {
            return d1 > d2 ? d1 : d2;
        }

        public static DateTime GetEndOfDay(DateTime d)
        {
            return d.Date.AddDays(1).AddSeconds(-1);
        }

        public static DateTime GetFirstDayOfMonth(DateTime d)
        {
            return GetFirstDayOfMonth(d.Year, d.Month);
        }

        public static DateTime GetLastDayOfMonth(DateTime d)
        {
            return GetLastDayOfMonth(d.Year, d.Month);
        }

        public static DateTime GetFirstDayOfMonth(int year, int month)
        {
            return Create(year, month, 1);
        }

        public static DateTime GetLastDayOfMonth(int year, int month)
        {
            return Create(year, month, DateTime.DaysInMonth(year, month));
        }

        public static DateTime GetFirstDayOfYear(int year)
        {
            return Create(year, 1, 1);
        }

        public static DateTime GetLastDayOfYear(int year)
        {
            return Create(year, 12, 31);
        }

        public static DateTime GetFirstDay(DateTime d)
        {
            return d.AddHours(d.Hour * -1).AddMinutes(d.Minute * -1).AddSeconds(d.Second * -1);
        }
        public static DateTime GetLastDay(DateTime d)
        {
            return d.AddHours(d.Hour + (23 - d.Hour)).AddMinutes(d.Minute + (59 - d.Minute)).AddSeconds(d.Second + (59 - d.Second));
        }
    }
}
