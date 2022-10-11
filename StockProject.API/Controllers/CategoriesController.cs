using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Dtos.CategoryDtos;

namespace StockProject.API.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories.Data);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("getActiveCategories")]
        public async Task<IActionResult> GetActiveCategories()
        {
            var categories = await _categoryService.GetActivesAsync();
            return Ok(categories.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("createCategory")]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);
            return Created(string.Empty, category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("updateCategory")]
        public async Task<IActionResult> Update(CategoryUpdateDto dto)
        {
            var checkCategory = _categoryService.GetByIdAsync(dto.Id);
            if (checkCategory == null)
            {
                return NotFound(checkCategory.Id);
            }
            await _categoryService.UpdateAsync(dto);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("removeCategory")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkCategory = _categoryService.GetByIdAsync(id);
            if (checkCategory == null)
            {
                return NotFound(checkCategory.Id);
            }
            await _categoryService.RemoveAsync(id);
            return NoContent();
        }
    }
}
