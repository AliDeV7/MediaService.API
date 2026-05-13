namespace MediaService.Core.Common
{
    /// <summary>
    /// Standard API response wrapper for all endpoints
    /// </summary>
    /// <typeparam name="T">Type of the response data</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the request was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response data (null if failed)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Error details (null if successful)
        /// </summary>
        public ErrorDetails? Error { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Error = null
            };
        }

        /// <summary>
        /// Creates a failure response
        /// </summary>
        public static ApiResponse<T> FailureResponse(string errorCode, string message, Dictionary<string, string[]>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Error = new ErrorDetails
                {
                    Code = errorCode,
                    Message = message,
                    Errors = errors
                }
            };
        }
    }

    /// <summary>
    /// Error details for failed responses
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Machine-readable error code
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable error message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Validation errors (field-level errors)
        /// </summary>
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
