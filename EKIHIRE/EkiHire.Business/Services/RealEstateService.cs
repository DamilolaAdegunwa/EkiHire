using EkiHire.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Business.Services
{
    public interface IRealEstateService
    {
        public List<RealEstateCategory> GetRealEstateCategories();
    }
    public class RealEstateService : IRealEstateService
    {
        public List<RealEstateCategory> GetRealEstateCategories()
        {
            throw new NotImplementedException();
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