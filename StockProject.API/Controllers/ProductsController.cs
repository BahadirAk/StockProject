using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Business.Services;
using StockProject.Dtos.CategoryDtos;
using StockProject.Dtos.ProductDtos;

namespace StockProject.API.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products.Data);
        }
        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("getActiveProducts")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await _productService.GetActivesAsync();
            return Ok(products.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("createProduct")]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            var product = await _productService.CreateAsync(dto);
            return Created(string.Empty, product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("updateProduct")]
        public async Task<IActionResult> Update(ProductUpdateDto dto)
        {
            var checkProduct = _productService.GetByIdAsync(dto.Id);
            if (checkProduct == null)
            {
                return NotFound(checkProduct.Id);
            }
            await _productService.UpdateAsync(dto);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("removeProduct")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkProduct = _productService.GetByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(checkProduct.Id);
            }
            await _productService.RemoveAsync(id);
            return NoContent();
        }
    }
}
