using Microsoft.AspNetCore.Http;

namespace SFB.Shared.Backend.Helpers
{
    public class ControllerException : Exception
    {
        public dynamic ErrorData { get; set; }
        public int CodeStatus { get; set; } = StatusCodes.Status200OK;
        public bool Status { get; set; } = false;

        public ControllerException(string message, dynamic customData) : base(message)
        {
            ErrorData = customData;
        }

        public ControllerException(string? message) : base(message)
        {

        }

        public ControllerException(int statusCodes, string? message) : base(message)
        {
            CodeStatus = statusCodes;
        }
    }
}
