using ChatApp.Services.IServices;

namespace ChatApp.Apis.Hubs.v1
{
    public class NotificationHub : BaseHub
    {
        public NotificationHub(IAuthService authService)
            : base(authService)
        {
        }
    }
}
