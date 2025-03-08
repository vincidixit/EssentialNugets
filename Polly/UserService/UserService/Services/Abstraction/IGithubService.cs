using UserService.Services.Types;

namespace UserService.Services.Abstraction
{
    public interface IGithubService
    {
        Task<User> GetGithubUserDetails(string username);
    }
}
