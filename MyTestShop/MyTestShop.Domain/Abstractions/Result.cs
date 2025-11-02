namespace MyTestShop.Domain.Abstractions
{
    public class Result
    {
        public Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }

        public string Message { get; }

        public static Result Success(string message) => new(true, message);

        public static Result Failure(string message) => new(false, message);
    }

    public class Result<T>
    {
        public Result(bool isSuccess, string message, T value)
        {
            IsSuccess = isSuccess;
            Message = message;
            Value = value;
        }

        public bool IsSuccess { get; }

        public string Message { get; }

        public T Value { get; init; }

        public static Result<T> Success(string message, T value) => new(true, message, value);

        public static Result<T> Failure(string message) => new(false, message, default);
    }

}
