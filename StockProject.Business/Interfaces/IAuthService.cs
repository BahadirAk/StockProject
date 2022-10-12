using StockProject.Common;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.RoleDtos;
using StockProject.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Interfaces
{
    public interface IAuthService
    {
        Task<IResponse<UserListDto>> LoginAsync(LoginDto dto);
        Task<IResponse<UserListDto>> RegisterAsync(UserCreateDto dto);
    }
}
