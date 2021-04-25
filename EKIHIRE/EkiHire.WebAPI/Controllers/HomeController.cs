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
namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
		#region main home endpoints
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));
        public HomeController(IUserService userSvc, IRoleService roleSvc
            ,IAccountService accountService, ICategoryService categoryService
            //, ILog logger
            )
        {
            _userSvc = userSvc;
            _roleSvc = roleSvc;
            _accountService = accountService;
            _categoryService = categoryService;
            //this.logger = new LogManager("");
            log.Info("Juust testing!");
            //_employeeService = employeeService;
        }

        [Route("Index"), HttpGet]
        public IActionResult Index()
        {
            log.Info("Juust testing from index!");
            return Ok("EkiHire.Web Api is running");
        }
		
		[HttpGet]
        [Route("WhatToSell")]
        public async Task<IServiceResponse<IEnumerable<CategoryDTO>>> WhatToSell()
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<IEnumerable<CategoryDTO>>();

                var data = await _categoryService.GetCategories();
                

                response.Object = data;

                return response;
            });
        }

        #endregion
    }
}