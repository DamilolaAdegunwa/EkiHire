//using EkiHire.Core.Configuration;
//using EkiHire.Core.Domain.Entities;
//using EkiHire.Core.Domain.Entities.Enums;
//using EkiHire.Core.Exceptions;
//using EkiHire.Core.Messaging.Email;
//using EkiHire.Core.Messaging.Sms;
//using EkiHire.Core.Model;
//using EkiHire.Core.Utils;
//using EkiHire.Data.Repository;
//using EkiHire.Data.UnitOfWork;
//using IPagedList;
//using log4net;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//namespace EkiHire.Business.Services
//{
//    public interface IAccountService
//    {
//        Task SignUp(LoginViewModel model);

//    }

//    public class AccountService : IAccountService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IServiceHelper _serviceHelper;
//        //private readonly IRepository<Account> _repo;
//        private readonly IRepository<Wallet> _walletRepo;
//        private readonly IUserService _userSvc;
//        //private readonly IRoleService _roleSvc;
//        private readonly ISMSService _smsSvc;
//        //private readonly IMailService _mailSvc;
//        private readonly IHostingEnvironment _hostingEnvironment;
//        private readonly IGuidGenerator _guidGenerator;
//        private readonly AppConfig appConfig;
//        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
//        readonly SmtpConfig _smtpsettings;
//        private readonly IRepository<User> _userRepo;
//        private readonly IChatHub _chatHub;
//        public AccountService(IUnitOfWork unitOfWork,
//            //IRepository<Account> employeeRepo,
//            IRepository<Wallet> walletRepo,
//            IServiceHelper serviceHelper,
//            IUserService userSvc,
//            //IRoleService roleSvc,
//            ISMSService smsSvc,
//            //IMailService mailSvc,
//            IHostingEnvironment hostingEnvironment,
//            IGuidGenerator guidGenerator,
//            IOptions<AppConfig> _appConfig
//            , IOptions<SmtpConfig> settingSvc
//            , IRepository<User> _userRepo
//            , IChatHub chatHub
//            )
//        {
//            _unitOfWork = unitOfWork;
//           // _repo = employeeRepo;
//            _walletRepo = walletRepo;
//            _serviceHelper = serviceHelper;
//            _userSvc = userSvc;
//            _smsSvc = smsSvc;
//            //_mailSvc = mailSvc;
//            appConfig = _appConfig.Value;
//            _hostingEnvironment = hostingEnvironment;
//            _guidGenerator = guidGenerator;
//            //_roleSvc = roleSvc;
//            _smtpsettings = settingSvc.Value;
//            this._userRepo = _userRepo;
//            _chatHub = chatHub;
//        }
//        //public async Task<bool> AccountExist(string username)
//        //{
//        //    return await _repo.ExistAsync(x => x.Email == username);
//        //}
        
//        //public async Task AddAccount(AccountDTO account)//Add profile
//        //{
//        //    //if (account == null)
//        //    //{
//        //    //    throw await _serviceHelper.GetExceptionAsync("invalid parameter");
//        //    //}
//        //    //if (account.TerminalId != null && !await IsValidTerminal(account.TerminalId))
//        //    //{
//        //    //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.TERMINAL_NOT_EXIST);
//        //    //}
//        //    //if (account.DepartmentId != null && !await IsValidDepartment(account.DepartmentId))
//        //    //{
//        //    //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.DEPARTMENT_NOT_EXIST);
//        //    //}
//        //    //account.AccountCode = account.AccountCode.Trim();
//        //    //if (await _repo.ExistAsync(v => v.AccountCode == account.AccountCode))
//        //    //{
//        //    //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.EMPLOYEE_EXIST);
//        //    //}
//        //    //try
//        //    //{
//        //    //    _unitOfWork.BeginTransaction();

//        //    //    var user = new User
//        //    //    {
//        //    //        FirstName = account.FirstName,
//        //    //        LastName = account.LastName,
//        //    //        MiddleName = account.MiddleName,
//        //    //        Gender = account.Gender,
//        //    //        Email = account.Email,
//        //    //        PhoneNumber = account.PhoneNumber,
//        //    //        Address = account.Address,
//        //    //        NextOfKinName = account.NextOfKin,
//        //    //        NextOfKinPhone = account.NextOfKinPhone,
//        //    //        EmailConfirmed = true,
//        //    //        PhoneNumberConfirmed = true,
//        //    //        UserName = account.Email,
//        //    //        ReferralCode = CommonHelper.GenerateRandonAlphaNumeric()
//        //    //    };

//        //    //    var creationStatus = await _userSvc.CreateAsync(user, account.Password);

//        //    //    if (creationStatus.Succeeded)
//        //    //    {

