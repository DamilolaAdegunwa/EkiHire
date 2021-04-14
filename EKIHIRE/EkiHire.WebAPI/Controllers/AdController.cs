using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkiHire.Business.Services;
using EkiHire.Core.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace EkiHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : BaseController
    {

        public AdController()
        {

        }

        //public async Task<IServiceResponse<bool>> CloseAd()
        //{
        //    return await HandleApiOperationAsync(async () => {
        //        //var result = await 
        //        return default;
        //    });
        //}
    }
}
