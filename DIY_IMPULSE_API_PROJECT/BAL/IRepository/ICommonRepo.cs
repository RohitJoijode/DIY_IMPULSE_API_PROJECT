using DIY_IMPULSE_API_PROJECT.MODEL;
using System.Data;

namespace DIY_IMPULSE_API_PROJECT.BAL.IRepository
{
    public interface ICommonRepo 
    {
        public AuthResponse GetAuthenticationAPI(string UserName, string Password);

        public Task<DataTable> GetDashboardData(string UserId);

    }
}
