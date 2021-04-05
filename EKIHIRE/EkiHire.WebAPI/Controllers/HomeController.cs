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
namespace EkiHire.WebAPI.Controllers
{
    public class HomeController : BaseController
    {
		#region main home endpoints
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;

        public HomeController(IUserService userSvc, IRoleService roleSvc
            ,IAccountService accountService, ICategoryService categoryService
            /*, IEmployeeService employeeService*/)
        {
            _userSvc = userSvc;
            _roleSvc = roleSvc;
            _accountService = accountService;
            _categoryService = categoryService;
            //_employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
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