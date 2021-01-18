using EkiHire.Core.Domain.Entities;
using EkiHire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Business.Services
{
    public interface IAttractionService
    {
    }
    public class AttractionService: IAttractionService
    {
        readonly IRepository<Attraction> _repo;
        private readonly IServiceHelper _serviceHelper;
        public AttractionService(
            IRepository<Attraction> repo,
            IServiceHelper serviceHelper
            )
        {
            _repo = repo;
            _serviceHelper = serviceHelper;
        }
    }
}
/*
 May 2021 - Attractions & Posting of Ads Sections

1st Week - 4th Week :
- Attractions
- Explore
- Empty State
- Review
- Subscription (Payment)
- Profile (Edit, Change Password, etc)
- Market
- Sign up & Sign in (Social Media sign up & sign in)
 */