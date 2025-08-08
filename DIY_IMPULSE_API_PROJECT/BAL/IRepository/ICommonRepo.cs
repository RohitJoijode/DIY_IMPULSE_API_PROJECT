using DIY_IMPULSE_API_PROJECT.MODEL;

namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface ICommonRepo
    {
        public AuthResponse GetAuthenticationAPI(string UserName, string Password);
    }
}
