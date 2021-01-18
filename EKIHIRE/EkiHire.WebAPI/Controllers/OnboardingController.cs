using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Business.Services;

namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : BaseController
    {
        private readonly IOnboardingService _onboardingService;
        public OnboardingController(
            IOnboardingService onboardingService
            )
        {
            _onboardingService = onboardingService;
        }

        public async Task<IServiceResponse<IEnumerable<Language>>> GetLanguages()
        {
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<IEnumerable<Language>>();
                
                return response;
            });
        }

        public async Task<IServiceResponse<IEnumerable<CategoryDTO>>> GetCategories()
        {//
            return await HandleApiOperationAsync(async () => {

                var response = new ServiceResponse<IEnumerable<CategoryDTO>>();
                var categories = _onboardingService.GetCategories();
                response.Object = categories;
                return response;
            });
        }
    }
}
//this is to cater for api needs as regards the splash screen and on-boarding 7 - 11