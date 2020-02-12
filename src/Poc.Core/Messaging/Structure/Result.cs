using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.Core.Messaging.Structure
{
    public class Result
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static Result Okay(string message = "", int statusCode = 200)
        {
            return new Result
            {
                Succeed = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result Error(string message = "", int statusCode = 500)
        {
            return new Result
            {
                Succeed = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result Exception(Exception ex, int statusCode = 500)
        {
            return new Result
            {
                Succeed = false,
                Message = ex.Message,
                StatusCode = statusCode
            };
        }

        public static implicit operator bool(Result result) => result.Succeed;

        #region Overrides 

        public override bool Equals(object obj)
        {
            var sr = obj as Result;

            if (sr == null)
            { return false; }


            return Succeed == sr.Succeed && StatusCode == sr.StatusCode;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Convert.ToInt32(Succeed) + StatusCode;
            }
        }

        #endregion
    }
}
