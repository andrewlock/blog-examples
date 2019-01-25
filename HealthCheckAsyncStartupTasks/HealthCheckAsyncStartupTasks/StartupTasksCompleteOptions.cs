namespace HealthCheckAsyncStartupTasks
{
    public class StartupTasksCompleteOptions
    {
        /// <summary>
        /// The number of seconds to use for the Retry-After header
        /// when all startup tasks have not yet completed. Defaults to 30s.
        /// </summary>
        public int RetryAfterSeconds { get; set; } = 30;

        /// <summary>
        /// The response code to return when all startup tasks have not yet
        /// completed. Defaults to 503 (Service Unavailable).
        /// </summary>
        public int FailureResponseCode { get; set; } = 503;
    }
}