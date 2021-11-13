using System.Threading.Tasks;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Auths;

namespace ChatApp.Services.IServices
{
    public interface IAuthService
    {
        Task<BaseResponseDto<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);

        Task<BaseResponseDto<LoginResponseDto>> Register(RegisterDto registerDto);

        Task<BaseResponseDto<LoginResponseDto>> GoogleLogin(GoogleLoginRequestDto loginRequestDto);

        Task<BaseResponseDto<bool>> ForgotPassword(string email);
    }
}
