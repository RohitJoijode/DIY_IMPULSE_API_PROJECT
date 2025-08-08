using DIY_IMPULSE_API_PROJECT.MODEL;

namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface ILogInRepository
    {
        public  Task<bool> RegisterUser(RegisterUserModel user);

        public Task<LoginResponse> Login(LogInUser user);
    }
}
