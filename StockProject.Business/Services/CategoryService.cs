using FluentValidation;
using StockProject.Business.Extensions;
using StockProject.Business.Interfaces;
using StockProject.Common;
using StockProject.DataAccess.UnitOfWork;
using StockProject.Dtos.CategoryDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Services
{
    public class CategoryService : Service<CategoryCreateDto, CategoryUpdateDto, CategoryListDto, Category>, ICategoryService
    {
        private readonly IUow _uow;
        private readonly IValidator<CategoryCreateDto> _createDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _updateDtoValidator;

        public CategoryService(IUow uow, IValidator<CategoryCreateDto> createDtoValidator, IValidator<CategoryUpdateDto> updateDtoValidator) : base(createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _createDtoValidator = createDtoValidator;
            _updateDtoValidator = updateDtoValidator;
        }

        public async Task<IResponse<CategoryCreateDto>> CreateAsync(CategoryCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var control = await _uow.GetRepository<Category>().GetByFilterAsync(x => x.Name == dto.Name);
                if (control == null)
                {
                    var entity = new Category
                    {
                        Name = dto.Name,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    await _uow.GetRepository<Category>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                    return new Response<CategoryCreateDto>(ResponseType.Success, dto);
                }
                List<CustomValidationError> errors = new List<CustomValidationError> { new CustomValidationError { ErrorMessage = "Böyle bir kategori zaten mevcut!!!", PropertyName = "" } };
                return new Response<CategoryCreateDto>(dto, errors);
            }
            return new Response<CategoryCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }

        public async Task<IResponse<List<CategoryListDto>>> GetAllAsync()
        {
            var data = await _uow.GetRepository<Category>().GetAllAsync();
            var dto = new List<CategoryListDto>();
            foreach (var category in data)
            {
                dto.Add(new CategoryListDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedDate = category.CreatedDate,
                    ModifiedDate = category.ModifiedDate,
                    IsDeleted = category.IsDeleted
                });
            }
            return new Response<List<CategoryListDto>>(ResponseType.Success, dto);
        }
        public async Task<IResponse<List<CategoryListDto>>> GetActivesAsync()
        {
            var data = await _uow.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);
            var dto = new List<CategoryListDto>();
            foreach (var category in data)
            {
                dto.Add(new CategoryListDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedDate = category.CreatedDate,
                    ModifiedDate = category.ModifiedDate,
                    IsDeleted = category.IsDeleted
                });
            }
            return new Response<List<CategoryListDto>>(ResponseType.Success, dto);
        }

        public async Task<IResponse<CategoryListDto>> GetByIdAsync(int id)
        {
            var data = await _uow.GetRepository<Category>().GetByFilterAsync(x => x.Id == id);
            if (data != null)
            {
                CategoryListDto dto = new CategoryListDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedDate = data.CreatedDate,
                    ModifiedDate = data.ModifiedDate,
                    IsDeleted = data.IsDeleted
                };
                return new Response<CategoryListDto>(ResponseType.Success, dto);
            }
            return new Response<CategoryListDto>(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var unchangedEntity = await _uow.GetRepository<Category>().FindAsync(id);
            if (unchangedEntity != null)
            {
                var data = new Category
                {
                    Id = unchangedEntity.Id,
                    Name = unchangedEntity.Name,
                    CreatedDate = unchangedEntity.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = true
                };
                _uow.GetRepository<Category>().Update(data, unchangedEntity);
                await _uow.SaveChangesAsync();
                return new Response(ResponseType.Success);
            }
            return new Response(ResponseType.NotFound, $"{id} sine sahip data bulunamadı!!!");
        }

        public async Task<IResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto)
        {
            var validationResult = _updateDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var unchangedEntity = await _uow.GetRepository<Category>().FindAsync(dto.Id);
                if (unchangedEntity != null)
                {
                    var entity = new Category
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        CreatedDate = unchangedEntity.CreatedDate,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = unchangedEntity.IsDeleted
                    };
                    _uow.GetRepository<Category>().Update(entity, unchangedEntity);
                    await _uow.SaveChangesAsync();
                    return new Response<CategoryUpdateDto>(ResponseType.Success, dto);
                }
                return new Response<CategoryUpdateDto>(ResponseType.NotFound, $"{dto.Id} sine sahip data bulunamadı!!!");
            }
            return new Response<CategoryUpdateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
    }
}
