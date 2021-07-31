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
using log4net;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;

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
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
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
        public async Task Ping()
        {

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
            var recipient = currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.Id == message.RecipientId);
            foreach (var connectionId in _connections.GetConnections(recipient?.UserName??"nothing"))
            {
                await Clients.Client(connectionId).SendAsync("receiveMessage", message);
            }

            //Microsoft.AspNetCore.SignalR
            //await Clients.User(currentUser.UserName).SendAsync("receiveMessage", message);
            //await Clients.Client(Context.ConnectionId).SendAsync("receiveMessage", message);
        }

        [AllowAnonymous]
        public async Task SendNotification(Notification notification)
        {
            #region save notification
            _ = await _applicationDbContext.Notification.AddAsync(notification);
            _ = await _applicationDbContext.SaveChangesAsync();
            #endregion
            foreach (var connectionId in _connections.GetConnections(notification.RecipientUserName ?? "nothing"))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
            }
            //var currentUser = await _userManager.GetUserAsync(Context.User);
            //string email = null;
            //if (currentUser == null)
            //{
            //    email = Context.User.FindFirst("name")?.Value;
            //    //currentUser = await _userSvc.FindByNameAsync(email);
            //    currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == email);
            //}
            //switch (notification.NotificationType)
            //{
            //    case NotificationType.Welcome:
            //        //await Clients.All.SendAsync("ReceiveNotification", notification);

            //        break;
            //    case NotificationType.AdApproval:
            //    case NotificationType.AdDenial:
            //        await Clients.All.SendAsync("ReceiveNotification", notification);
            //        break;
            //    case NotificationType.Chat:

            //        break;
            //}
        }
        //treat connection id
        public async override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            var currentUser = await _userManager.GetUserAsync(Context.User);
            string email = null;
            if (currentUser == null)
            {
                email = Context.User.FindFirst("name")?.Value;
                //currentUser = await _userSvc.FindByNameAsync(email);
                currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == email);
            }
            name = name ?? currentUser.UserName;
            //_connections.Add(name, Context.ConnectionId);
            if (!string.IsNullOrWhiteSpace(name) && !_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;
            var currentUser = await _userManager.GetUserAsync(Context.User);
            string email = null;
            if (currentUser == null)
            {
                email = Context.User.FindFirst("name")?.Value;
                //currentUser = await _userSvc.FindByNameAsync(email);
                currentUser = await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == email);
            }
            name = name ?? currentUser.UserName;

            //_connections.Remove(name, Context.ConnectionId);
            if (!string.IsNullOrWhiteSpace(name) && _connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Remove(name, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}
