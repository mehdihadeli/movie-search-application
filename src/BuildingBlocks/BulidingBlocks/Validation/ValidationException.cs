namespace BuildingBlocks.Validation;

public class ValidationException : System.Exception
{
    public ValidationException(ValidationResultModel validationResultModel)
    {
        ValidationResultModel = validationResultModel;
    }

    public ValidationResultModel ValidationResultModel { get; }
}
