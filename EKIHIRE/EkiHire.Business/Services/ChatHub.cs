using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EkiHire.Data.efCore.Context;
using EkiHire.Core.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EkiHire.Data.Repository;

namespace EkiHire.Business.Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserService _userSvc;
        public ChatHub(ILogger<ChatHub> logger,
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext, 
            IUserService userSvc
            )
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _httpContext = httpContext;
            _userSvc = userSvc;
        }
        public async Task SendMessage(Message message)
        {
            #region save message
            var currentUser = await _userManager.GetUserAsync(Context.User);
            string email = null;
            if(currentUser == null)
            {
                email = Context.User.FindFirst("name")?.Value;
                currentUser = await _userSvc.FindByNameAsync(email);
            }
            message.SenderId = currentUser.Id;
            message.When = DateTimeOffset.Now;
            _ = await _applicationDbContext.Messages.AddAsync(message);
            _ = await _applicationDbContext.SaveChangesAsync();
            #endregion
            await Clients.All.SendAsync("receiveMessage", message);
        }
        public async Task SendNotification()
        {

        }
    }
}
