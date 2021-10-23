﻿using System.Threading.Tasks;
using ChatApp.Dtos.Common;
using ChatApp.Dtos.Models.Users;

namespace ChatApp.Services.IServices
{
    public interface IUserService
    {
        Task<BaseResponseDto<bool>> InsertUser(InsertUserDto insertUserDto);
    }
}