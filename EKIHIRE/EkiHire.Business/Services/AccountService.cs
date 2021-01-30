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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EkiHire.Business.Services
{
    public interface IAccountService
    {
        Account FirstOrDefault(Expression<Func<Account, bool>> filter);
        IQueryable<Account> GetAll();
        Task<AccountDTO> GetAccountsByemailAsync(string email);
        //Task AddAccount(AccountDTO employee);//might be used in the future
        Task AddAccount(AccountDTO employee);
        Task<IPagedList<AccountDTO>> GetAccounts(int pageNumber, int pageSize, string query);
        //Task<int?> GetAssignedTerminal(string email);
        //Task UpdateAccountOtp(int employeeId, string otp);
        Task<bool> Verifyotp(string otp);
        Task<AccountDTO> GetAccount(int id);
        Task UpdateAccount(int id, AccountDTO model);
        Task SignUp(LoginViewModel model);
    }

    public class AccountService : IAccountService
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

        public AccountService(IUnitOfWork unitOfWork,
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
            IOptions<AppConfig> _appConfig)
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
        }





        private async Task<bool> AccountExist(string username)
        {
            return await _repo.ExistAsync(x => x.Email == username);
        }
        public async Task SignUp(LoginViewModel model)
        {
            //validate credential
            if (model == null)
            {
                throw await _serviceHelper.GetExceptionAsync("Invalid parameter");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                throw await _serviceHelper.GetExceptionAsync("Please input a password");
            }
            var user = await _userSvc.FindFirstAsync(x => x.UserName == model.UserName);

            if (user!=null)
            {
                throw await _serviceHelper.GetExceptionAsync("User already exist");
            }

            //sign up a new user
            try
            {
                _unitOfWork.BeginTransaction();
                user = new User
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    AccountConfirmationCode = CommonHelper.GenereateRandonAlphaNumeric(),
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                };
                var creationStatus = await _userSvc.CreateAsync(user, model.Password);

                if (creationStatus.Succeeded)
                {
                    _repo.Insert(new Account
                    {
                        UserId = user.Id,
                    });

                    //await SendAccountEmail(user,model.Password);
                    if (user != null)
                    {
                        var replacement = new StringDictionary
                        {
                            //["FirstName"] = user.FirstName,
                            ["ActivationCode"] = user.AccountConfirmationCode
                        };

                        var mail = new Mail(appConfig.AppEmail, "EkiHire.com: Account Verification Code", user.Email)
                        {
                            BodyIsFile = true,
                            BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.ActivationCodeEmail)
                        };

                        await _mailSvc.SendMailAsync(mail, replacement);
                        
                    }
                }
                else
                {
                    _unitOfWork.Rollback();

                    throw await _serviceHelper.GetExceptionAsync(creationStatus.Errors.FirstOrDefault()?.Description);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw await _serviceHelper.GetExceptionAsync("an error occured!"); ;
            }
            //send email
        }
        public async Task AddAccount(AccountDTO account)
        {
            if (account == null)
            {
                throw await _serviceHelper.GetExceptionAsync("invalid parameter");
            }
            //if (account.TerminalId != null && !await IsValidTerminal(account.TerminalId))
            //{
            //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.TERMINAL_NOT_EXIST);
            //}
            //if (account.DepartmentId != null && !await IsValidDepartment(account.DepartmentId))
            //{
            //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.DEPARTMENT_NOT_EXIST);
            //}
            //account.AccountCode = account.AccountCode.Trim();
            //if (await _repo.ExistAsync(v => v.AccountCode == account.AccountCode))
            //{
            //    throw await _serviceHelper.GetExceptionAsync(ErrorConstants.EMPLOYEE_EXIST);
            //}
            try
            {
                _unitOfWork.BeginTransaction();

                var user = new User
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    MiddleName = account.MiddleName,
                    Gender = account.Gender,
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber,
                    Address = account.Address,
                    NextOfKinName = account.NextOfKin,
                    NextOfKinPhone = account.NextOfKinPhone,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    UserName = account.Email,
                    ReferralCode = CommonHelper.GenereateRandonAlphaNumeric()
                };

                var creationStatus = await _userSvc.CreateAsync(user, account.Password);

                if (creationStatus.Succeeded)
                {

                    var dbRole = await _roleSvc.FindByIdAsync(account.RoleId);

                    if (dbRole != null)
                    {
                        await _userSvc.AddToRoleAsync(user, dbRole.Name);
                    }

                    _repo.Insert(new Account
                    {
                        UserId = user.Id,
                        
                        CreatorUserId = _serviceHelper.GetCurrentUserId()
                    });


                    await SendAccountEmail(user, "");

                }
                else
                {
                    _unitOfWork.Rollback();

                    throw await _serviceHelper
                        .GetExceptionAsync(creationStatus.Errors.FirstOrDefault()?.Description);
                }
                _unitOfWork.Commit();
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
                throw;
            }
        }

        private async Task SendAccountEmail(User user, string password)
        {
            try
            {

                var replacement = new StringDictionary
                {
                    ["FirstName"] = user.FirstName,
                    ["UserName"] = user.UserName,
                    ["DefaultPassword"] = password
                };

                var mail = new Mail(appConfig.AppEmail, "EkiHire.com: New staff account information", user.Email)
                {
                    BodyIsFile = true,
                    BodyPath = Path.Combine(_hostingEnvironment.ContentRootPath, CoreConstants.Url.AccountActivationEmail)
                };

                await _mailSvc.SendMailAsync(mail, replacement);
            }
            catch (Exception)
            {
            }
        }

        public Account FirstOrDefault(Expression<Func<Account, bool>> filter)
        {
            return _repo.FirstOrDefault(filter);
        }

        public IQueryable<Account> GetAll()
        {
            return _repo.GetAll();
        }

        public Task<AccountDTO> GetAccountsByemailAsync(string email)
        {
            var employees =
                from employee in _repo.GetAllIncluding(x => x.User)

                //join department in _departmentRepo.GetAll() on employee.DepartmentId equals department.Id
                //into departments
                //from department in departments.DefaultIfEmpty()

                //join wallet in _walletRepo.GetAll() on employee.User.WalletId equals wallet.Id
                //into wallets
                //from wallet in wallets.DefaultIfEmpty()

                //join terminal in _terminalRepo.GetAll() on employee.TerminalId equals terminal.Id
                //into terminals
                //from terminal in terminals.DefaultIfEmpty()

                where employee.User.Email == email

                select new AccountDTO
                {
                    Id = employee.Id,
                    FirstName = employee.User.FirstName,
                    LastName = employee.User.LastName,
                    MiddleName = employee.User.MiddleName,
                    Gender = employee.User.Gender,
                    Email = employee.User.Email,
                    PhoneNumber = employee.User.PhoneNumber,
                    Address = employee.User.Address,
                    //AccountPhoto = employee.User.Photo,
                    NextOfKin = employee.User.NextOfKinName,
                    NextOfKinPhone = employee.User.NextOfKinPhone,
                    //DepartmentName = department.Name,
                    //DepartmentId = department.Id,
                    //TerminalId = terminal.Id,
                    //TerminalName = terminal.Name,
                    Otp = employee.Otp,
                    //OtpIsUsed = employee.OtpIsUsed,
                    //AccountCode = employee.AccountCode,
                };

            return employees.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<bool> Verifyotp(string otp)
        {


            //var email = await _userSvc.FindByNameAsync(_serviceHelper.GetCurrentUserEmail());
            //var operationManager = await GetOperationManager(email.Email);
            //if (operationManager.Otp == otp && !operationManager.OtpIsUsed)
            //{
            //    await UpdateUsedAccountOtp(operationManager.Id);
            //    return true;
            //}
            return false;


        }
        //public async Task<List<AccountDTO>> GetAllAccountByTerminalId(int terminalId)
        //{

        //    var employee = await GetTerminalAccounts(terminalId);

        //    if (employee == null)
        //    {
        //        //throw await _helper.GetExceptionAsync(ErrorConstants.EMPLOYEE_NOT_EXIST);
        //        return new List<AccountDTO>();
        //    }

        //    return employee;

        //}

        public Task<IPagedList<AccountDTO>> GetAccounts(int pageNumber, int pageSize, string query)
        {
            var employees =
                from employee in _repo.GetAllIncluding(x => x.User)

                //join department in _departmentRepo.GetAll() on employee.DepartmentId equals department.Id
                //into departments
                //from department in departments.DefaultIfEmpty()

                //join terminal in _terminalRepo.GetAll() on employee.TerminalId equals terminal.Id
                //into terminals
                //from terminal in terminals.DefaultIfEmpty()

                let userRole = employee.User == null ? Enumerable.Empty<string>() : _userSvc.GetUserRoles(employee.User).Result

                where string.IsNullOrWhiteSpace(query) ||
                (employee.User.FirstName.Contains(query) ||
                    employee.User.LastName.Contains(query) ||
                    employee.User.Email.Contains(query) ||
                    employee.User.PhoneNumber.Contains(query))
                select new AccountDTO
                {
                    Id = employee.Id,
                    FirstName = employee.User.FirstName,
                    LastName = employee.User.LastName,
                    MiddleName = employee.User.MiddleName,
                    Gender = employee.User.Gender,
                    Email = employee.User.Email,
                    PhoneNumber = employee.User.PhoneNumber,
                    Address = employee.User.Address,
                    //AccountPhoto = employee.User.Photo,
                    NextOfKin = employee.User.NextOfKinName,
                    NextOfKinPhone = employee.User.NextOfKinPhone,
                    //DepartmentName = department.Name,
                    //DepartmentId = department.Id,
                    //TerminalId = terminal.Id,
                    //TerminalName = terminal.Name,
                    //AccountCode = employee.AccountCode,
                    RoleName = string.Join(",", userRole)
                };

            return employees.AsNoTracking().ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<AccountDTO> GetAccount(int id)
        {
            var employee = _repo.GetAllIncluding(x => x.User).FirstOrDefault(x => x.Id == id);

            var employeeDto = new AccountDTO
            {
                Id = employee.Id,
                FirstName = employee.User.FirstName,
                LastName = employee.User.LastName,
                MiddleName = employee.User.MiddleName,
                Gender = employee.User.Gender,
                Email = employee.User.Email,
                PhoneNumber = employee.User.PhoneNumber,
                Address = employee.User.Address,
                //AccountPhoto = employee.User.Photo,
                NextOfKin = employee.User.NextOfKinName,
                NextOfKinPhone = employee.User.NextOfKinPhone,
                //DepartmentId = employee.DepartmentId,
                //TerminalId = employee.TerminalId,
                //AccountCode = employee.AccountCode,
            };

            var userRole = _userSvc.GetUserRoles(employee.User).Result.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                var role = await _roleSvc.FindByName(userRole);
                employeeDto.RoleId = role.Id;
            }

            return employeeDto;
        }

        public async Task UpdateAccount(int id, AccountDTO model)
        {
            var employee = _repo.Get(id);

            var user = await _userSvc.FindFirstAsync(x => x.Id == employee.UserId);

            if (user is null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            //employee.AccountCode = model.AccountCode;
            //employee.DepartmentId = model.DepartmentId;
            //employee.TerminalId = model.TerminalId;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.MiddleName = model.MiddleName;
            user.Gender = model.Gender;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.NextOfKinName = model.NextOfKin;
            user.NextOfKinPhone = model.NextOfKinPhone;


            if (model.RoleId != 0)
            {
                var newRole = await _roleSvc.FindByIdAsync(model.RoleId);
                if (newRole != null && !await _userSvc.IsInRoleAsync(user, newRole.Name))
                {
                    await _userSvc.RemoveFromRolesAsync(user, await _userSvc.GetUserRoles(user));
                    await _userSvc.AddToRoleAsync(user, newRole.Name);
                }
            }

            await _userSvc.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
