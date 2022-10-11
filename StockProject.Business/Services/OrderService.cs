﻿using FluentValidation;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.DataAccess.Interfaces;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.OrderDtos;
using StockProject.Dtos.ProductDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class OrderService : Service<OrderCreateDto, OrderUpdateDto, OrderListDto, Order>, IOrderService
    {
        private readonly IUow _uow;
        private readonly IValidator<OrderCreateDto> _createDtoValidator;
        private readonly IValidator<OrderUpdateDto> _updateDtoValidator;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public OrderService(IUow uow, IValidator<OrderCreateDto> createDtoValidator, IValidator<OrderUpdateDto> updateDtoValidator, IUserService userService, IProductService productService) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
            _userService = userService;
            _productService = productService;
        }
        public async Task<IResponse<List<OrderListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync();
            var dto = new List<OrderListDto>();
            foreach (var order in data)
            {
                dto.Add(new OrderListDto
                {
                    Id = order.Id,
                    UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                    ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                    CreatedDate = order.CreatedDate,
                    ModifiedDate = order.ModifiedDate,
                    IsDeleted = order.IsDeleted
                });
            }
            return new Response<List<OrderListDto>>(ResponseType.Success, dto);
        }
        public async Task<IResponse<List<OrderListDto>>> GetActivesAsync()
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => !x.IsDeleted);
            var dto = new List<OrderListDto>();
            foreach (var order in data)
            {
                dto.Add(new OrderListDto
                {
                    Id = order.Id,
                    UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                    ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                    CreatedDate = order.CreatedDate,
                    ModifiedDate = order.ModifiedDate,
                    IsDeleted = order.IsDeleted
                });
            }
            return new Response<List<OrderListDto>>(ResponseType.Success, dto);
        }
        public async Task<IResponse<OrderListDto>> GetByIdAsync(int id)
        {
            var data = await _uow.GetRepository<Order>().GetByFilterAsync(x => x.Id == id);
            if (data != null)
            {
                OrderListDto dto = new OrderListDto
                {
                    Id = data.Id,
                    UserName = _userService.GetByIdAsync(data.UserId).Result.Data.Username,
                    ProductName = _productService.GetByIdAsync(data.ProductId).Result.Data.Name,
                    Quantity = data.Quantity,
                    TotalPrice = data.TotalPrice,
                    CreatedDate = data.CreatedDate,
                    ModifiedDate = data.ModifiedDate,
                    IsDeleted = data.IsDeleted
                };
                return new Response<OrderListDto>(ResponseType.Success, dto);
            }
            return new Response<OrderListDto>(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<List<OrderListDto>>> GetOrdersByUserIdAsync(int userId)
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.UserId == userId);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                        ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                        Quantity = order.Quantity,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        IsDeleted = order.IsDeleted
                    });
                }
                return new Response<List<OrderListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{userId} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<List<OrderListDto>>> GetActiveOrdersByUserIdAsync(int userId)
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.UserId == userId && !x.IsDeleted);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                        ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                        Quantity = order.Quantity,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        IsDeleted = order.IsDeleted
                    });
                }
                return new Response<List<OrderListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{userId} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<List<OrderListDto>>> GetOrdersByProductIdAsync(int productId)
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.ProductId == productId);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                        ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                        Quantity = order.Quantity,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        IsDeleted = order.IsDeleted
                    });
                }
                return new Response<List<OrderListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{productId} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<List<OrderListDto>>> GetActiveOrdersByProductIdAsync(int productId)
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.ProductId == productId && !x.IsDeleted);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        UserName = _userService.GetByIdAsync(order.UserId).Result.Data.Username,
                        ProductName = _productService.GetByIdAsync(order.ProductId).Result.Data.Name,
                        Quantity = order.Quantity,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        IsDeleted = order.IsDeleted
                    });
                }
                return new Response<List<OrderListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{productId} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<OrderCreateDto>> CreateAsync(OrderCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var entity = new Order
                {
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    TotalPrice = _productService.GetByIdAsync(dto.ProductId).Result.Data.Price * dto.Quantity,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                await _uow.GetRepository<Order>().CreateAsync(entity);
                await _uow.SaveChangesAsync();
                return new Response<OrderCreateDto>(ResponseType.Success, dto);
            }
            return new Response<OrderCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<IResponse> RemoveAsync(int id)
        {
            var unchangedEntity = await _uow.GetRepository<Order>().FindAsync(id);
            if (unchangedEntity != null)
            {
                var data = new Order
                {
                    Id = unchangedEntity.Id,
                    UserId = unchangedEntity.UserId,
                    ProductId = unchangedEntity.ProductId,
                    Quantity = unchangedEntity.Quantity,
                    TotalPrice = unchangedEntity.TotalPrice,
                    CreatedDate = unchangedEntity.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = true
                };
                _uow.GetRepository<Order>().Update(data, unchangedEntity);
                await _uow.SaveChangesAsync();
                return new Response(ResponseType.Success);
            }
            return new Response(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<OrderUpdateDto>> UpdateAsync(OrderUpdateDto dto)
        {
            var validationResult = _updateDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var unchangedEntity = await _uow.GetRepository<Order>().FindAsync(dto.Id);
                if (unchangedEntity != null)
                {
                    var entity = new Order
                    {
                        Id = dto.Id,
                        UserId = dto.UserId,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        TotalPrice = _productService.GetByIdAsync(dto.ProductId).Result.Data.Price * dto.Quantity,
                        CreatedDate = unchangedEntity.CreatedDate,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = unchangedEntity.IsDeleted
                    };
                    _uow.GetRepository<Order>().Update(entity, unchangedEntity);
                    await _uow.SaveChangesAsync();
                    return new Response<OrderUpdateDto>(ResponseType.Success, dto);
                }
                return new Response<OrderUpdateDto>(ResponseType.NotFound, $"{dto.Id} sine sahip data bulunamadı!!!");
            }
            return new Response<OrderUpdateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
    }
}