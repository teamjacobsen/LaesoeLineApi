using System;

namespace LaesoeLineApi
{
    public class ApiException : Exception
    {
        public ApiStatus Status { get; private set; }

        public ApiException(ApiStatus status)
        {
            Status = status;
        }
    }
}
