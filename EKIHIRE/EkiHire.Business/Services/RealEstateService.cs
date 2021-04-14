using EkiHire.Core.Domain.Entities;
using EkiHire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EkiHire.Core.Domain.Extensions;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities.Enums;

namespace EkiHire.Business.Services
{
    public interface IRealEstateService
    {
        public List<RealEstateCategory> GetRealEstateCategories();
        Task<IEnumerable<RealEstateDTO>> GetRealEstateAds(int? status);
    }
    public class RealEstateService : IRealEstateService
    {
        private readonly IServiceHelper _serviceHelper;
        private readonly IRepository<RealEstate> _RealEstateRepo;
        public RealEstateService(IServiceHelper serviceHelper, IRepository<RealEstate> RealEstateRepo)
        {
            _serviceHelper = serviceHelper;
            _RealEstateRepo = RealEstateRepo;
        }
        public List<RealEstateCategory> GetRealEstateCategories()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RealEstateDTO>> GetRealEstateAds(int? status)
        {
            try
            {
                var result = await _RealEstateRepo.GetAll().ToListAsync();

                if(status != null)
                {
                    result = result.Where(x => (int)x.RealEstateAdsStatus == (int)status).ToList();
                }
                
                return result.ToDTO();
            }
            catch (Exception ex)
            {
                throw await _serviceHelper.GetExceptionAsync("An error occured while trying to get real estate ads");
                //throw;
            }
        }
    }
}
/*
 *January 2021 - Real Estate and Services

1st Week - 2nd Week :
- Landing Page (Categories)
- Real Estate (All Categories, Houses & Apartments for Rent & Sale, Land & Plots for Rent and Sale, Shortlet)
3rd Week - 4th Week :
- Service Landing Page (Categories)
- All Services Categories
*/