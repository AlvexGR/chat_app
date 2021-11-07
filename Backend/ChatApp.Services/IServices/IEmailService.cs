using System.Threading.Tasks;
using ChatApp.Dtos.Models.Emails;

namespace ChatApp.Services.IServices
{
    public interface IEmailService
    {
        Task<bool> Send(EmailDto emailDto);
    }
}
