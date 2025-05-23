using System;
using System.Web;
using VSW.Core.Data;

namespace VSW.Lib.Global
{
    public static class Error
    {
        private static int Year => DateTime.Now.Year;

        private static int Month => DateTime.Now.Month;

        private static string PathCurrent
        {
            get
            {
                var path = Core.Global.Application.BaseDirectory + "/Logs";

                //bo qua loi
                try
                {
                    Directory.Create(path + "/" + Year);
                }
                catch
                {
                    //ignored
                }

                return path;
            }
        }

        public static void Write(string func, string var, Exception exception)
        {
            Write(func + " : " + var + "\r\n", exception);
        }

        public static void Write(string func, Exception exception)
        {
            Write(func + " : \r\n", exception);
        }

        public static void Write(Exception exception)
        {
            var message = "Message : " + exception.Message + "\r\n";

            //neu la loi SQL
            if (exception is SQLException ex)
            {
                var sqlException = ex;
                message += "SQLText : " + sqlException.CommandText + "\r\n";
                message += "SQLParameters : " + sqlException.Parameters + "\r\n";
            }

            message += "Source : " + exception.Source
                + "\r\nTargetSite : " + exception.TargetSite
                + "\r\nStackTrace : " + exception.StackTrace;

            Write(message);
        }

        public static void Write(string message)
        {
            var s = "Time : " + $"{DateTime.Now:dd/MM/yyyy hh:mm:ss}" + "\r\n";
            s += "IP : " + HttpContext.Current.Request.UserHostAddress + "\r\n";
            s += "URL : " + HttpContext.Current.Request.Url + "\r\n";
            s += message + "\r\n\r\n";

            //bo qua loi
            try
            {
                File.WriteText(PathCurrent + "/" + Year + "/" + Month + ".err", s);
            }
            catch
            {
                //ignored
            }
        }
    }
}