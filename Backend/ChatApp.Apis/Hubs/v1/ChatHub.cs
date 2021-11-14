using System.Threading.Tasks;
using ChatApp.DataAccess;
using ChatApp.Dtos.Models.Chats;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Apis.Hubs.v1
{
    public class ChatHub : BaseHub
    {
        private readonly IUserService _userService;
        private readonly IFriendShipService _friendShipService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IAuthService authService,
            IUserService userService,
            IFriendShipService friendShipService,
            IUnitOfWork unitOfWork)
            : base(authService)
        {
            _userService = userService;
            _friendShipService = friendShipService;
            _unitOfWork = unitOfWork;
        }

        public async Task SendMessage(ChatMessageDto messageDto)
        {
            var user = _userService.Get(messageDto.ReceiverId);
            if (user == null) return;

            // TODO: Check friendship

            await Clients.All.SendAsync(HubMethods.ReceiveMessage);
        }
    }
}
