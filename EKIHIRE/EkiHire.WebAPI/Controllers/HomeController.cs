using EkiHire.Business.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;

namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        #region main home endpoints
        //private readonly IUserService _userSvc;
        //private readonly IRoleService _roleSvc;
        //private readonly IAccountService _accountService;
        //private readonly ICategoryService _categoryService;
        private readonly IChatHub _chatHub;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        public HomeController(IChatHub chatHub
        //IUserService userSvc, IRoleService roleSvc
        //    , IAccountService accountService, ICategoryService categoryService
            )
        {
            //_userSvc = userSvc;
            //_roleSvc = roleSvc;
            //_accountService = accountService;
            //_categoryService = categoryService;
            _chatHub = chatHub;
        }

        [Route("Index"), HttpGet]
        public async Task<IActionResult> Index()
        {
            //await new EkiHire.Business.Services.RealTime.ChatClient().TestRealTimeFunc();
            //await _chatHub.SendNotification(new Notification());
            log.Info("This app is working!");
            return Ok("EkiHire.Web Api is running");
        }
        #endregion
    }
}