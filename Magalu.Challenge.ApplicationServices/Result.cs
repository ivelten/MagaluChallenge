using System;

namespace Magalu.Challenge.ApplicationServices
{
    public enum ResultType
    {
        NotFound,
        Success,
        BadRequest,
        Forbidden
    }

    public struct Result
    {
        public string ErrorMessage { get; private set; }

        public bool IsSuccess => Type == ResultType.Success;

        public bool IsError => Type == ResultType.BadRequest;

        public bool IsNotFound => Type == ResultType.NotFound;

        public bool IsForbidden => Type == ResultType.Forbidden;

        public ResultType Type { get; private set; }

        public static Result Success()
        {
            return new Result { Type = ResultType.Success };
        }

        public static Result BadRequest(string message)
        {
            return new Result { Type = ResultType.BadRequest, ErrorMessage = message };
        }

        public static Result NotFound()
        {
            return new Result { Type = ResultType.NotFound };
        }

        public static Result Forbidden()
        {
            return new Result { Type = ResultType.Forbidden };
        }
    }

    public struct Result<T>
    {
        public string ErrorMessage { get; private set; }

        public bool IsSuccess => Type == ResultType.Success;

        public bool IsError => Type == ResultType.BadRequest;

        public bool IsNotFound => Type == ResultType.NotFound;

        public bool IsForbidden => Type == ResultType.Forbidden;

        public ResultType Type { get; private set; }

        private T value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException($"Value can not be retrieved, since the result {Type} does not generate a value.");

                return value;
            }

            private set
            {
                this.value = value;
            }
        }

        public static Result<T> Success(T value)
        {
            return new Result<T> { Type = ResultType.Success, Value = value };
        }

        public static Result<T> BadRequest(string message)
        {
            return new Result<T> { Type = ResultType.BadRequest, ErrorMessage = message };
        }

        public static Result<T> NotFound()
        {
            return new Result<T> { Type = ResultType.NotFound };
        }

        public static Result<T> Forbidden()
        {
            return new Result<T> { Type = ResultType.Forbidden };
        }
    }
}
