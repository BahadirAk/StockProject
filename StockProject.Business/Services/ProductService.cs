using FluentValidation;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.CategoryDtos;
using StockProject.Dtos.ProductDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class ProductService : Service<ProductCreateDto, ProductUpdateDto, ProductListDto, Product>, IProductService
    {
        private readonly IUow _uow;
        private readonly IValidator<ProductCreateDto> _createDtoValidator;
        private readonly IValidator<ProductUpdateDto> _updateDtoValidator;

        public ProductService(IUow uow, IValidator<ProductCreateDto> createDtoValidator, IValidator<ProductUpdateDto> updateDtoValidator) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
        }
        public async Task<IResponse<ProductCreateDto>> CreateAsync(ProductCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var control = await _uow.GetRepository<Product>().GetByFilterAsync(x => x.Name == dto.Name);
                if (control == null)
                {
                    var entity = new Product
                    {
                        Name = dto.Name,
                        Quantity = dto.Quantity,
                        Price = dto.Price,
                        CategoryId = dto.CategoryId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    await _uow.GetRepository<Product>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                    return new Response<ProductCreateDto>(ResponseType.Success, dto);
                }
                List<CustomValidationError> errors = new List<CustomValidationError> { new CustomValidationError { ErrorMessage = "Böyle bir kategori zaten mevcut!!!", PropertyName = "" } };
                return new Response<ProductCreateDto>(dto, errors);
            }
            return new Response<ProductCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<IResponse<List<ProductListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<Product>().GetAllAsync();
            var dto = new List<ProductListDto>();
            foreach (var product in data)
            {
                dto.Add(new ProductListDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    CategoryName = _uow.GetRepository<Category>().GetByFilterAsync(x => x.Id == product.CategoryId).Result.Name,
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = product.ModifiedDate,
                    IsDeleted = product.IsDeleted
                });
            }
            return new Response<List<ProductListDto>>(ResponseType.Success, dto);
        }
        public async Task<IResponse<List<ProductListDto>>> GetActivesAsync()
        {
            var data = await _uow.GetRepository<Product>().GetAllAsync(x => !x.IsDeleted);
            var dto = new List<ProductListDto>();
            foreach (var product in data)
            {
                dto.Add(new ProductListDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    CategoryName = _uow.GetRepository<Category>().GetByFilterAsync(x => x.Id == product.CategoryId).Result.Name,
                    CreatedDate = product.CreatedDate,
                    ModifiedDate = product.ModifiedDate,
                    IsDeleted = product.IsDeleted
                });
            }
            return new Response<List<ProductListDto>>(ResponseType.Success, dto);
        }
        public async Task<IResponse<ProductListDto>> GetByIdAsync(int id)
        {
            var data = await _uow.GetRepository<Product>().GetByFilterAsync(x => x.Id == id);
            if (data != null)
            {
                ProductListDto dto = new ProductListDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    Quantity = data.Quantity,
                    Price = data.Price,
                    CategoryName = _uow.GetRepository<Category>().GetByFilterAsync(x => x.Id == data.CategoryId).Result.Name,
                    CreatedDate = data.CreatedDate,
                    ModifiedDate = data.ModifiedDate,
                    IsDeleted = data.IsDeleted
                };
                return new Response<ProductListDto>(ResponseType.Success, dto);
            }
            return new Response<ProductListDto>(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse> RemoveAsync(int id)
        {
            var unchangedEntity = await _uow.GetRepository<Product>().FindAsync(id);
            if (unchangedEntity != null)
            {
                var data = new Product
                {
                    Id = unchangedEntity.Id,
                    Name = unchangedEntity.Name,
                    Quantity = unchangedEntity.Quantity,
                    Price = unchangedEntity.Price,
                    CategoryId = unchangedEntity.CategoryId,
                    CreatedDate = unchangedEntity.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = true
                };
                _uow.GetRepository<Product>().Update(data, unchangedEntity);
                await _uow.SaveChangesAsync();
                return new Response(ResponseType.Success);
            }
            return new Response(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }
        public async Task<IResponse<ProductUpdateDto>> UpdateAsync(ProductUpdateDto dto)
        {
            var validationResult = _updateDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var unchangedEntity = await _uow.GetRepository<Product>().FindAsync(dto.Id);
                if (unchangedEntity != null)
                {
                    var entity = new Product
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        Quantity = dto.Quantity,
                        Price = dto.Price,
                        CategoryId = dto.CategoryId,
                        CreatedDate = unchangedEntity.CreatedDate,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = unchangedEntity.IsDeleted
                    };
                    _uow.GetRepository<Product>().Update(entity, unchangedEntity);
                    await _uow.SaveChangesAsync();
                    return new Response<ProductUpdateDto>(ResponseType.Success, dto);
                }
                return new Response<ProductUpdateDto>(ResponseType.NotFound, $"{dto.Id} sine sahip data bulunamadı!!!");
            }
            return new Response<ProductUpdateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
    }
}