//        //    //        var dbRole = await _roleSvc.FindByIdAsync(account.RoleId);

//        //    //        if (dbRole != null)
//        //    //        {
//        //    //            await _userSvc.AddToRoleAsync(user, dbRole.Name);
//        //    //        }

//        //    //        //_repo.Insert(new Account
//        //    //        //{
//        //    //        //    UserId = user.Id,
                        
//        //    //        //    CreatorUserId = _serviceHelper.GetCurrentUserId()
//        //    //        //});


//        //    //        await SendAccountEmail(user, "");

//        //    //    }
//        //    //    else
//        //    //    {
//        //    //        _unitOfWork.Rollback();

//        //    //        throw await _serviceHelper
//        //    //            .GetExceptionAsync(creationStatus.Errors.FirstOrDefault()?.Description);
//        //    //    }
//        //    //    _unitOfWork.Commit();
//        //    //}
//        //    //catch (Exception)
//        //    //{

//        //    //    _unitOfWork.Rollback();
//        //    //    throw;
//        //    //}
//        //}
//        //public async Task SendAccountCredentials(User user, string password)
//        //{
//        //    try
//        //    {

//        //        var replacement = new StringDictionary
//        //        {
//        //            ["FirstName"] = user.FirstName,
//        //            ["UserName"] = user.UserName,
//        //            ["DefaultPassword"] = password
//        //        };

//        //        var mail = new Mail(appConfig.AppEmail, "EkiHire.com: New User Account Information", user.Email)
//        //        {
//        //            BodyIsFile = true,
//        //            BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.AccountActivationEmail)
//        //        };

//        //        await _mailSvc.SendMailAsync(mail, replacement);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        log.Error($"could not send credentials for user : {user.UserName}");
//        //    }
//        //}

        
//        #region account
//        //public Task<Account> GetAccountsByemailAsync(string email)
//        //{
//        //    var employees =
//        //        from employee in _repo.GetAllIncluding(x => x.User)

//        //        //join department in _departmentRepo.GetAll() on employee.DepartmentId equals department.Id
//        //        //into departments
//        //        //from department in departments.DefaultIfEmpty()

//        //        //join wallet in _walletRepo.GetAll() on employee.User.WalletId equals wallet.Id
//        //        //into wallets
//        //        //from wallet in wallets.DefaultIfEmpty()

//        //        //join terminal in _terminalRepo.GetAll() on employee.TerminalId equals terminal.Id
//        //        //into terminals
//        //        //from terminal in terminals.DefaultIfEmpty()

//        //        where employee.User.Email == email

//        //        select new AccountDTO
//        //        {
//        //            Id = employee.Id,
//        //            FirstName = employee.User.FirstName,
//        //            LastName = employee.User.LastName,
//        //            MiddleName = employee.User.MiddleName,
//        //            Gender = employee.User.Gender,
//        //            Email = employee.User.Email,
//        //            PhoneNumber = employee.User.PhoneNumber,
//        //            Address = employee.User.Address,
//        //            //AccountPhoto = employee.User.Photo,
//        //            NextOfKinName = employee.User.NextOfKinName,
//        //            NextOfKinPhone = employee.User.NextOfKinPhone,
//        //            //DepartmentName = department.Name,
//        //            //DepartmentId = department.Id,
//        //            //TerminalId = terminal.Id,
//        //            //TerminalName = terminal.Name,
//        //            OTP = employee.Otp,
//        //            //OtpIsUsed = employee.OtpIsUsed,
//        //            //AccountCode = employee.AccountCode,
//        //        };

//        //    //return employees.AsNoTracking().FirstOrDefaultAsync();
//        //    return employees.FirstOrDefaultAsync();
//        //}

//        //public Task<IPagedList<User>> GetAccounts(int pageNumber, int pageSize, string query)
//        //{
//        //    var employees =
//        //        from employee in _repo.GetAllIncluding(x => x.User)

//        //        //join department in _departmentRepo.GetAll() on employee.DepartmentId equals department.Id
//        //        //into departments
//        //        //from department in departments.DefaultIfEmpty()

//        //        //join terminal in _terminalRepo.GetAll() on employee.TerminalId equals terminal.Id
//        //        //into terminals
//        //        //from terminal in terminals.DefaultIfEmpty()

//        //        let userRole = employee.User == null ? Enumerable.Empty<string>() : _userSvc.GetUserRoles(employee.User).Result

