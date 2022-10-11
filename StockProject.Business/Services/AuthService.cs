﻿using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.RoleDtos;
using StockProject.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IResponse<UserListDto>> Login(LoginDto dto)
        {
            var result = await _userService.CheckUserAsync(dto);
            if (result.ResponseType == ResponseType.Success)
            {
                var roleResult = await _userService.GetRolesByUserIdAsync(result.Data.Id);
                result.Data.Roles = roleResult.Data;
                return new Response<UserListDto>(ResponseType.Success, result.Data);
            }
            return new Response<UserListDto>(ResponseType.ValidationError, result.Data);
        }
    }
}
