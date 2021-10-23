using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatApp.Dtos.Models.Auths;
using ChatApp.Entities.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Utilities.Extensions
{
    public static class StringExtension
    {
        public static string HashMd5(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            using var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(source);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GenerateAccessToken(this User user, JwtSettingDto jwtSettingDto)
        {
            if (jwtSettingDto == null)
            {
                throw new ArgumentNullException(nameof(jwtSettingDto));
            }

            if (string.IsNullOrEmpty(jwtSettingDto.Secret))
            {
                throw new ArgumentNullException(nameof(jwtSettingDto.Secret));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettingDto.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTimeExtension.Get().AddDays(jwtSettingDto.ExpiredInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
