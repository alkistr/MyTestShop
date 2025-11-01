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
}
