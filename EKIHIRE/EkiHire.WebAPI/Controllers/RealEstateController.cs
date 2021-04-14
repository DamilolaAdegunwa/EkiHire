using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateController : BaseController
    {
        private readonly IRealEstateService _realEstateService;
        public RealEstateController(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        [Route("GetRealEstateAds/{status}")]
        public async Task<IServiceResponse<IEnumerable<RealEstateDTO>>> GetRealEstateAds(int? status)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _realEstateService.GetRealEstateAds(status);
                return new ServiceResponse<IEnumerable<RealEstateDTO>>(result);
            });
        }
    }
}
