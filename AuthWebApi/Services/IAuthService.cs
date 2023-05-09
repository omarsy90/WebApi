using AuthWebApi.Models;

namespace AuthWebApi.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model);

        public Task<AuthModel> LoginAsync(LoginModel model);
    }
}
