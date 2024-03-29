﻿using EkiHire.Business.Services;
using EkiHire.Core.Domain.Entities;
using EkiHire.WebAPI.Infrastructure.Services;
using EkiHire.WebAPI.Utils;
using EkiHire.WebAPI.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using EkiHire.Core.Domain.Model;
namespace EkiHire.WebAPI.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IUserService _userSvc;
        //private readonly IRoleService _roleSvc;
        private readonly ITokenService _tokenSvc;

        public TokenController(
            IUserService usersvc,
            ITokenService tokenSvc
            //, IRoleService rolesvc
            )
        {
            _userSvc = usersvc;
            _tokenSvc = tokenSvc;
          //  _roleSvc = rolesvc;
        }

        [HttpPost]/*Sign In - checked*/
        public async Task<IServiceResponse<TokenDTO>> Index([FromBody] LoginModel model)
        {//worked locally and online
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<TokenDTO>();

                var user = await _userSvc.FindByNameAsync(model.Username)
                        ?? await _userSvc.FindByEmailAsync(model.Username);

                if (!user.IsNull() && await _userSvc.CheckPasswordAsync(user, model.Password))
                {

                    if (!user.IsDefaultAccount())
                    {

                        if (!user.IsConfirmed())
                        {

                            response.Code = HttpStatusCode.BadRequest.GetStatusCodeValue();
                            response.ShortDescription = "Account not active. Please activate your acccount to continue.";
                            return response;
                        }

                        if (user.AccountLocked())
                        {
                            response.Code = HttpStatusCode.BadRequest.GetStatusCodeValue();
                            response.ShortDescription = "Account locked. Please contact the system administrator.";
                            return response;
                        }

                    }

                    var userClaims = user.UserToClaims();
                    //userClaims.AddRange(await RoleClaims(user));

                    var token = _tokenSvc.GenerateAccessTokenFromClaims(userClaims.ToArray());

                    user.RefreshToken = token.RefreshToken;
                    await _userSvc.UpdateAsync(user);

                    response.Object = token;
                }

                else
                {
                    response.Code = HttpStatusCode.BadRequest.GetStatusCodeValue();
                    response.ShortDescription = "Invalid credentials supplied.";
                }

                return response;
            });
        }

        //private async Task<IEnumerable<Claim>> RoleClaims(User user)
        //{
        //    var userRoleClaims = new List<Claim>();
        //    var roles = await _userSvc.GetUserRoles(user);

        //    foreach (var item in roles) {

        //        userRoleClaims.Add(new Claim(JwtClaimTypes.Role, item));

        //        var roleClaims = await _roleSvc.GetRoleClaimsAsync(item);
        //        userRoleClaims.AddRange(roleClaims);
        //    }

        //    return userRoleClaims;
        //}

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IServiceResponse<TokenDTO>> Refresh(RefreshTokenModel model)
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<TokenDTO>();

                var principal = _tokenSvc.GetPrincipalFromExpiredToken(model.AccessToken);
                if (principal != null)
                {
                    var username = principal.FindFirst(JwtClaimTypes.Name).Value;

                    var user = await _userSvc.FindByNameAsync(username);

                    if (user is null || user.RefreshToken != model.RefreshToken)
                    {
                        response.Code = HttpStatusCode.BadRequest.GetStatusCodeValue();
                        response.ShortDescription = "Invalid token supplied.";
                        return response;
                    }

                    var userClaims = user.UserToClaims();
                    //userClaims.AddRange(await RoleClaims(user));

                    var token = _tokenSvc.GenerateAccessTokenFromClaims(userClaims.ToArray());

                    user.RefreshToken = token.RefreshToken;
                    await _userSvc.UpdateAsync(user);

                    response.Object = token;

                    return response;
                }

                response.Code = HttpStatusCode.BadRequest.GetStatusCodeValue();
                response.ShortDescription = "User is invalid.";
                return response;
            });
        }
    }
}