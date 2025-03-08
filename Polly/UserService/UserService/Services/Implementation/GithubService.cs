using Microsoft.Extensions.Options;
using Polly;
using System.Text.Json;
using UserService.Helpers;
using UserService.Services.Abstraction;
using UserService.Services.Types;

namespace UserService.Services.Implementation
{
    public class GithubService : IGithubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly AsyncPolicy _policy;

        public GithubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            var policy = configuration["PollyPolicy"];

            if (!string.IsNullOrWhiteSpace(policy))
            {
                if (policy == "RetryPolicy")
                {
                    _policy = PollyPolicyProvider.GetRetryPolicy(Convert.ToInt32(configuration["RetryPolicy:RetryCount"]));
                }
                else if (policy == "WaitAndRetryPolicy")
                {
                    _policy = PollyPolicyProvider.GetWaitAndRetryPolicy(Convert.ToInt32(configuration["WaitAndRetryPolicy:RetryCount"]), Convert.ToInt32(configuration["WaitAndRetryPolicy:SleepDuration"]));
                }
            }
        }

        public async Task<User> GetGithubUserDetails(string username)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.BaseAddress = new Uri("https://api.github.com");

                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");

                    var response = await _policy.ExecuteAsync(async () => await httpClient.GetAsync($"/users/{username}"));

                    // Throw an exception if the API response is not successful (not in 200-299 range)
                    response.EnsureSuccessStatusCode();

                    var stringContent = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<User>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Exception occurred while retrieving user details from Github API. Exception Message: {ex.Message}");
            }

            return null;
        }
    }
}
