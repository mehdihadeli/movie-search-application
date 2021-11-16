using System.Collections.Generic;

namespace BuildingBlocks.Exception
{
    public class InvalidCommandException
    {
        public List<string> Errors { get; }

        public InvalidCommandException(List<string> errors)
        {
            this.Errors = errors;
        }
    }
}