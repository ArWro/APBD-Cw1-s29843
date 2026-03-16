namespace APBD_Cw1.Services;

public class Result
{
    private Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; }
    public string Message { get; }

    public static Result Success(string message) => new(true, message);
    public static Result Failure(string message) => new(false, message);
}