using FluentValidation;
using Microsoft.AspNetCore.Http;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.BasketDtos;
using StockProject.Dtos.BasketProductDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class BasketService : Service<BasketCreateDto, BasketUpdateDto, BasketListDto, BasketProduct>, IBasketService
    {
        private readonly IUow _uow;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<BasketProductCreateDto> _basketProductCreateDtoValidator;
        private int UserId { get { return _httpContextAccessor.GetUserId(); } }
        public BasketService(IValidator<BasketCreateDto> createDtoValidator, IValidator<BasketUpdateDto> updateDtoValidator, IUow uow, IUserService userService, IProductService productService, IHttpContextAccessor httpContextAccessor, IValidator<BasketProductCreateDto> basketProductCreateDtoValidator) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _userService = userService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _basketProductCreateDtoValidator = basketProductCreateDtoValidator;
        }
        public async Task<IResponse<BasketListDto>> GetByUserIdAsync()
        {
            var data = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == UserId && !x.IsDeleted);
            if (data != null)
            {
                BasketListDto dto = new BasketListDto
                {
                    Id = data.Id,
                    UserName = _userService.GetByIdAsync(UserId).Result.Data.Username,
                    SubTotal = data.SubTotal,
                    CreateDate = data.CreatedDate,
                    ModifiedDate = data.ModifiedDate,
                    IsDeleted = data.IsDeleted
                };
                var productsInBasket = await _uow.GetRepository<BasketProduct>().GetAllAsync(x => x.BasketId == dto.Id && !x.IsDeleted);
                if (productsInBasket.Count > 0)
                {
                    List<BasketProductListDto> productsInUserBasket = new List<BasketProductListDto>();
                    foreach (var productInBasket in productsInBasket)
                    {
                        productsInUserBasket.Add(new BasketProductListDto
                        {
                            Id = productInBasket.Id,
                            BasketId = dto.Id,
                            ProductName = _productService.GetByIdAsync(productInBasket.ProductId).Result.Data.Name,
                            Quantity = productInBasket.Quantity,
                            TotalPrice = productInBasket.TotalPrice,
                            CreatedDate = productInBasket.CreatedDate,
                            ModifiedDate = productInBasket.ModifiedDate,
                            IsDeleted = productInBasket.IsDeleted
                        });
                    }
                    dto.BasketProducts = productsInUserBasket;
                    return new Response<BasketListDto>(ResponseType.Success, dto);
                }
                return new Response<BasketListDto>(ResponseType.Success, dto, "Sepette henüz ürün bulunmamaktadır!!!");
            }
            return new Response<BasketListDto>(ResponseType.NotFound, $"{UserId} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<BasketProductCreateDto>> CreateProductForBasket(BasketProductCreateDto dto)
        {
            var validationResult = _basketProductCreateDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.UserId == UserId);
                var control = await _uow.GetRepository<BasketProduct>().GetByFilterAsync(x => x.ProductId == dto.ProductId && x.BasketId == basket.Id);
                if (control == null)
                {
                    var entity = new BasketProduct
                    {
                        BasketId = GetByUserIdAsync().Result.Data.Id,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        TotalPrice = dto.Quantity * _productService.GetByIdAsync(dto.ProductId).Result.Data.Price,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    var createResult = await UpdateBasketSubTotal(entity.BasketId, entity.TotalPrice);
                    if (createResult.ResponseType == ResponseType.Success)
                    {
                        await _uow.GetRepository<BasketProduct>().CreateAsync(entity);
                        await _uow.SaveChangesAsync();
                        return new Response<BasketProductCreateDto>(ResponseType.Success, dto);
                    }
                    return new Response<BasketProductCreateDto>(ResponseType.ValidationError, "İşlem başarısız!!!");
                }
                var data = new BasketProduct
                {
                    Id = control.Id,
                    BasketId = control.BasketId,
                    ProductId = control.ProductId,
                    Quantity = control.Quantity + dto.Quantity,
                    TotalPrice = control.TotalPrice + (dto.Quantity * _productService.GetByIdAsync(dto.ProductId).Result.Data.Price),
                    CreatedDate = control.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = control.IsDeleted
                };
                var updateResult = await UpdateBasketSubTotal(data.BasketId, data.TotalPrice - control.TotalPrice);
                if (updateResult.ResponseType == ResponseType.Success)
                {
                    _uow.GetRepository<BasketProduct>().UpdateModified(data);
                    await _uow.SaveChangesAsync();
                    return new Response<BasketProductCreateDto>(ResponseType.Success, dto);
                }
                return new Response<BasketProductCreateDto>(ResponseType.ValidationError, "İşlem başarısız!!!");
            }
            return new Response<BasketProductCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        private async Task<IResponse> UpdateBasketSubTotal(int basketId, decimal totalPrice)
        {
            var basket = await _uow.GetRepository<Basket>().GetByFilterAsync(x => x.Id == basketId && !x.IsDeleted);
            var entity = new Basket
            {
                Id = basketId,
                UserId = basket.UserId,
                SubTotal = basket.SubTotal + totalPrice,
                CreatedDate = basket.CreatedDate,
                ModifiedDate = DateTime.Now,
                IsDeleted = basket.IsDeleted,
                BasketProducts = basket.BasketProducts
            };
            _uow.GetRepository<Basket>().UpdateModified(entity);
            return new Response(ResponseType.Success);
        }
    }
}
