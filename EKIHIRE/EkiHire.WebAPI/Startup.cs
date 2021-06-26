using EkiHire.Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EkiHire.Core.Caching;
using EkiHire.Core.Configuration;
using EkiHire.Core.Domain.Entities;
using EkiHire.Core.Domain.Extensions;
using EkiHire.Core.Utils;
using EkiHire.Data.efCore;
using EkiHire.Data.efCore.Context;
using EkiHire.Data.UnitOfWork;
using EkiHire.Core.Messaging.Email;
using EkiHire.Core.Messaging.Sms;
using EkiHire.WebAPI.Infrastructure.Services;
using EkiHire.WebAPI.Models;
using EkiHire.WebAPI.Utils;
using EkiHire.WebAPI.Utils.Extentions;
using EkiHire.Core.Collections.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using EkiHire.Data.Repository;
using log4net;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Specialized;

namespace EkiHire.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        //public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration/*, IWebHostEnvironment environment*/)
        {
            Configuration = configuration;
            //Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            #region Identity

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(WebConstants.ConnectionStringName))
                , ServiceLifetime.Transient);

            services.AddIdentity<User, Role>(options => {
                //password options
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                //other options
                options.Lockout.AllowedForNewUsers = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region Services
            services.AddTransient<DbContext>((_) => {
                var connStr = Configuration.GetConnectionString(WebConstants.ConnectionStringName);
                return new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                                         .UseSqlServer(connStr)
                                         .Options);
            });

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            services.RegisterGenericRepos(typeof(ApplicationDbContext));

            services.AddScoped<IErrorCodeService, ErrorCodeService>();
            services.AddScoped<IRepository<ErrorCode>, EfCoreRepository<DbContext, ErrorCode>>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRepository<User>, EfCoreRepository<DbContext, User>>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRepository<Account>, EfCoreRepository<DbContext, Account>>();

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IRepository<Wallet>, EfCoreRepository<DbContext, Wallet>>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRepository<Category>, EfCoreRepository<DbContext, Category>>();
            services.AddScoped<IRepository<Subcategory>, EfCoreRepository<DbContext, Subcategory>>();
            services.AddScoped<IRepository<AdImage>, EfCoreRepository<DbContext, AdImage>>();

            services.AddScoped<IAdService, AdService>();
            services.AddScoped<IRepository<Ad>, EfCoreRepository<DbContext, Ad>>();
            services.AddScoped<IRepository<Item>, EfCoreRepository<DbContext, Item>>();
            services.AddScoped<IRepository<CartItem>, EfCoreRepository<DbContext, CartItem>>();
            services.AddScoped<IRepository<AdFeedback>, EfCoreRepository<DbContext, AdFeedback>>();
            services.AddScoped<IRepository<Follow>, EfCoreRepository<DbContext, Follow>>();
            services.AddScoped<IRepository<Keyword>, EfCoreRepository<DbContext, Keyword>>();
            services.AddScoped<IRepository<AdProperty>, EfCoreRepository<DbContext, AdProperty>>();
            services.AddScoped<IRepository<AdPropertyValue>, EfCoreRepository<DbContext, AdPropertyValue>>();
            services.AddScoped<IRepository<AdLookupLog>, EfCoreRepository<DbContext, AdLookupLog>>();
            services.AddScoped<IRepository<RequestQuote>, EfCoreRepository<DbContext, RequestQuote>>();
            services.AddScoped<IRepository<JobApplication>, EfCoreRepository<DbContext, JobApplication>>();

            //services.AddScoped<IWalletService, WalletService>();
            //services.AddScoped<IOnboardingService, OnboardingService>();
            //services.AddScoped<IAccountService,AccountService>();
            //services.AddScoped<IAttractionService, AttractionService>();
            //services.AddScoped<IJobService, JobService>();
            //services.AddScoped<IModuleService, ModuleService>();
            //services.AddScoped<IRealEstateService, RealEstateService>();
            //services.AddScoped<IRetailService, RetailService>();
            //services.AddScoped<IWalletNumberService, WalletNumberService>();
            //services.AddScoped<ICouponService, CouponService>();
            //services.AddScoped<ICustomerService, CustomerService>();
            //services.AddScoped<IReferralService, ReferralService>();
            //services.AddScoped<IRouteService, RouteService>();
            //services.AddScoped<IRegionService, RegionService>();
            //services.AddScoped<IStateService, StateService>();
            //services.AddScoped<ITerminalService, TerminalService>();
            //services.AddScoped<IEmployeeService, EmployeeService>();
            //services.AddScoped<IVehicleModelService, VehicleModelService>();
            //services.AddScoped<IVehicleMakeService, VehicleMakeService>();
            //services.AddScoped<IBookingService, BookingService>();
            //services.AddScoped<IManifestService, ManifestService>();
            //services.AddScoped<ISeatManagementService, SeatManagementService>();
            //services.AddScoped<ITripService, TripService>();
            //services.AddScoped<IDiscountService, DiscountService>();
            //services.AddScoped<IDriverService, DriverService>();
            //services.AddScoped<ICouponService, CouponService>();
            //services.AddScoped<IMtuReports, MtuReportService>();
            //services.AddScoped<ITripAvailabilityService, TripAvailabilityService>();
            //services.AddScoped<IPickupPointService, PickupPointService>();
            //services.AddScoped<IAccountTransactionService, AccountTransactionService>();
            //services.AddScoped<IFareService, FareService>();
            //services.AddScoped<IFareCalendarService, FareCalendarService>();
            //services.AddScoped<IVehicleService, VehicleService>();
            //services.AddScoped<IVehicleTripRegistrationService, VehicleTripRegistrationService>();
            //services.AddScoped<IAccountSummaryService, AccountSummaryService>();
            //services.AddScoped<IHireRequestService, HireRequestService>();
            //services.AddScoped<IBookingReportService, BookingReportService>();
            //services.AddScoped<IFeedbackService, FeedbackService>();
            //services.AddScoped<ISubRouteService, SubRouteService>();
            //services.AddScoped<IJourneyManagementService, JourneyManagementService>();
            //services.AddScoped<IManifestService, ManifestService>();
            //services.AddScoped<IFranchizeService, FranchizeService>();
            //services.AddScoped<IPassportTypeService, PassportTypeService>();
            #endregion

            services.Configure<JwtConfig>(options =>
                        Configuration.GetSection(WebConstants.Sections.AuthJwtBearer).Bind(options));

            //services.Configure<BookingConfig>(options =>
            //           Configuration.GetSection(WebConstants.Sections.Booking).Bind(options));

            services.Configure<AppConfig>(options =>
                     Configuration.GetSection(WebConstants.Sections.App).Bind(options));

            services.Configure<SmtpConfig>(options =>
                     Configuration.GetSection(WebConstants.Sections.Smtp).Bind(options));

            services.Configure<PaymentConfig.Paystack>(options =>
                     Configuration.GetSection(WebConstants.Sections.Paystack).Bind(options));

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                     o.TokenLifespan = TimeSpan.FromHours(3));

            #region Auth
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                var jwtConfig = new JwtConfig();

                Configuration.Bind(WebConstants.Sections.AuthJwtBearer, jwtConfig);

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(3),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.SecurityKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //#endregion

            //#region Auth
            services.AddAuthorization(options => {
                SetupPolicies(options);
            });
            services.AddCors();
            services.AddDistributedMemoryCache();

            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    });
            //services.AddAuthentication()
            //    .AddFacebook(options =>
            //    {
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    });
            #endregion

            services.AddHttpContextAccessor();
            services.AddTransient<IServiceHelper, ServiceHelper>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IMailService, SmtpEmailService>();
            services.AddTransient<ISMSService, SMSService>();
            services.AddTransient<IWebClient, WebClient>();
            services.AddSingleton<IGuidGenerator>((s) => SequentialGuidGenerator.Instance);

            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EkiHire.WebAPI", Version = "v1" });
                c.DocInclusionPredicate((docName, description) => true);

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Description = "Token Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header, //"header",
                    Type = SecuritySchemeType.ApiKey,// "apiKey",
                });



                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                   {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
                });
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        private static void SetupPolicies(Microsoft.AspNetCore.Authorization.AuthorizationOptions options)
        {
            options.AddPolicy("Manage Customer", policy =>
                 policy.RequireClaim("Permission", PermissionClaimsProvider.ManageCustomer.Value));

            options.AddPolicy("Manage Employee", policy =>
                 policy.RequireClaim("Permission", PermissionClaimsProvider.ManageEmployee.Value));

            options.AddPolicy("Manage Report", policy =>
                policy.RequireClaim("Permission", PermissionClaimsProvider.ManageReport.Value));

            options.AddPolicy("Manage State", policy =>
                policy.RequireClaim("Permission", PermissionClaimsProvider.ManageState.Value));

            options.AddPolicy("Manage Region", policy =>
                policy.RequireClaim("Permission", PermissionClaimsProvider.ManageRegion.Value));

            options.AddPolicy("Manage HireBooking", policy =>
                policy.RequireClaim("Permission", PermissionClaimsProvider.ManageHireBooking.Value));

            options.AddPolicy("Manage Vehicle", policy =>
                policy.RequireClaim("Permission", PermissionClaimsProvider.ManageVehicle.Value));

            options.AddPolicy("Manage Terminal", policy =>
              policy.RequireClaim("Permission", PermissionClaimsProvider.ManageTerminal.Value));

            options.AddPolicy("Manage Route", policy =>
              policy.RequireClaim("Permission", PermissionClaimsProvider.ManageRoute.Value));

            options.AddPolicy("Manage Trip", policy =>
              policy.RequireClaim("Permission", PermissionClaimsProvider.ManageTrip.Value));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICategoryService categoryService, ILoggerFactory loggerFactory, IAdService adService, IAccountService accountService, IMailService _mailSvc/*, IRepository<Search> s, IUnitOfWork _unitOfWork*/)
        {
            //var replacement = new StringDictionary
            //{
            //    ["FirstName"] = "user.FirstName",
            //    ["ActivationCode"] = "user.AccountConfirmationCode"
            //};

            //var mail = new Mail("damilolar_moyo@outlook.com", "EkiHire.com: Account Verification Code", "damee1993@gmail.com")
            //{
            //    BodyIsFile = true,
            //    BodyPath = Path.Combine(env.ContentRootPath, CoreConstants.Url.ActivationCodeEmail),
            //    SenderDisplayName = "I'm Testing Please!",

            //};

            //_mailSvc.SendMailAsync(mail, replacement);



            //#region send emnail
            ////first file
            //if (File.Exists(Path.Combine(env.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)))
            //{
            //    var fileString = File.ReadAllText(Path.Combine(env.ContentRootPath, CoreConstants.Url.ActivationCodeEmail));
            //    if (!string.IsNullOrWhiteSpace(fileString))
            //    {
            //        fileString = fileString.Replace("{{FirstName}}", "test name");
            //        fileString = fileString.Replace("{{ActivationCode}}", "QWERTY_12345");

            //        _mailSvc.SendMailAsync("adegunwad@accessbankplc.com", "EkiHire.com: Account Verification Code", fileString);
            //    }
            //}
            //#endregion
            //var s =  adService.Search(new Core.Model.SearchVM(), "", true).GetAwaiter().GetResult();
            //categoryService.TestMail();
            //categoryService.TestMailGmail();
            //categoryService.TestMailYahoo();
            //accountService.TestEHMail();
            loggerFactory.AddLog4Net();
            #region seeding the db
            //categoryService.SeedCategories();
            //categoryService.SeedSubcategories();//108
            #endregion end seeding the db
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EkiHire.WebAPI v1"));
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}
            app.UseDeveloperExceptionPage();
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EkiHire.WebAPI v1"));

            app.UseCors(x => {
                //x.WithOrigins(Configuration["App:CorsOrigins"]
                //  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                //  .Select(o => o.RemovePostFix("/"))
                //  .ToArray());
                x.AllowAnyOrigin() .AllowAnyMethod() .AllowAnyHeader();
            });

            app.UseAuthentication();
            //app.UseStatusCodePages();
            //app.UseMvc();

            //app.UseSwagger();
            //app.UseSwaggerUI(options => {
            //    options.SwaggerEndpoint(Configuration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "EkiHire.Web API version 1");
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
//1701;1702
//;NU1605