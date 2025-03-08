using Polly;

namespace UserService.Helpers
{
    public static class PollyPolicyProvider
    {   
        public static AsyncPolicy GetRetryPolicy(int maxRetries)
        {
            return Policy.Handle<HttpRequestException>()
                            .RetryAsync(maxRetries, 
                                        onRetry: (exception, count) =>
                                        {
                                            Console.WriteLine($"Exception occurred. Retrying {count}/{maxRetries}");
                                        });
        }

        public static AsyncPolicy GetWaitAndRetryPolicy(int maxRetries, int sleepDurationInSeconds)
        {
            return Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(maxRetries,
                                                sleepDurationProvider: (retryCount) => TimeSpan.FromMilliseconds(retryCount * sleepDurationInSeconds),
                                                onRetry: (exception, timeSpan) =>
                                                {
                                                    Console.WriteLine($"Exception occurred. Will retry in {timeSpan}.");
                                                });
        }



    }
}
