using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.BAL.Repository;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Mvc;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // This adds the base route
    public class AuthController : Controller
    {
        private readonly IAuthServices _authService;
        private readonly DBEngine _DbEngine;
        private readonly ILogInRepository _ILogInRepository;
        
        public AuthController(AuthServices authService,DBEngine DbEngine,ILogInRepository iLogInRepository)
        {
            _authService = authService;
            _DbEngine = DbEngine;
            _ILogInRepository = iLogInRepository;
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RegisterUserModel model)
        {
            bool loginResult = await _ILogInRepository.RegisterUser(model);
            if (loginResult)
            {
                return Ok(loginResult);
            }
            return Unauthorized();
        }
    }
}
