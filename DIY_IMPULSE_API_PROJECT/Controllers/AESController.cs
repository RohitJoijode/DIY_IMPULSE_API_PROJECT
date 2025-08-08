using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AESController : ControllerBase
    {
        private readonly IAESEncription _IAESEncriptionRepo;
        public AESController(IAESEncription IAESEncriptionRepo)
        {
            _IAESEncriptionRepo = IAESEncriptionRepo;
        }
        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] string plainText)
        {
            try
            {
                var encryptedText = _IAESEncriptionRepo.AESEncrypt(plainText);
                return Ok(new { EncryptedText = encryptedText });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] string cipherText)
        {
            try
            {
                var decryptedText = _IAESEncriptionRepo.AESDecrypt(cipherText);
                return Ok(new { DecryptedText = decryptedText });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
