using System;
using System.Collections.Generic;
using System.Text;
using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Exceptions;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Messaging.Sms;
using EkiHire.Core.Model;
using EkiHire.Core.Utils;
using EkiHire.Data.Repository;
using EkiHire.Data.UnitOfWork;
using IPagedList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using EkiHire.Core.Domain.Entities.Enums;
using EkiHire.Core.Domain.Extensions;
using Microsoft.Extensions.Logging;
using log4net;

namespace EkiHire.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<IEnumerable<SubcategoryDTO>> GetSubcategoriesByCategoryId(long CategoryId);
        Task<bool> SeedCategories();
        Task<bool> SeedRealEstateSubcategories();
    }
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceHelper _serviceHelper;
        private readonly IRepository<Account> _repo;
        //private readonly IRepository<Terminal> _terminalRepo;
        private readonly IRepository<Wallet> _walletRepo;
        //private readonly IRepository<Department> _departmentRepo;
        private readonly IUserService _userSvc;
        private readonly IRoleService _roleSvc;
        //private readonly IReferralService _referralSvc;
        private readonly ISMSService _smsSvc;
        private readonly IMailService _mailSvc;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGuidGenerator _guidGenerator;
        private readonly AppConfig appConfig;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Subcategory> _subcategoryRepo;
        private readonly ILog _logger;
        public CategoryService(IUnitOfWork unitOfWork,
            IRepository<Account> employeeRepo,
            //IRepository<Terminal> terminalRepo,
            IRepository<Wallet> walletRepo,
            //IRepository<Department> departmentRepo,
            IServiceHelper serviceHelper,
            IUserService userSvc,
            IRoleService roleSvc,
            //IReferralService referralSvc,
            ISMSService smsSvc,
            IMailService mailSvc,
            IHostingEnvironment hostingEnvironment,
            IGuidGenerator guidGenerator,
            IOptions<AppConfig> _appConfig,
            IRepository<Category> categoryRepo,
            IRepository<Subcategory> subcategoryRepo
            
            )
        {
            _unitOfWork = unitOfWork;
            _repo = employeeRepo;
            //_terminalRepo = terminalRepo;
            _walletRepo = walletRepo;
            //_departmentRepo = departmentRepo;
            _serviceHelper = serviceHelper;
            _userSvc = userSvc;
            //_referralSvc = referralSvc;
            _smsSvc = smsSvc;
            _mailSvc = mailSvc;
            appConfig = _appConfig.Value;
            _hostingEnvironment = hostingEnvironment;
            _guidGenerator = guidGenerator;
            _roleSvc = roleSvc;
            _categoryRepo = categoryRepo;
            _subcategoryRepo = subcategoryRepo;

            _logger = LogManager.GetLogger(typeof(CategoryService));
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            try
            {
                var result =  _categoryRepo.GetAll().AsEnumerable().ToDTO();
                return result;
            }
            catch (Exception ex)
            {

                throw await _serviceHelper.GetExceptionAsync($"could not get categories :: {ex}");
            }
        }
        public async Task<CategoryDTO> GetCategory(long Id)
        {
            try
            {
                CategoryDTO result = _categoryRepo.Get(Id);
                return result;
            }
            catch (Exception ex)
            {

                throw await _serviceHelper.GetExceptionAsync($"could not get category :: {ex}");
            }
        }
        public async Task<IEnumerable<SubcategoryDTO>> GetSubcategoriesByCategoryId(long CategoryId)
        {
            try
            {
                var result = _subcategoryRepo.GetAll().Where(x => x.Category.Id == CategoryId).ToList().ToDTO();
                return result;
            }
            catch (Exception ex)
            {

                throw await _serviceHelper.GetExceptionAsync($"could not get category :: {ex}");
            }
        }
        public async Task<bool> SeedCategories()
        {
            try
            {
                var categories = (await GetCategories()).ToList();

                if (categories != null && categories.Count > 0)
                {
                    return true;
                }
                var data = new List<string>{
                    "Real Estate", "Services", "Jobs", "Automobile", "Retails", "Hotels", "Attractions", "Restaurant"
                };
                //,"Phones & Tablets"
                _unitOfWork.BeginTransaction();
                foreach (var d in data)
                {
                    var cat = new Category
                    {
                        CreationTime = DateTime.Now,
                        CreatorUserId = null,
                        DeleterUserId = null,
                        DeletionTime = null,
                        IsDeleted = false,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = null,
                        Name = d,
                        
                    };
                    await _categoryRepo.InsertAsync(cat);
                    
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return false;
                //throw await _serviceHelper.GetExceptionAsync($"could not get categories :: {ex}");
            }
        }

        public async Task<bool> SeedRealEstateSubcategories()
        {
            try
            {
                var category = _categoryRepo.GetAll().Where(x => x.Name == "Real Estate")?.FirstOrDefault();
                var realEstateSub = _subcategoryRepo.GetAll().Where(x => x.Category.Id == category.Id).FirstOrDefault();
                if(realEstateSub != null)
                {
                    return true;
                }
                var realEstateSubcategories = new List<string>()
                {
                    "Houses & Apartments For Sale",
                    "Houses & Apartments For Rent",
                    "Commercial Property For Sale",
                    "Commercial Property For Rent",
                    "Land & Plots For Sale",
                    "Land & Plots For Rent",
                    "Short Lets"
                };
                _unitOfWork.BeginTransaction();
                foreach(var s in realEstateSubcategories)
                {
                    var body = new Subcategory
                    {
                        Category = category,
                        CreationTime = DateTime.Now,
                        CreatorUserId = null,
                        DeleterUserId = null,
                        DeletionTime = null,
                        IsDeleted = false,
                        LastModificationTime = DateTime.Now,
                        LastModifierUserId = null,
                        Name = s,
                        
                    };
                    await _subcategoryRepo.InsertAsync(body);
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error("An error occured while trying to seed real estate sub categories", ex);
                return false;
            }
        }
    }
}
