using StockProject.Common;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.RoleDtos;
using StockProject.Dtos.UserDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Interfaces
{
    public interface IUserService : IService<UserCreateDto, UserUpdateDto, UserListDto, User>
    {
        Task<IResponse<UserListDto>> GetByIdAsync(int id);
        Task<IResponse<UserListDto>> CheckUserAsync(LoginDto dto);
        Task<IResponse<List<RoleListDto>>> GetRolesByUserIdAsync(int userId);
    }
}
