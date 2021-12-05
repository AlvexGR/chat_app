using System.Threading.Tasks;
using ChatApp.DataAccess;
using ChatApp.Dtos.Models.Chats;
using ChatApp.Entities.Enums;
using ChatApp.Entities.Models;
using ChatApp.Services.IServices;
using ChatApp.Utilities.Constants;
using ChatApp.Utilities.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Apis.Hubs.v1
{
    public class ChatHub : BaseHub
    {
        private readonly IUserService _userService;
        private readonly IFriendService _friendShipService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(
            IUserService userService,
            IFriendService friendShipService,
            IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _friendShipService = friendShipService;
            _unitOfWork = unitOfWork;
        }

        public async Task SendMessage(PrivateChatMessageDto privateChatMessage)
        {
            var chatMessageRepo = _unitOfWork.GetRepository<ChatMessage>();
            await chatMessageRepo.Insert(new ChatMessage
            {
                SenderId = Context.GetHttpContext().UserId(),
                ReceiverId = privateChatMessage.ReceiverId,
                MessageType = MessageType.Message,
                ReceiverType = ReceiverType.Private,
                Message = privateChatMessage.Content
            });

            var allMessages = await chatMessageRepo.FindAll();
            await Clients.All.SendAsync(HubMethods.ReceiveMessage, privateChatMessage);
        }

        //public async Task SendMessage(ChatMessageDto messageDto)
        //{
        //    // TODO: Check friendship

        //    await Clients.All.SendAsync(HubMethods.ReceiveMessage);
        //}
    }
}
