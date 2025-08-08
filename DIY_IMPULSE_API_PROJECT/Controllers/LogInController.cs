using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.BAL.Repository;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogInController : Controller
    {
        private readonly IAuthServices _authService;
        private readonly ICommonRepo _commonRepo;
        private readonly ILogInRepository _logInRepository;
        

        public LogInController(ICommonRepo CommonRepo, IAuthServices AuthService,ILogInRepository ILogInRepository)
        {
            _commonRepo = CommonRepo;
            _authService = AuthService;
            _logInRepository = ILogInRepository;
        }

        [HttpPost,Route("UserLogIn")]
        public async Task<IActionResult> LogIn(LogInUser user)
        {

            if (!Request.Headers.TryGetValue("Username", out var usernameHeader) ||
       !Request.Headers.TryGetValue("Password", out var passwordHeader))
            {
                return Unauthorized("Required headers missing");
            }

            AuthResponse  ResponseObj = new AuthResponse();
            LoginResponse LoginResponseObj = new LoginResponse();
            
            ResponseObj =  _commonRepo.GetAuthenticationAPI(usernameHeader.ToString(),passwordHeader.ToString());

            if (ResponseObj.IsSuccess)
            {
                LoginResponseObj = await _logInRepository.Login(user);
                return Ok(LoginResponseObj);
            } else
            {
                return BadRequest();
            }
        }
    }
}
