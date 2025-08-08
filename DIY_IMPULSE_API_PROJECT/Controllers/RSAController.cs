using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSAController : Controller
    {
        private readonly DBEngine _DbEngine;
        private readonly IRSAHelperRepo _RsaHelperRepo;
        public RSAController(DBEngine DbEngine,IRSAHelperRepo RsaHelperRepo)
        {
            _DbEngine = DbEngine;
            _RsaHelperRepo = RsaHelperRepo;
        }

        [HttpPost, Route("EncryptApi")]
        public IActionResult EncryptApi(string EncryptRequestModalObj)
        {
            var VendorName = Request.Headers["VendorName"].ToString();
            var VendorApiKey = Request.Headers["VendorApiKey"].ToString();
            var VendorApiToken = Request.Headers["VendorApiToken"].ToString();
            dynamic RequestObject = null, ResponseObject = null, ReturnData = null;
            try
            {
                ReturnData = _RsaHelperRepo.Encrypt(EncryptRequestModalObj);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }
            return Ok(ReturnData);
        }

        [HttpPost, Route("DecryptApi")]
        public IActionResult DecryptApi(string EncryptRequestModalObj)
        {
            var VendorName = Request.Headers["VendorName"].ToString();
            var VendorApiKey = Request.Headers["VendorApiKey"].ToString();
            var VendorApiToken = Request.Headers["VendorApiToken"].ToString();
            dynamic RequestObject = null, ResponseObject = null, ReturnData = null;
            
            try
            {
                ReturnData = _RsaHelperRepo.Decrypt(EncryptRequestModalObj);
            }
            catch (Exception ex)
            {
                ReturnData = JsonSerializer.Serialize("Something went Wrong !!!");
            
            }
            finally
            {

            }
            return Ok(ReturnData);
        }
    }
}
