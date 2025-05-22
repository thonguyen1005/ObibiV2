using System.IO;

namespace VSW.Website.Global
{
    public static class Error
    {
        public static string getError(int StatusCode)
        {
            switch (StatusCode)
            {
                case 400:
                    return "400 - Bad Request";
                    break;
                case 401:
                    return "401 - Unauthorized";
                    break;
                case 403:
                    return "403 - Forbidden";
                    break;
                case 404:
                    return "404 - File not found";
                    break;
                case 405:
                    return "405 - Method Not Allowed";
                    break;
                case 406:
                    return "406 - Not Acceptable";
                    break;
                case 407:
                    return "407 - Proxy Authentication Required";
                    break;
                case 412:
                    return "412 - Precondition Failed";
                    break;
                case 414:
                    return "414 - Request-URI Too Long";
                    break;
                case 415:
                    return "415 - Unsupported Media Type";
                    break;
                case 500:
                    return "500 - Server error";
                    break;
                case 501:
                    return "501 - Not Implemented";
                    break;
                case 503:
                    return "503 - Service Temporarily Unavailable";
                    break;
                default:
                    return "500 - Server error";
                    break;
            }
        }
    }
}

