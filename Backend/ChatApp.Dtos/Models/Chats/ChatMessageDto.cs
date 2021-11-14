using ChatApp.Entities.Enums;

namespace ChatApp.Dtos.Models.Chats
{
    public class ChatMessageDto
    {
        public string ReceiverId { get; set; }

        public MessageType MessageType { get; set; }

        public string Content { get; set; }
    }
}
