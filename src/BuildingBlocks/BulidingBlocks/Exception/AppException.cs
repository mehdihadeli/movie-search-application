namespace BuildingBlocks.Exception;

public class AppException : System.Exception
{
    public AppException(string message, string code = default!)
        : base(message)
    {
        Code = code;
    }

    public virtual string Code { get; }
}
