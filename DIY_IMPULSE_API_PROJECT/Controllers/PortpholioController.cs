using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.BAL.Repository;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Mvc;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortpholioController : Controller
    {
        private readonly IAuthServices _authService;
        private readonly ICommonRepo _commonRepo;
        private readonly ILogInRepository _logInRepository;
        private readonly IPortpholio _IPortpholioRepo;
        private readonly IBaseUrlServiceRepo _baseUrlService;
        private readonly IIpServiceRepo _ipService;
        public PortpholioController(ICommonRepo CommonRepo, IAuthServices AuthService, ILogInRepository ILogInRepository,IPortpholio IPortpholioRepo,IBaseUrlServiceRepo IBaseUrlServiceRepo, IIpServiceRepo IIpServiceRepo)
        {
            _commonRepo = CommonRepo;
            _authService = AuthService;
            _logInRepository = ILogInRepository;
            _IPortpholioRepo = IPortpholioRepo;
            _baseUrlService = IBaseUrlServiceRepo;
            _ipService = IIpServiceRepo;
        }

        [HttpPost,Route("SaveDataFromPortpholio")]
        public async Task<IActionResult> SaveDataFromPortpholio(PortpholioRequest PortpholioRequestObj)
        {
            var requestUrl = _baseUrlService.GetBaseUrl();
            var ipAddress = _ipService.GetClientIpAddress();

            if (requestUrl == null)
                return BadRequest("Unable to determine base URL");

            if (!Request.Headers.TryGetValue("Username", out var usernameHeader) ||
       !Request.Headers.TryGetValue("Password", out var passwordHeader))
            {
                return Unauthorized("Required headers missing");
            }

            AuthResponse ResponseObj = new AuthResponse();
            Responses ResponsesObj = new Responses();

            ResponseObj = _commonRepo.GetAuthenticationAPI(usernameHeader.ToString(), passwordHeader.ToString());

            if (ResponseObj.IsSuccess)
            {

                ResponsesObj = await _IPortpholioRepo.SaveDataFromPortfolio(PortpholioRequestObj, requestUrl,ipAddress);
                return Ok(ResponsesObj);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("DownloadResume")]
        public async Task<IActionResult> DownloadResume()
        {
            try
            {
                // 1. Define file path
                var resumeFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"RESUMES");
                var resumeFileName = "Rohit_Ram_Joijode_Resume.pdf";
                var fullPath = Path.Combine(resumeFolderPath, resumeFileName);

                // 2. Verify file exists
                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound("Resume file not found");
                }

                // 3. Read file into byte array
                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);

                // 4. Return file with correct content type
                return File(fileBytes, "application/pdf", resumeFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while downloading the resume");
            }
        }
    }
}
