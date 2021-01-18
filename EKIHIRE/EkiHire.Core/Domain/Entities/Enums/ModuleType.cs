using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Core.Domain.Entities.Enums
{
    public enum ModuleType
    {
        Job = 0, //research employment opportunities (for employee|employer)
        Retail = 1, //marketplace
        Service = 2, 
        Automobile = 3, 
        RealEstate = 4, 
        Restaurant = 5,
        Recreational = 6, 
        OnlineForum = 7,
        Hotel = 8,
        AdsPost = 9,
        Attraction = 10
    }
}
//January 2021 - Real Estate and Services
//February 2021 - Retail and Jobs
//March 2021 - Automobiles
//April 2021 - Hotels & Restaurants
//May 2021 - Attractions & Posting of Ads Sections