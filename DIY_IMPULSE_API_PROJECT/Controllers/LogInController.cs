using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.BAL.Repository;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data;
using System.Xml;

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

        ///[Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserModel user)
        {
            if (await _logInRepository.RegisterUser(user))
            {
                return Ok("Successfully done");
            }
            return BadRequest("Something went wrong");
        }

        //[Authorize]
        [HttpPost,Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {

            if (!Request.Headers.TryGetValue("Username", out var usernameHeader) ||
       !Request.Headers.TryGetValue("Password", out var passwordHeader))
            {
                return Unauthorized("Required headers missing");
            }

            AuthResponse ResponseObj = new AuthResponse();
            ResponseObj = _commonRepo.GetAuthenticationAPI(usernameHeader.ToString(), passwordHeader.ToString());

            if (ResponseObj.IsSuccess)
            {
                LoginResponse LoginResponseObj = new LoginResponse();
                LoginResponseObj = await _logInRepository.RefreshToken(model);
                if (LoginResponseObj.IsLogedIn)
                {
                    return Ok(LoginResponseObj);
                }
            }
            
            return Unauthorized();
        }

        [HttpPost, Route("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {

            if (!Request.Headers.TryGetValue("Username", out var usernameHeader) ||
       !Request.Headers.TryGetValue("Password", out var passwordHeader))
            {
                return Unauthorized("Required headers missing");
            }

            AuthResponse ResponseObj = new AuthResponse();
            LoginResponse LoginResponseObj = new LoginResponse();

            ResponseObj = _commonRepo.GetAuthenticationAPI(usernameHeader.ToString(), passwordHeader.ToString());

            if (ResponseObj.IsSuccess)
            {
                DataTable datatable = await _commonRepo.GetDashboardData("1");
                return Ok(datatable);
            }
            else
            {
                return BadRequest();
            }
        }

        
    }
}
