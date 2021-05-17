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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        #region main account endpoints
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;
        private readonly IAccountService _accountService;

        public AccountController(IUserService userSvc, IRoleService roleSvc
            ,IAccountService accountService
            /*, IEmployeeService employeeService*/)
        {
            _userSvc = userSvc;
            _roleSvc = roleSvc;
            _accountService = accountService;
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
        public async Task<IServiceResponse<UserDTO>> GetCurrentUserProfile()
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<UserDTO>();

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
        [Route("Activate")]/*verify and activate/deny account - checked*/
        public async Task<ServiceResponse<UserDTO>> Activate(string username, string activationCode)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ActivateAccount(username, activationCode);
                return new ServiceResponse<UserDTO>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword/{username}")] /*Indicate Password Forget and get OTP for new - checked*/
        public async Task<ServiceResponse<bool>> ForgotPassword(string username)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ForgotPassword(username);
                return new ServiceResponse<bool>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]/*remove old password and input new password - checked*/
        public async Task<ServiceResponse<bool>> ResetPassword(PassordResetDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ResetPassword(model);
                return new ServiceResponse<bool>(result);
            });
        }

        [HttpPost]
        [Route("ChangePassword")]/*create new password*/
        public async Task<ServiceResponse<bool>> ChangePassword(ChangePassordDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ChangePassword(User.FindFirst(JwtClaimTypes.Name)?.Value, model);
                return new ServiceResponse<bool>(result);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]/*SignUp - Create your account */
        public async Task<IServiceResponse<bool>> SignUp(LoginViewModel loginModel)
        {
            return await HandleApiOperationAsync(async () => {
                await _accountService.SignUp(loginModel);
                return new ServiceResponse<bool>(true);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ContinueWithFacebook")]/*Login via Facebook*/
        public async Task<IServiceResponse<bool>> ContinueWithFacebook(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ContinueWithGmail")]/*Login via Gmail*/
        public async Task<IServiceResponse<bool>> ContinueWithGmail(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ContinueWithLinkedIn")]/*Login via LinkedIn*/
        public async Task<IServiceResponse<bool>> ContinueWithLinkedIn(UserDTO user)
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<bool>(true);
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PrivacyPolicy")]/*Our Privacy Policy*/
        public async Task<IServiceResponse<string>> PrivacyPolicy()
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<string>(null);
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("TermsOfService")]/*Our Terms Of Service*/
        public async Task<IServiceResponse<string>> TermsOfService()
        {
            return await HandleApiOperationAsync(async () => {

                return new ServiceResponse<string>(null);
            });
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("ValidatePassordResetCode")] /*checked */
        public async Task<IServiceResponse<bool>> ValidatePassordResetCode(PassordResetDTO model)
		{
			return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.ValidatePassordResetCode(model);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
		}
        //      [AllowAnonymous]
        //[HttpGet]
        //[Route("TestEmail")]
        //public async Task<IServiceResponse<bool>> TestEmail()
        //{
        //          //
        //          #region email test
        //          //var fromAddress = new MailAddress("contact@ekihire.com", "EkiHire");
        //          //var toAddress = new MailAddress("damee1993@gmail.com", "Damilola Adegunwa");
        //          //const string fromPassword = "ekihireapp1";
        //          //const string subject = "My Email Test";
        //          //const string body = "This is awesome!!";

        //          //var smtp = new SmtpClient
        //          //{
        //          //    Host = "smtp.gmail.com",
        //          //    Port = 587,
        //          //    EnableSsl = true,
        //          //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //          //    UseDefaultCredentials = false,
        //          //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //          //};
        //          //using (var message = new MailMessage(fromAddress, toAddress)
        //          //{
        //          //    Subject = subject,
        //          //    Body = body
        //          //})
        //          //{
        //          //    smtp.Send(message);
        //          //}
        //          #endregion
        //          //
        //          return await HandleApiOperationAsync(async () => {
        //              var result = await _userSvc.TestEmail();
        //              var response = new ServiceResponse<bool>(result);
        //              return response;
        //          });
        //}
        #endregion

        #region profile endpoints
        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpGet]
        [Route("GetProfile/{username}")]
        public async Task<IServiceResponse<UserDTO>> GetProfile(string username)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userSvc.GetProfile(username);
                var response = new ServiceResponse<UserDTO>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpPost]
        [Route("ChangeName")]
        public async Task<IServiceResponse<bool>> ChangeName(Name name)
        {
            return await HandleApiOperationAsync(async () => {
                var username = User.FindFirst(JwtClaimTypes.Name)?.Value;
                var result = await _userSvc.ChangeName(name, username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpPost]
        [Route("ChangeGender")]
        public async Task<IServiceResponse<bool>> ChangeGender(Gender gender)
        {
            return await HandleApiOperationAsync(async () => {
                var username = User.FindFirst(JwtClaimTypes.Name)?.Value;
                bool result = await _userSvc.ChangeGender(gender, username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpPost]
        [Route("ChangeBirthday")]
        public async Task<IServiceResponse<bool>> ChangeBirthday(DateTime birthdate)
        {
            return await HandleApiOperationAsync(async () => {
                var username = User.FindFirst(JwtClaimTypes.Name)?.Value;
                bool result = await _userSvc.ChangeBirthday(birthdate, username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpPost]
        [Route("ChangeEmail")]
        public async Task<IServiceResponse<bool>> ChangeEmail(string userEmail)
        {
            return await HandleApiOperationAsync(async () => {
                var username = User.FindFirst(JwtClaimTypes.Name)?.Value;
                bool result = await _userSvc.ChangeEmail(userEmail, username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        [HttpPost]
        [Route("ChangePhoneNumber")]
        public async Task<IServiceResponse<bool>> ChangePhoneNumber(string userPhoneNumber)
        {
            return await HandleApiOperationAsync(async () => {
                var username = User.FindFirst(JwtClaimTypes.Name)?.Value;
                bool result = await _userSvc.ChangePhoneNumber(userPhoneNumber, username);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }

        //[AllowAnonymous]/*TO-BE-REMOVED*/
        //[HttpPost]
        //[Route("PostTimeGraph")]
        //public async Task<IServiceResponse<IDictionary<DateTime, List<PostDTO>>>> PostTimeGraph()
        //{
        //    return await HandleApiOperationAsync(async () => {
        //        var result = await _userSvc.PostTimeGraph();
        //        var response = new ServiceResponse<IDictionary<DateTime, List<PostDTO>>>(result);
        //        return response;
        //    });
        //}

        [HttpPost]
        [Route("ChangeProfileImage")]
        public async Task<IServiceResponse<bool>> ChangeProfileImage(ProfileImageDTO model)
        {
            return await HandleApiOperationAsync(async () => {
                bool result = await _userSvc.ChangeProfileImage(model.profileImageString,User.FindFirst(JwtClaimTypes.Name)?.Value);
                var response = new ServiceResponse<bool>(result);
                return response;
            });
        }
        #endregion
        //Authentication, Roles and Permissions....
        #region default from boilerplate
        //// GET: api/<AccountController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<AccountController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<AccountController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AccountController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AccountController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion
    }
}
//accts (users), profile