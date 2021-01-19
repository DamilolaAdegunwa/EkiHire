using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Domain.Extensions;
namespace EkiHire.Business.Services
{
    public interface IOnboardingService
    {
        public IEnumerable<CategoryDTO> GetCategories();
    }

    public class OnboardingService: IOnboardingService
    {
        readonly IRepository<Onboarding> _onboardingRepo;
        readonly IRepository<Category> _categoryRepo;
        private readonly IServiceHelper _serviceHelper;
        public OnboardingService(
            IRepository<Onboarding> onboardingRepo,
            IRepository<Category> categoryRepo,
            IServiceHelper serviceHelper
            )
        {
            _onboardingRepo = onboardingRepo;
            _categoryRepo = categoryRepo;
            _serviceHelper = serviceHelper;
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var categories = _categoryRepo.GetAll().ToDTO();
            return categories;
        }
    }
}
//Splash Screen, On - boarding