using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public class Result
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string ErrorContent { get; set; }

        public string Extention { get; set; }

        public Result(string code, string message, string errorContent = null)
        {
            Code = code;
            Message = message;
            ErrorContent = errorContent;
        }

        public Result()
        {
            Code = ResultExtensions.CODE_OK;
        }

        public static Result Ok()
        {
            return new Result(ResultExtensions.CODE_OK, "Ok");
        }

        public static Result Error(string code, string message)
        {
            return new Result(code, message);
        }

        public static Result Error(string message)
        {
            return new Result(ResultExtensions.UNKNOW_CODE, message);
        }

        public static Result Exception(string message, Exception ex)
        {
            return new Result(ResultExtensions.EXCEPTION_CODE, message, ex.ToString());
        }

        public static Result<TData> Ok<TData>(TData data, string message = null)
        {
            return new Result<TData>(ResultExtensions.CODE_OK, message, data);
        }

        public static Result<TData> Error<TData>(string code, string message, TData data)
        {
            return new Result<TData>(code, message, data);
        }

        public static Result<TData> Error<TData>(string message, TData data)
        {
            return new Result<TData>(ResultExtensions.UNKNOW_CODE, message, data);
        }

        public static Result<TData> Error<TData>(string message)
        {
            return new Result<TData>(ResultExtensions.UNKNOW_CODE, message);
        }

        public static Result<TData> ErrorWithData<TData>(string message, TData data)
        {
            return new Result<TData>(ResultExtensions.UNKNOW_CODE, message, data);
        }

        public static Result<TData> Exception<TData>(string message, Exception ex)
        {
            return new Result<TData>(ResultExtensions.EXCEPTION_CODE, message, ex.ToString());
        }

        public static Result<TData> ToResultWithData<TData>(Result result)
        {
            return new Result<TData>(result.Code, result.Message, result.ErrorContent);
        }
    }

    public class Result<TData> : Result
    {
        public TData Data { get; set; }

        public Result(string code, string message, TData data = default) : base(code, message)
        {
            Data = data;
        }

        public Result(string code, string message, string errorContent) : base(code, message, errorContent)
        {
        }

        public Result() : base()
        {

        }
    }

    public static class ResultExtensions
    {
        public const string CODE_OK = "00";
        public const string UNKNOW_CODE = "98";
        public const string EXCEPTION_CODE = "99";

        public static Result<TData> As<TData>(this Result obj)
        {
            var rs = new Result<TData>(obj.Code, obj.Message);
            return rs;
        }


        public static bool IsOk(this Result result)
        {
            return result.Code == CODE_OK;
        }

        public static bool IsException(this Result result)
        {
            return result.Code == UNKNOW_CODE;
        }

        public static bool IsError(this Result result)
        {
            return result.Code != CODE_OK;
        }

        public static void ThrowException(this Result result)
        {
            var message = result.Message;
            if(result.ErrorContent.IsNotEmpty())
            {
                message += Environment.NewLine + result.ErrorContent;
            }    

            throw new Exception(message);
        }

        public static Result<TData> WithData<TData>(this Result result, TData data = default)
        {
            var rs = new Result<TData>(result.Code, result.Message, result.ErrorContent);
            rs.Data = data;
            return rs;
        }
    }
}