//        //        where string.IsNullOrWhiteSpace(query) ||
//        //        (employee.User.FirstName.Contains(query) ||
//        //            employee.User.LastName.Contains(query) ||
//        //            employee.User.Email.Contains(query) ||
//        //            employee.User.PhoneNumber.Contains(query))
//        //        select new User
//        //        {
//        //            Id = employee.Id,
//        //            FirstName = employee.User.FirstName,
//        //            LastName = employee.User.LastName,
//        //            MiddleName = employee.User.MiddleName,
//        //            Gender = employee.User.Gender,
//        //            Email = employee.User.Email,
//        //            PhoneNumber = employee.User.PhoneNumber,
//        //            Address = employee.User.Address,
//        //            //AccountPhoto = employee.User.Photo,
//        //            NextOfKinName = employee.User.NextOfKinName,
//        //            NextOfKinPhone = employee.User.NextOfKinPhone,
//        //            //DepartmentName = department.Name,
//        //            //DepartmentId = department.Id,
//        //            //TerminalId = terminal.Id,
//        //            //TerminalName = terminal.Name,
//        //            //AccountCode = employee.AccountCode,
//        //            //RoleName = string.Join(",", userRole)
//        //        };

//        //    //return employees.AsNoTracking().ToPagedListAsync(pageNumber, pageSize);
//        //    return employees.ToPagedListAsync(pageNumber, pageSize);
//        //}

//        //public async Task<Account> GetAccount(int id)
//        //{
//        //    var employee = _repo.GetAllIncluding(x => x.User).FirstOrDefault(x => x.Id == id);

//        //    var employeeDto = new Account
//        //    {
//        //        Id = employee.Id,
//        //        FirstName = employee.User.FirstName,
//        //        LastName = employee.User.LastName,
//        //        MiddleName = employee.User.MiddleName,
//        //        Gender = employee.User.Gender,
//        //        Email = employee.User.Email,
//        //        PhoneNumber = employee.User.PhoneNumber,
//        //        Address = employee.User.Address,
//        //        //AccountPhoto = employee.User.Photo,
//        //        NextOfKinName = employee.User.NextOfKinName,
//        //        NextOfKinPhone = employee.User.NextOfKinPhone,
//        //        //DepartmentId = employee.DepartmentId,
//        //        //TerminalId = employee.TerminalId,
//        //        //AccountCode = employee.AccountCode,
//        //    };

//        //    var userRole = _userSvc.GetUserRoles(employee.User).Result.FirstOrDefault();

//        //    if (!string.IsNullOrEmpty(userRole))
//        //    {
//        //        var role = await _roleSvc.FindByName(userRole);
//        //        employeeDto.RoleId = role.Id;
//        //    }

//        //    return employeeDto;
//        //}

//        //public async Task UpdateAccount(int id, Account model)
//        //{
//        //    var employee = _repo.Get(id);

//        //    var user = await _userSvc.FindFirstAsync(x => x.Id == employee.User.Id);

//        //    if (user is null)
//        //    {
//        //        throw new EntityNotFoundException(typeof(User), id);
//        //    }

//        //    //employee.AccountCode = model.AccountCode;
//        //    //employee.DepartmentId = model.DepartmentId;
//        //    //employee.TerminalId = model.TerminalId;

//        //    user.FirstName = model.FirstName;
//        //    user.LastName = model.LastName;
//        //    user.MiddleName = model.MiddleName;
//        //    user.Gender = model.Gender;
//        //    user.Email = model.Email;
//        //    user.PhoneNumber = model.PhoneNumber;
//        //    user.Address = model.Address;
//        //    user.NextOfKinName = model.NextOfKinName;
//        //    user.NextOfKinPhone = model.NextOfKinPhone;


//        //    if (model.RoleId != 0)
//        //    {
//        //        var newRole = await _roleSvc.FindByIdAsync(model.RoleId);
//        //        if (newRole != null && !await _userSvc.IsInRoleAsync(user, newRole.Name))
//        //        {
//        //            await _userSvc.RemoveFromRolesAsync(user, await _userSvc.GetUserRoles(user));
//        //            await _userSvc.AddToRoleAsync(user, newRole.Name);
//        //        }
//        //    }

//        //    await _userSvc.UpdateAsync(user);
//        //    await _unitOfWork.SaveChangesAsync();
//        //}
//        #endregion
//        //public async Task TestEHMail()
//        //{
//        //    try
//        //    {
//        //        var replacement = new StringDictionary
//        //        {
//        //            ["FirstName"] = "user.FirstName",
//        //            ["ActivationCode"] = "user.AccountConfirmationCode"
//        //        };

//        //        var mail = new Mail(_smtpsettings.UserName, "EkiHire.com: Account Verification Code", "damee1993@gmail.com")
//        //        {
//        //            BodyIsFile = true,
//        //            BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail),
//        //            SenderDisplayName = _smtpsettings.SenderDisplayName
//        //        };

//        //        await _mailSvc.SendMailAsync(mail, replacement);
//        //        _ = default(string);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //}
//    }
//}