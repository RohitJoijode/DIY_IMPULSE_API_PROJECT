using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.BAL.Repository;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DIY_IMPULSE_API_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        private readonly IAuthServices _authService;
        private readonly DBEngine _DbEngine;
        private readonly ILogInRepository _ILogInRepository;
        private readonly ICommonRepo _ICommonRepo;

        public CommonController(ICommonRepo ICommonRepo, AuthServices authService, DBEngine DbEngine, ILogInRepository iLogInRepository)
        {
            _authService = authService;
            _DbEngine = DbEngine;
            _ILogInRepository = iLogInRepository;
            _ICommonRepo = ICommonRepo;
        }

        
    }
}
