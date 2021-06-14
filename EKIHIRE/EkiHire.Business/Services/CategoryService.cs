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
using System.Reflection;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
//using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace EkiHire.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories(long[] catIds = null, bool withOtherData = false);//IEnumerable<Category>
        Task<IEnumerable<SubcategoryDTO>> GetSubcategoriesByCategoryId(long? CategoryId = null);
        Task<bool> SeedCategories();
        Task<bool> SeedSubcategories();
        Task<CategoryDTO> GetCategory(long Id, bool withOtherData = false);
        Task<bool> AddCategory(CategoryDTO model, string username);
        Task<bool> AddSubcategory(SubcategoryDTO model, string username);
        Task<List<List<Item>>> GetAllItemGroupsForSubcategory(long subId, string username);
        Task testemail();
        Task TestMail();
        Task TestMailGmail();
        Task TestMailYahoo();
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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<AdProperty> _adPropertyRepo;
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
            IRepository<Subcategory> subcategoryRepo,
            IRepository<Item> _itemRepository,
            IRepository<AdProperty> _adPropertyRepo

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
            this._itemRepository = _itemRepository;
            this._adPropertyRepo = _adPropertyRepo;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories(long[] catIds = null, bool withOtherData = false)//Task<IEnumerable<Category>>
        {
            try
            {
                var result =  _categoryRepo.GetAll().ToDTO().ToList();
                //var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                if(withOtherData)
                {
                    foreach (var r in result)
                    {
                        List<SubcategoryDTO> subcategories = _subcategoryRepo.GetAll().Where(s => s.CategoryId == r.Id).ToDTO().ToList();
                        subcategories.ForEach(x => {
                            var _adProperties = _adPropertyRepo.GetAll().Where(a => a.SubcategoryId == x.Id).ToDTO().ToList();
                            x.AdProperties = _adProperties;
                        });
                        r.Subcategories = subcategories;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw await _serviceHelper.GetExceptionAsync($"could not get categories :: {ex}");
            }
        }
        public async Task<CategoryDTO> GetCategory(long Id, bool withOtherData = false)
        {
            try
            {
                CategoryDTO result = _categoryRepo.Get(Id);
                if(withOtherData)
                {
                    List<SubcategoryDTO> subcategories = _subcategoryRepo.GetAll().Where(s => s.CategoryId == result.Id).ToDTO().ToList();
                    subcategories.ForEach(x => {
                        var _adProperties = _adPropertyRepo.GetAll().Where(a => a.SubcategoryId == x.Id).ToDTO().ToList();
                        x.AdProperties = _adProperties;
                    });
                    result.Subcategories = subcategories;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw await _serviceHelper.GetExceptionAsync($"could not get category :: {ex}");
            }
        }
        public async Task<IEnumerable<SubcategoryDTO>> GetSubcategoriesByCategoryId(long? CategoryId = null)
        {
            try
            {
                var result = _subcategoryRepo.GetAll().Where(x => x.Category.Id == CategoryId  || CategoryId == null).ToDTO().ToList();
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
        public async Task<bool> AddCategory(CategoryDTO model, string username)
        {
            try
            {
                #region validate the data given
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input valid data!");
                }
                var entity = _categoryRepo.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower());
                if (entity != null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Category already exist!!");
                }
                #endregion

                #region Set the Category
                Category data = new Category
                {
                    Name = model.Name,
                    ImagePath = model.ImagePath,
                    ImageString = model.ImageString,
                    //Subcategories = model.Subcategories,

                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = user.Id,
                    DeleterUserId = null,
                    DeletionTime = null,
                    Id = 0
                };
                _unitOfWork.BeginTransaction();
                await _categoryRepo.InsertAsync(data);
                _unitOfWork.Commit();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"A error occured while trying to set categories - error - {ex.Message} - stackTraack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }
        }
        public async Task<bool> AddSubcategory(SubcategoryDTO model, string username)
        {
            try
            {
                #region validate the data given
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                if (model == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input valid data!");
                }
                var entity = _subcategoryRepo.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower());
                if (entity != null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Subcategory already exist!!");
                }
                var cat = _categoryRepo.FirstOrDefault(c => c.Id == model.CategoryId);
                if (cat == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Category does not exist!!");
                }

                #endregion

                #region Set the Subcategory

                Subcategory data = new Subcategory
                {
                    Name = model.Name,
                    ImagePath = model.ImagePath,
                    ImageString = model.ImageString,
                    CategoryId = cat.Id,

                    //basic properties
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    IsDeleted = false,
                    LastModificationTime = DateTime.Now,
                    LastModifierUserId = user.Id,
                    DeleterUserId = null,
                    DeletionTime = null,
                    Id = 0
                };
                _unitOfWork.BeginTransaction();
                await _subcategoryRepo.InsertAsync(data);
                _unitOfWork.Commit();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"A error occured while trying to set categories - error - {ex.Message} - stackTrack - {ex.StackTrace} :: {MethodBase.GetCurrentMethod().Name}", ex);
                return false;
            }
        }
        public async Task<List<List<Item>>> GetAllItemGroupsForSubcategory(long subId, string username)
        {
            try
            {
                #region validate the data given
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw await _serviceHelper.GetExceptionAsync("Please input a username!");
                }
                var user = await _userSvc.FindFirstAsync(x => x.UserName == username);
                if (user == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Unauthorized access! Please login");
                }
                var entity = _subcategoryRepo.FirstOrDefault(x => x.Id == subId);
                if (entity == null)
                {
                    throw await _serviceHelper.GetExceptionAsync("Subcategory does not exist!!");
                }
                #endregion
                /*go to the item table, search out all the ones with subcategoryId = subId, grouped by the groupname*/
                //List<IGrouping<string, Item>> result = _itemRepository.GetAll().Where(a => a.Subcategory.Id == subId).ToList().GroupBy(g => g.GroupName).ToList();
                var list = _itemRepository.GetAll().Where(a => a.Subcategory.Id == subId).ToList();
                List<List<Item>> itemsbygroup = new List<List<Item>>();
                var uniqueGroups = list.Select(x => x.GroupName).Distinct();
                foreach (var g in uniqueGroups)
                {
                    var itemsInEachGroup = list.Where(x => x.GroupName == g).ToList();
                    itemsbygroup.Add(itemsInEachGroup);
                }
                return itemsbygroup;
            }
            catch (Exception ex)
            {
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}",ex);
                return null;
            }
        }

        public async Task<bool> SeedSubcategories()
        {
            try
            {
                Dictionary<Category, List<string>> catAndSub = new Dictionary<Category, List<string>>();
                #region (1) Real Estate
                var reCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Real Estate");
                var reSubData = new List<string>()
                {
                    "Houses & Apartments For Sale",
                    "Houses & Apartments For Rent",
                    "Commercial Property For Sale",
                    "Commercial Property For Rent",
                    "Land & Plots For Sale",
                    "Land & Plots For Rent",
                    "Short Lets"
                };
                catAndSub.Add(reCat,reSubData);
                #endregion

                #region (2) SERVICES
                var servicesCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Services");
                var servicesSubData = new List<string>
                {
                    "Agency",
                    "Automotives", //companyname, location
                    "Beauty & Personal Care",
                    "Building & Repair",
                    "Classes & Lessons", //companyname, servicefeature
                    "Cleaning", //companyname, serviceArea
                    "Computer & IT",
                    "Electronics",
                    "Fitness & Sports",
                    "Gardening & Landscaping", //condition,company,servicefeature,typeofservice
                    "Health",
                    "Logistics & Movers", //CompanyName
                    "Manufacturing",
                    "Music & Film",
                    "Party & Events",
                    "Pet Services", //gender, breed
                    "Photography & Videography",
                    "Printing",
                    "Professionals", //companyname, location
                    "Security & Surveillance",
                    "Skills & Talent",
                    "Other",
                    "Leisure" //companyname, location
                };
                catAndSub.Add(servicesCat, servicesSubData);
                #endregion

                #region (3) Jobs
                var JobsCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Jobs");
                var JobsSubData = new List<string> {
            "Account", "Administrative", "Advert & Marketing", "Architecture", "Beauty", "Consultnat", "Driver", "Engineering", "Farming", "HealthCare", "Hotel", "House Keeping", "Human Resouces", "IT & Software", "Internship", "Legal", "Logistics", "Management", "Manufacturing", "Nanny", "Oil & Gas", "Others...", "Part-Time", "Quality Control / Assurance", "Research", "Restaurant", "Retail", "Sales & Telemarketing", "Security", "Sports", "Teaching", "technology", "Travel & Tourism"
                };
                catAndSub.Add(JobsCat, JobsSubData);
                #endregion

                #region (4) Automobile
                var AutomobileCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Automobile");
                var AutomobileSubData = new List<string>{
                    "Cars",
                    "Trucks & Trailers",
                    "Buses & Micro Buses",
                    "Heavy Equipment",
                    "Vehicle Parts & Accessories",
                    "Motor Cycles & Scooters",
                    "Water Craft & Boats"
                };
                catAndSub.Add(AutomobileCat, AutomobileSubData);
                #endregion

                #region (5) Retails
                var RetailsCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Retails");
                var RetailsSubData = new List<string> {
                "Agriculture & farming",
                "Apparel",
                "Art, Paintings & crafts",
                "Audio, Books & Movies",
                "Baby & Toddler Essentials",
                "Beauty & Personal Care",
                "Cell Phones, Desktops, Laptops & Tablets",
                "Construction Materials & Supplies",
                "Consumer Electronics",
                "Education",
                "Electrical Materials & Supplies",
                "Energy & Power Supplies",
                "Electrical Materials & Supplies",
                "Event & Party Supplies",
                "Fitness & Sports",
                "Furniture",
                "Garden",
                "Gift Ideas & Presents",
                "Gift Cards & Coupons",
                "Groceries/FoodStuff",
                "Hair & Hair Care",
                "Home Decor & Bedding",
                "Household Appliances",
                "Household Supplies",
                "Industrial Tools & Hardware",
                "Luggage",
                "Machinery & Equipment",
                "Musical Instruments",
                "Office",
                "Packaging & Printing",
                "Pet & Pet Supplies",
                "Security & Surveillance",
                "Toys & Games",
                "Wholesale",
                "Other"
                };
                catAndSub.Add(RetailsCat, RetailsSubData);
                #endregion

                #region (6) Hotels
                var HotelsCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Hotels");
                var HotelsSubData = new List<string> { 
                
                };
                catAndSub.Add(HotelsCat, HotelsSubData);
                #endregion

                #region (7) Attractions
                var AttractionsCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Attractions");
                var AttractionsSubData = new List<string> {
                    "Cinema", "Bar & Lounges", "Parks"
                };
                catAndSub.Add(AttractionsCat, AttractionsSubData);
                #endregion

                #region (8) Restaurant
                var RestaurantCat = _categoryRepo.GetAll().FirstOrDefault(x => x.Name == "Restaurant");
                var RestaurantSubData = new List<string> {
                
                };
                catAndSub.Add(RestaurantCat, RestaurantSubData);
                #endregion

                await SaveEachSubcategories(catAndSub);
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> SaveEachSubcategories(Dictionary<Category, List<string>> catAndSub)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach(var kv in catAndSub)
                {
                    foreach (var name in kv.Value)//subcategories
                    {
                        var test = _subcategoryRepo.FirstOrDefault(x => true);
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(test);
                        var subData = _subcategoryRepo.FirstOrDefault(x => x.Category.Id == kv.Key.Id && kv.Key.Name == name);//category = kv.Key
                        if (subData != null)
                        {
                            continue;
                        }
                        var body = new Subcategory
                        {
                            Category = kv.Key,
                            Name = name,
                            ImagePath = null,
                            ImageString = null,
                            //basic properties
                            CreationTime = DateTime.Now,
                            CreatorUserId = null,
                            IsDeleted = false,
                            LastModificationTime = DateTime.Now,
                            LastModifierUserId = null,
                            DeleterUserId = null,
                            DeletionTime = null,
                            Id = 0

                        };
                        await _subcategoryRepo.InsertAsync(body);
                    }
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                log.Error($"{ex.Message} :: {MethodBase.GetCurrentMethod().Name} :: {ex.StackTrace}");
                return false;
            }
        }

        public async Task testemail()
        {
            //var replacement = new StringDictionary
            //{
            //    ["FirstName"] = "user.UserName",
            //    ["Otp"] = "user.OTP"
            //};

            //var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Password Reset OTP", "damee1993@gmail.com")
            //{
            //    BodyIsFile = true,
            //    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.PasswordResetEmail)
            //};

            //await _mailSvc.SendMailAsync(mail, replacement);

            //_ = "done";

            //string fromaddr = "damee1993@gmail.com";
            //string toaddr = "adegunwad@accessbankplc.com";//TO ADDRESS HERE
            //string password = "Damilola@123";

            //System.Net.Mail.MailMessage msg = new MailMessage();
            //msg.Subject = "Username &password";
            //msg.From = new MailAddress(fromaddr);
            //msg.Body = "Message BODY";
            //msg.To.Add(new MailAddress(toaddr));
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.UseDefaultCredentials = false;
            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential nc = new NetworkCredential(fromaddr, password);
            //smtp.Credentials = nc;
            //smtp.Send(msg);

            //MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            //MimeKit.MailboxAddress from = new MimeKit.MailboxAddress("Damilola Adegunwa",
            //"damee1993@gmailcom");
            //message.From.Add(from);

            //MimeKit.MailboxAddress to = new MimeKit.MailboxAddress("recipient",
            //"adegunwad@accessbankplc.com");
            //message.To.Add(to);

            //message.Subject = "This is email subject";

            //MimeKit.BodyBuilder bodyBuilder = new MimeKit.BodyBuilder();
            //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
            //bodyBuilder.TextBody = "Hello World!";

            //SmtpClient client = new SmtpClient();
            //client.Connect("smtp_address_here", 587, true);
            //client.Authenticate("user_name_here", "pwd_here");
            //_ = "";
            
        }
        public Task TestMail()//using outlook account
        {
            string to = "damee1993@gmail.com"; //To address    
            //string from = "damilola_093425@yahoo.com"; //From address    
            string from = "damilolar_moyo@outlook.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "(Just testing EkiHire Project) In this article you will learn how to send a email using Asp.Net & C#";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 25); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, "Damilola#123");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return default;
        }

        public Task TestMailGmail()//using gmail account
        {
            string to = "damilolar_moyo@outlook.com";//To address  
            string from = "damee1993@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "(Just testing EkiHire Project) In this article you will learn how to send a email using Asp.Net & C#";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 465); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, "Damilola@123");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return default;
        }

        public Task TestMailYahoo()//using ymail account
        {
            string to = "damilolar_moyo@outlook.com";//To address  
            string from = "damilola_093425@yahoo.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "(Just testing EkiHire Project) In this article you will learn how to send a email using Asp.Net & C#";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.mail.yahoo.com", 587); //ymail smtp 25, 465,587    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, "Damilola#123");
            client.EnableSsl =false;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return default;
        }
    }
}
