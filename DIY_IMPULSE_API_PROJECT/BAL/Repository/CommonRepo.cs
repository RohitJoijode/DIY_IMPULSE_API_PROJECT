using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using DIY_IMPULSE_API_PROJECT.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class CommonRepo :ICommonRepo
    {
        private readonly DBENGINE.DBEngine _dbEngine;
        private readonly IConfiguration _IConfiguration;
        public CommonRepo(DBENGINE.DBEngine DbEngine,IConfiguration iConfiguration)
        {
            _dbEngine = DbEngine;
            _IConfiguration = iConfiguration;
        }
        public AuthResponse GetAuthenticationAPI(string UserName, string Password)
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

        public async Task<DataTable> GetDashboardData(string UserId)
        {
            var connectionString = _IConfiguration.GetConnectionString("ConnectionString");
            var dataTable = new DataTable();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("GetDashboardData",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId",UserId);
                
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }


        public async Task<DataTable> SaveRequestHistory(string? UserId,object Request,object Response,string baseUrls,string IP)
        {
            var dataTable = new DataTable();
            try
            {
                var connectionString = _IConfiguration.GetConnectionString("ConnectionString");
                

                using (var connection = new SqlConnection(connectionString))
                {
                    var command = new SqlCommand("usp_SaveRequestHistory", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@BaseUrl", baseUrls);
                    command.Parameters.AddWithValue("@Request", Request);
                    command.Parameters.AddWithValue("@Response", Response);
                    command.Parameters.AddWithValue("@IP", IP);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            } catch(Exception ex)
            {

            }
            

            return dataTable;
        }


    }
}
