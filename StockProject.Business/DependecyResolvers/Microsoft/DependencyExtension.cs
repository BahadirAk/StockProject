using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockProject.Business.Interfaces;
using StockProject.Business.Services;
using StockProject.Business.ValidationRules;
using StockProject.DataAccess.Contexts;
using StockProject.DataAccess.Interfaces;
using StockProject.DataAccess.Repositories;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.AuthDtos;
using StockProject.Dtos.BasketDtos;
using StockProject.Dtos.BasketProductDtos;
using StockProject.Dtos.CategoryDtos;
using StockProject.Dtos.OrderDtos;
using StockProject.Dtos.ProductDtos;
using StockProject.Dtos.UserDtos;
using StockProject.Dtos.UserRoleDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.DependecyResolvers.Microsoft
{
    public static class DependencyExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StockContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Local"));
            });

            services.AddScoped<IUow, Uow>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBasketService, BasketService>();

            services.AddTransient<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
            services.AddTransient<IValidator<CategoryUpdateDto>, CategoryUpdateDtoValidator>();
            services.AddTransient<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
            services.AddTransient<IValidator<OrderUpdateDto>, OrderUpdateDtoValidator>();
            services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            services.AddTransient<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
            services.AddTransient<IValidator<UserRoleCreateDto>, UserRoleCreateDtoValidator>();
            services.AddTransient<IValidator<UserRoleUpdateDto>, UserRoleUpdateDtoValidator>();
            services.AddTransient<IValidator<UserCreateDto>, UserCreateDtoValidator>();
            services.AddTransient<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
            services.AddTransient <IValidator<BasketCreateDto>, BasketCreateDtoValidator>();
            services.AddTransient <IValidator<BasketUpdateDto>, BasketUpdateDtoValidator>();
            services.AddTransient<IValidator<BasketProductCreateDto>, BasketProductCreateDtoValidator>();
            services.AddTransient<IValidator<BasketProductUpdateDto>, BasketProductUpdateDtoValidator>();
        }
    }
}
