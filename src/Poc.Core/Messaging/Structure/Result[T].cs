using System;

namespace Poc.Core.Messaging.Structure
{
    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Okay(T data, string message = "", int statusCode = 200)
        {
            return new Result<T>
            {
                Succeed = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public new static Result<T> Error(string message = "", int statusCode = 500)
        {
            return new Result<T>
            {
                Succeed = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        public new static Result<T> Exception(Exception ex, int statusCode = 500)
        {
            return new Result<T>
            {
                Succeed = false,
                Message = ex.Message,
                StatusCode = statusCode
            };
        }

        public static implicit operator bool(Result<T> result) => result.Succeed;

        #region Overrides 

        public override bool Equals(object obj)
        {
            var sr = obj as Result<T>;

            if (sr == null)
            { return false; }

            if (Succeed && Data != null)
            {
                return Succeed == sr.Succeed
                   && StatusCode == sr.StatusCode
                   && Data.Equals(sr.Data);
            }

            return Succeed == sr.Succeed
                && StatusCode == sr.StatusCode;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Data == null)
                {
                    return Convert.ToInt32(Succeed)
                    + StatusCode;
                }
                else
                {
                    return Convert.ToInt32(Succeed)
                    + StatusCode
                    + Data.GetHashCode();
                }
            }
        }
        #endregion
    }
}
