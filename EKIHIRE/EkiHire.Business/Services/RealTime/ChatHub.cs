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
using EkiHire.Core.Domain.Entities.Enums;
namespace EkiHire.Business.Services
{
    public interface IChatHub
    {
        Task SendMessage(Message message);
        Task SendNotification(Notification notification);
    }
    [Authorize]
    public class ChatHub : Hub, IChatHub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        //private readonly IUserService _userSvc;
        public ChatHub(ILogger<ChatHub> logger,
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager,
            IHttpContextAccessor httpContext
            //,IUserService userSvc
            )
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _httpContext = httpContext;
            //_userSvc = userSvc;
        }
        public async Task SendMessage(Message message)
        {
            #region save message
            var currentUser = await _userManager.GetUserAsync(Context.User);
            string email = null;
            if(currentUser == null)
            {
                email = Context.User.FindFirst("name")?.Value;
                //currentUser = await _userSvc.FindByNameAsync(email);
                currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == email);
            }
            message.SenderId = currentUser.Id;
            message.When = DateTimeOffset.Now;
            _ = await _applicationDbContext.Messages.AddAsync(message);
            _ = await _applicationDbContext.SaveChangesAsync();
            #endregion

            //await Clients.All.SendAsync("receiveMessage", message);
            await Clients.Client(Context.ConnectionId).SendAsync("receiveMessage", message);
        }

        [AllowAnonymous]
        public async Task SendNotification(Notification notification)
        {
            switch (notification.NotificationType)
            {
                case NotificationType.Welcome:
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", notification);
                    //await Clients.All.SendAsync($"ReceiveNotification{}", notification);
                    break;
                case NotificationType.AdApproval:
                case NotificationType.AdDenial:
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", notification);
                    break;
                case NotificationType.Chat:

                    break;
            }
            
        }
    }
}
