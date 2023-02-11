namespace SupportLibrary.HttpClient.Options
{
    public class RetryOptions
    {
        public RetryOptions() { }

        public RetryOptions(int retryCount, TimeSpan sleepDurationMilliseconds)
        {
            RetryCount = retryCount;
            SleepDurationMilliseconds = sleepDurationMilliseconds;
        }

        public static RetryOptions Default { get; private set; } = new RetryOptions(3, TimeSpan.FromMilliseconds(1000));

        public int RetryCount { get; set; }
        public TimeSpan SleepDurationMilliseconds { get; set; }
    }
}