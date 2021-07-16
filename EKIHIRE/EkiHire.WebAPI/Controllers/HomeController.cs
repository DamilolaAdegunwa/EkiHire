using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Model;
using EkiHire.WebAPI.Utils;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

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