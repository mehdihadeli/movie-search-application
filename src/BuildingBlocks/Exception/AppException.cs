namespace BuildingBlocks.Exception
{
    public class AppException : System.Exception
    {
        public virtual string Code { get; }
        public AppException(string message, string code = default!) : base(message)
        {
            Code = code;
        }
    }
}