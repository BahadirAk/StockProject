using FluentValidation;
using Microsoft.AspNetCore.Http;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.Common.Enums;
using StockProject.DataAccess.Interfaces;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.Interfaces;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _basketService;
        private int UserId { get { return _httpContextAccessor.GetUserId(); } }

        public OrderService(IUow uow, IValidator<OrderCreateDto> createDtoValidator, IValidator<OrderUpdateDto> updateDtoValidator, IUserService userService, IProductService productService, IHttpContextAccessor httpContextAccessor, IBasketService basketService) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
            _userService = userService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _basketService = basketService;
        }
        #region Get
        public async Task<IResponse<List<OrderListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<Order>().GetAllAsync();
            var dto = new List<OrderListDto>();
            foreach (var order in data)
            {
                dto.Add(new OrderListDto
                {
                    Id = order.Id,
                    Basket = _basketService.GetByIdAsync(order.BasketId).Result.Data,
                    TotalPrice = order.TotalPrice,
                    CreatedDate = order.CreatedDate,
                    ModifiedDate = order.ModifiedDate,
                    IsDeleted = order.IsDeleted
                });
            }
            return new Response<List<OrderListDto>>(ResponseType.Success, dto);
        }
        //public async Task<IResponse<List<OrderListDto>>> GetActivesAsync()
        //{
        //    var data = await _uow.GetRepository<Order>().GetAllAsync(x => !x.IsDeleted);
        //    var dto = new List<OrderListDto>();
        //    foreach (var order in data)
        //    {
        //        dto.Add(new OrderListDto
        //        {
        //            Id = order.Id,
        //            Basket = _basketService.GetActiveBasketByIdAsync(order.BasketId).Result.Data,
        //            TotalPrice = order.TotalPrice,
        //            CreatedDate = order.CreatedDate,
        //            ModifiedDate = order.ModifiedDate,
        //            IsDeleted = order.IsDeleted
        //        });
        //    }
        //    return new Response<List<OrderListDto>>(ResponseType.Success, dto);
        //}
        public async Task<IResponse<OrderListDto>> GetByIdAsync(int id)
        {
            var data = await _uow.GetRepository<Order>().GetByFilterAsync(x => x.Id == id);
            if (data != null)
            {
                OrderListDto dto = new OrderListDto
                {
                    Id = data.Id,
                    Basket = _basketService.GetByIdAsync(data.BasketId).Result.Data,
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
            var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == userId);
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.BasketId == basket.Id);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        Basket = _basketService.GetByIdAsync(order.BasketId).Result.Data,
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
        //public async Task<IResponse<List<OrderListDto>>> GetActiveOrdersByUserIdAsync(int userId)
        //{
        //    var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == userId);
        //    var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.BasketId == basket.Id && !x.IsDeleted);
        //    if (data != null)
        //    {
        //        List<OrderListDto> dto = new List<OrderListDto>();
        //        foreach (var order in data)
        //        {
        //            dto.Add(new OrderListDto
        //            {
        //                Id = order.Id,
        //                Basket = _basketService.GetActiveBasketByIdAsync(order.BasketId).Result.Data,
        //                TotalPrice = order.TotalPrice,
        //                CreatedDate = order.CreatedDate,
        //                ModifiedDate = order.ModifiedDate,
        //                IsDeleted = order.IsDeleted
        //            });
        //        }
        //        return new Response<List<OrderListDto>>(ResponseType.Success, dto);
        //    }
        //    return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{userId} sine sahip data bulunamadı!!!");
        //}
        public async Task<IResponse<List<OrderListDto>>> GetOrdersByAuthorizedUserIdAsync()
        {
            var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == UserId);
            var data = await _uow.GetRepository<Order>().GetAllAsync(x => x.BasketId == basket.Id && !x.IsDeleted);
            if (data != null)
            {
                List<OrderListDto> dto = new List<OrderListDto>();
                foreach (var order in data)
                {
                    dto.Add(new OrderListDto
                    {
                        Id = order.Id,
                        Basket = _basketService.GetByIdAsync(order.BasketId).Result.Data,
                        TotalPrice = order.TotalPrice,
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        IsDeleted = order.IsDeleted
                    });
                }
                return new Response<List<OrderListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<OrderListDto>>(ResponseType.NotFound, $"{UserId} sine sahip data bulunamadı!!!");
        }
        #endregion
        #region Create Update Remove
        public async Task<IResponse<OrderCreateDto>> CreateAsync()
        {
            var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == UserId);
            var productsInBasket = await _uow.GetRepository<BasketProduct>().GetAllAsync(x => x.BasketId == basket.Id);
            if (productsInBasket.Count > 0)
            {
                var order = new Order
                {
                    BasketId = basket.Id,
                    TotalPrice = basket.SubTotal,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                await _uow.GetRepository<Order>().CreateAsync(order);
                var basketResult = await ResetBasketSubTotalAsync(basket);
                var removeProductsResult = await RemoveProductsFromBasketAsync(basket.Id);
                var updateUserBalanceResult = await UpdateUserBalanceAsync(basket.UserId, basket.SubTotal);
                var updateProductQuantityResult = await UpdateProductQuantityAsync(productsInBasket);
                if (basketResult.ResponseType == ResponseType.Success && removeProductsResult.ResponseType == ResponseType.Success && updateUserBalanceResult.ResponseType == ResponseType.Success && updateProductQuantityResult.ResponseType == ResponseType.Success)
                {
                    await _uow.SaveChangesAsync();
                    return new Response<OrderCreateDto>(ResponseType.Success, "Siparişiniz oluşturuldu!!!");
                }
                return new Response<OrderCreateDto>(new OrderCreateDto { BasketId = basket.Id, TotalPrice = order.TotalPrice }, new List<CustomValidationError> { new CustomValidationError { PropertyName = "User", ErrorMessage = updateUserBalanceResult.Message }, new CustomValidationError { PropertyName = "Product", ErrorMessage = updateProductQuantityResult.Message } });
            }
            return new Response<OrderCreateDto>(ResponseType.NotFound, "Sepetinizde ürün bulunmamaktadır!!!");
        }
        //public async Task<IResponse> RemoveAsync(int id)
        //{
        //    var unchangedEntity = await _uow.GetRepository<Order>().FindAsync(id);
        //    if (unchangedEntity != null)
        //    {
        //        var data = new Order
        //        {
        //            Id = unchangedEntity.Id,
        //            UserId = unchangedEntity.UserId,
        //            //ProductId = unchangedEntity.ProductId,
        //            Quantity = unchangedEntity.Quantity,
        //            TotalPrice = unchangedEntity.TotalPrice,
        //            CreatedDate = unchangedEntity.CreatedDate,
        //            ModifiedDate = DateTime.Now,
        //            IsDeleted = true
        //        };
        //        _uow.GetRepository<Order>().Update(data, unchangedEntity);
        //        await _uow.SaveChangesAsync();
        //        return new Response(ResponseType.Success);
        //    }
        //    return new Response(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        //}
        //public async Task<IResponse<OrderUpdateDto>> UpdateAsync(OrderUpdateDto dto)
        //{
        //    var validationResult = _updateDtoValidator.Validate(dto);
        //    if (validationResult.IsValid)
        //    {
        //        var unchangedEntity = await _uow.GetRepository<Order>().FindAsync(dto.Id);
        //        if (unchangedEntity != null)
        //        {
        //            var entity = new Order
        //            {
        //                Id = dto.Id,
        //                UserId = UserId,
        //                //ProductId = dto.ProductId,
        //                Quantity = dto.Quantity,
        //                TotalPrice = _productService.GetByIdAsync(dto.ProductId).Result.Data.Price * dto.Quantity,
        //                CreatedDate = unchangedEntity.CreatedDate,
        //                ModifiedDate = DateTime.Now,
        //                IsDeleted = unchangedEntity.IsDeleted
        //            };
        //            //await UpdateBalanceAndQuantityAsync(entity.UserId, entity.ProductId, entity.Quantity);
        //            _uow.GetRepository<Order>().Update(entity, unchangedEntity);
        //            await _uow.SaveChangesAsync();
        //            return new Response<OrderUpdateDto>(ResponseType.Success, dto);
        //        }
        //        return new Response<OrderUpdateDto>(ResponseType.NotFound, $"{dto.Id} sine sahip data bulunamadı!!!");
        //    }
        //    return new Response<OrderUpdateDto>(dto, validationResult.ConvertToCustomValidationError());
        //}
        //#endregion
        //#region Balance ve Quantity İşlemleri
        //private async Task<IResponse> UpdateUserBalanceAndProductQuantityAsync(int productId, int userId, int quantity, decimal totalPrice)
        //{
        //    var productResult = UpdateProductQuantityAsync(productId, quantity);
        //    if (productResult.Result.ResponseType == ResponseType.Success)
        //    {
        //        var userResult = UpdateUserBalanceAsync(userId, totalPrice);
        //        if (userResult.Result.ResponseType == ResponseType.Success)
        //        {
        //            return new Response(ResponseType.Success);
        //        }
        //        return new Response(ResponseType.ValidationError, "Kullanıcının yeterli bakiyesi bulunmamaktadır!!!");
        //    }
        //    return new Response(ResponseType.ValidationError, "Stokta yeteri miktarda ürün bulunmamaktadır!!!");
        //}
        //private async Task<IResponse> UpdateProductQuantityAsync(int productId, int quantity)
        //{
        //    var product = _uow.GetRepository<Product>().GetByFilterAsync(x => x.Id == productId).Result;
        //    if (product.Quantity >= quantity)
        //    {
        //        var updateProduct = new Product
        //        {
        //            Id = product.Id,
        //            Name = product.Name,
        //            Quantity = product.Quantity - quantity,
        //            Price = product.Price,
        //            CategoryId = product.CategoryId,
        //            CreatedDate = product.CreatedDate,
        //            ModifiedDate = DateTime.Now,
        //            IsDeleted = product.IsDeleted
        //        };
        //        _uow.GetRepository<Product>().UpdateModified(updateProduct);
        //        return new Response(ResponseType.Success);
        //    }
        //    return new Response(ResponseType.ValidationError);
        //}
        //private async Task<IResponse> UpdateUserBalanceAsync(int userId, decimal totalPrice)
        //{
        //    var user = _uow.GetRepository<User>().GetByFilterAsync(x => x.Id == userId).Result;
        //    if (user.Balance >= totalPrice)
        //    {
        //        var updateUser = new User
        //        {
        //            Id = user.Id,
        //            Firstname = user.Firstname,
        //            Surname = user.Surname,
        //            Username = user.Username,
        //            Password = user.Password,
        //            Balance = user.Balance - totalPrice,
        //            CreatedDate = user.CreatedDate,
        //            ModifiedDate = DateTime.Now,
        //            IsDeleted = user.IsDeleted
        //        };
        //        _uow.GetRepository<User>().UpdateModified(updateUser);
        //        return new Response(ResponseType.Success);
        //    }
        //    return new Response(ResponseType.ValidationError);
        //}
        #endregion
        #region Helper
        private async Task<IResponse> ResetBasketSubTotalAsync(Basket basket)
        {
            var entity = new Basket
            {
                Id = basket.Id,
                UserId = basket.UserId,
                SubTotal = 0,
                CreatedDate = basket.CreatedDate,
                ModifiedDate = DateTime.Now,
                IsDeleted = basket.IsDeleted
            };
            _uow.GetRepository<Basket>().UpdateModified(entity);
            return new Response(ResponseType.Success);
        }
        private async Task<IResponse> RemoveProductsFromBasketAsync(int basketId)
        {
            var productsInBasket = await _uow.GetRepository<BasketProduct>().GetAllAsync(x => x.BasketId == basketId);
            foreach (var product in productsInBasket)
            {
                var entity = new BasketProduct
                {
                    Id = product.Id,
                    BasketId = product.BasketId,
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    TotalPrice = product.TotalPrice,
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = true
                };
                _uow.GetRepository<BasketProduct>().UpdateModified(entity);
            }
            return new Response(ResponseType.Success);
        }
        private async Task<IResponse> UpdateUserBalanceAsync(int userId, decimal totalPrice)
        {
            var user = await _uow.GetRepository<User>().GetByFilterAsync(x => x.Id == userId);
            if (totalPrice > user.Balance)
            {
                return new Response(ResponseType.ValidationError, "Kullanıcının yeterli bakiyesi bulunmamaktadır!!!");
            }
            var entity = new User
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Surname = user.Surname,
                Username = user.Username,
                Password = user.Password,
                Balance = user.Balance - totalPrice,
                CreatedDate = user.CreatedDate,
                ModifiedDate = DateTime.Now,
                IsDeleted = user.IsDeleted
            };
            _uow.GetRepository<User>().UpdateModified(entity);
            return new Response(ResponseType.Success);
        }
        private async Task<IResponse> UpdateProductQuantityAsync(List<BasketProduct> productsInBasket)
        {
            foreach (var productInBasket in productsInBasket)
            {
                var product = await _uow.GetRepository<Product>().GetByFilterAsync(x => x.Id == productInBasket.ProductId);
                if (productInBasket.Quantity > product.Quantity)
                {
                    return new Response(ResponseType.ValidationError, "Stokta yeterli miktarda ürün bulunmamaktadır!!!");
                }
                var entity = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = product.Quantity - productInBasket.Quantity,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = product.IsDeleted
                };
                _uow.GetRepository<Product>().UpdateModified(entity);
            }
            return new Response(ResponseType.Success);
        }
        #endregion
    }
}
