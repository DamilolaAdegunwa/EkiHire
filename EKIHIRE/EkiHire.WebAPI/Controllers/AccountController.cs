using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.WebAPI.Utils;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EkiHire.Business.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace EkiHire.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        #region main account endpoints
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;
        //private readonly IEmployeeService _employeeService;

        public AccountController(IUserService userSvc, IRoleService roleSvc/*, IEmployeeService employeeService*/)
        {
            _userSvc = userSvc;
            _roleSvc = roleSvc;
            //_employeeService = employeeService;
        }

        private async Task<List<Claim>> GetUserIdentityClaims(User user)
        {
            var userClaims = user.UserToClaims();

            var roles = await _userSvc.GetUserRoles(user);

            foreach (var item in roles)
            {

                userClaims.Add(new Claim(JwtClaimTypes.Role, item));

                var roleClaims = await _roleSvc.GetClaimsAsync(item);
                userClaims.AddRange(roleClaims);
            }
            //var userRow = await _employeeService.GetEmployeesByemailAsync(user.Email);
            var userRow = await _userSvc.FindByEmailAsync(user.Email);
            if (userRow != null)
            {
                userClaims.Add(new Claim("location", userRow.UserName));
            }
            return userClaims;
        }

        [HttpGet]
        [Route("GetCurrentUserClaims")]
        public async Task<IServiceResponse<IEnumerable<Claim>>> GetCurrentUserClaims()
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<IEnumerable<Claim>>();

                var user = await _userSvc.FindByNameAsync(User?.FindFirst(JwtClaimTypes.Name)?.Value);

                var claims = await GetUserIdentityClaims(user);

                response.Object = claims;

                return response;
            });
        }

        [HttpGet]
        [Route("GetProfile")]
        public async Task<IServiceResponse<UserProfileDTO>> GetCurrentUserProfile()
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<UserProfileDTO>();

                var profile = await _userSvc.GetProfile(User.FindFirst(JwtClaimTypes.Name)?.Value);
                response.Object = profile;
                return response;
            });
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public async Task<IServiceResponse<bool>> UpdatetUserProfile(UserProfileDTO model)
        {
            return await HandleApiOperationAsync(async () => {

                var result = await _userSvc.UpdateProfile(User.FindFirst(JwtClaimTypes.Name)?.Value, model);

                return new ServiceResponse<bool>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Activate")]/*verify and activate or deny*/
        public async Task<ServiceResponse<UserDTO>> Activate(string usernameOrEmail, string activationCode)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ActivateAccount(usernameOrEmail, activationCode);
                return new ServiceResponse<UserDTO>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword/{usernameOrEmail}")]
        public async Task<ServiceResponse<bool>> ForgotPassword(string usernameOrEmail)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ForgotPassword(usernameOrEmail);
                return new ServiceResponse<bool>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ServiceResponse<bool>> ResetPassword(PassordResetDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ResetPassword(model);
                return new ServiceResponse<bool>(result);
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ServiceResponse<bool>> ChangePassword(ChangePassordDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ChangePassword(User.FindFirst(JwtClaimTypes.Name)?.Value, model);
                return new ServiceResponse<bool>(result);
            });
        }

        [HttpPost]
        [Route("Add")]/*SignUp - Create your account*/
        public async Task<IServiceResponse<bool>> AddUser(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {
                await _userSvc.CreateAsync(user);
                return new ServiceResponse<bool>(true);
            });
        }
        [HttpPost]
        [Route("ContinueWithFacebook")]/**/
        public async Task<IServiceResponse<bool>> ContinueWithFacebook(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }
        [HttpPost]
        [Route("ContinueWithGmail")]/**/
        public async Task<IServiceResponse<bool>> ContinueWithGmail(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }
        [HttpPost]
        [Route("ContinueWithLinkedIn")]/**/
        public async Task<IServiceResponse<bool>> ContinueWithLinkedIn(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }
        [HttpGet]
        [Route("PrivacyPolicy")]/**/
        public async Task<IServiceResponse<string>> PrivacyPolicy()
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<string>(null);
            });
        }
        [HttpGet]
        [Route("TermsOfService")]/**/
        public async Task<IServiceResponse<string>> TermsOfService()
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<string>(null);
            });
        }
        [HttpPost]
        [Route("ResendVerificationCode")]
        public async Task<IServiceResponse<bool>> ResendVerificationCode(string username)
        {
            
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ResendVerificationCode(username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        [HttpPost]
        [Route("Signin")]

        #endregion

        #region default from boilerplate
        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion
    }
}
