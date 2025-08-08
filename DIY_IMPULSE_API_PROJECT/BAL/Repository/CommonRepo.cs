using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using DIY_IMPULSE_API_PROJECT.Services;
using Microsoft.Data.SqlClient;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class CommonRepo : IRepository.ICommonRepo
    {
        private readonly DBENGINE.DBEngine _dbEngine;
        public CommonRepo(DBENGINE.DBEngine DbEngine)
        {
            _dbEngine = DbEngine;
        }
        public AuthResponse GetAuthenticationAPI(string UserName,string Password)
        {
            AuthResponse ResponseObj = new AuthResponse();
            List<Response<object>> ResponseList = new List<Response<object>>();

            try
            {
                SqlParameter[] parameters =   {
                                                new SqlParameter("@UserName",UserName),
                                                new SqlParameter("@Password",Password),
                                            };


                return _dbEngine.SqlQuery<AuthResponse>("GetAuthenticationAPI @UserName, @Password", parameters).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ResponseObj;
        }
    }
}
