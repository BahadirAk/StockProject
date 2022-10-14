using FluentValidation;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.RoleDtos;
using StockProject.Dtos.UserDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class UserService : Service<UserCreateDto, UserUpdateDto, UserListDto, User>, IUserService
    {
        private readonly IUow _uow;
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<UserCreateDto> _createDtoValidator;

        public UserService(IUow uow, IValidator<UserCreateDto> createDtoValidator, IValidator<UserUpdateDto> updateDtoValidator, IValidator<LoginDto> loginDtoValidator) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _loginDtoValidator = loginDtoValidator;
            _createDtoValidator = createDtoValidator;
        }
        public async Task<IResponse<UserListDto>> GetByIdAsync(int id)
        {
            var user = await _uow.GetRepository<User>().GetByFilterAsync(x => x.Id == id);
            if (user != null)
            {
                UserListDto dto = new UserListDto
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Surname = user.Surname,
                    Balance = user.Balance,
                    Username = user.Username,
                    Password = user.Password,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate,
                    IsDeleted = user.IsDeleted
                };
                return new Response<UserListDto>(ResponseType.Success, dto);
            }
            return new Response<UserListDto>(ResponseType.NotFound, $"{id} sine sahip kullanıcı bulunamadı!!!");
        }
        public async Task<IResponse<UserListDto>> CheckUserLoginAsync(LoginDto dto)
        {
            var validationResult = _loginDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = await _uow.GetRepository<User>().GetByFilterAsync(x => x.Username == dto.Username && x.Password == dto.Password);
                if (user != null)
                {
                    var mappedUser = new UserListDto
                    {
                        Id = user.Id,
                        Firstname = user.Firstname,
                        Surname = user.Surname,
                        Balance = user.Balance,
                        Username = user.Username,
                        Password = user.Password,
                        CreatedDate = user.CreatedDate,
                        ModifiedDate = user.ModifiedDate,
                        IsDeleted = user.IsDeleted
                    };
                    return new Response<UserListDto>(ResponseType.Success, mappedUser);
                }
                return new Response<UserListDto>(ResponseType.NotFound, "Kullanıcı adı veya şifre hatalı!!!");
            }
            return new Response<UserListDto>(ResponseType.ValidationError, "Kullanıcı adı veya şifre boş olamaz!!!");
        }
        public async Task<IResponse<List<RoleListDto>>> GetRolesByUserIdAsync(int userId)
        {
            var roles = await _uow.GetRepository<Role>().GetAllAsync(x => x.UserRoles.Any(x => x.UserId == userId));
            if (roles != null)
            {
                var dto = new List<RoleListDto>();
                foreach (var role in roles)
                {
                    dto.Add(new RoleListDto
                    {
                        Id = role.Id,
                        Definition = role.Definition,
                        CreatedDate = role.CreatedDate,
                        ModifiedDate = role.ModifiedDate,
                        IsDeleted = role.IsDeleted
                    });
                }
                return new Response<List<RoleListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<RoleListDto>>(ResponseType.NotFound, "Rol bulunamadı!!!");
        }
        public async Task<IResponse<UserCreateDto>> CreateWithRoleAsync(UserCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = await _uow.GetRepository<User>().GetByFilterAsync(x => x.Username == dto.Username);
                if (user == null)
                {
                    var mappedUser = new User
                    {
                        Firstname = dto.Firstname,
                        Surname = dto.Surname,
                        Username = dto.Username,
                        Password = dto.Password,
                        Balance = dto.Balance,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    await _uow.GetRepository<User>().CreateAsync(mappedUser);
                    await _uow.SaveChangesAsync();
                    await _uow.GetRepository<UserRole>().CreateAsync(new UserRole
                    {
                        UserId = mappedUser.Id,
                        RoleId = 2,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    });
                    var createBasketResult = await CreateBasketForUserAsync(mappedUser.Id);
                    await _uow.SaveChangesAsync();
                    return new Response<UserCreateDto>(ResponseType.Success, dto);
                }
                return new Response<UserCreateDto>(ResponseType.ValidationError, $"{dto.Username} böyle bir kullanıcı mevcut!!!");
            }
            return new Response<UserCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        private async Task<IResponse> CreateBasketForUserAsync(int userId)
        {
            var basket = new Basket
            {
                UserId = userId,
                SubTotal = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsDeleted = false
            };
            await _uow.GetRepository<Basket>().CreateAsync(basket);
            return new Response(ResponseType.Success);
        }
    }
}
