using Azure;
using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Reflection.PortableExecutable;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class PortpholioRepo : IPortpholio
    {
        private readonly IConfiguration _IConfiguration;
        private readonly ICommonRepo _ICommonRepo;
        public PortpholioRepo(IConfiguration iConfiguration,ICommonRepo ICommonRepo)
        {
            _IConfiguration = iConfiguration;
            _ICommonRepo = ICommonRepo;
            
        }

        public async Task<Responses> SaveDataFromPortfolio(PortpholioRequest portfolioRequestObj,string baseUrl,string Ip)
        {
            var response = new Responses();

            try
            {
                var connectionString = _IConfiguration.GetConnectionString("ConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured");
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("usp_SaveEnquiryPortpholio", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters with null checks
                        command.Parameters.AddWithValue("@Name", portfolioRequestObj.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EmailId", portfolioRequestObj.EmailId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Message", portfolioRequestObj.Message ?? (object)DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Safe column reading with null checks
                                    var isSuccessOrdinal = reader.GetInt32("IsSuccess");
                                    var messageOrdinal = reader.GetOrdinal("Message");

                                    if(reader.IsDBNull(isSuccessOrdinal) || isSuccessOrdinal == 0)
                                    {
                                        response.IsSuccess = false;
                                    }else
                                    {
                                        response.IsSuccess = true;
                                    }

                                        response.Message = reader.IsDBNull(messageOrdinal)
                                            ? string.Empty
                                            : reader.GetString(messageOrdinal);
                                }
                            }
                            else
                            {
                                response.IsSuccess = false;
                                response.Message = "No data returned from stored procedure";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                response.IsSuccess = false;
                response.Message = $"Database operation failed: {ex.Message}";
                // Log the exception here

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An unexpected error occurred: {ex.Message}";
                // Log the exception here

            }
            finally
            {
                var requestObj = JsonConvert.SerializeObject(portfolioRequestObj);
                var responseObj = JsonConvert.SerializeObject(response);
                await _ICommonRepo.SaveRequestHistory(portfolioRequestObj.EmailId, requestObj,responseObj,baseUrl,Ip);
            }

            return response;
        }

        
    }
}
