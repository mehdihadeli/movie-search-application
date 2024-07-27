using System.Collections.Generic;

namespace BuildingBlocks.Exception;

public class InvalidCommandException
{
    public InvalidCommandException(List<string> errors)
    {
        Errors = errors;
    }

    public List<string> Errors { get; }
}
