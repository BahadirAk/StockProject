using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Dtos.BasketProductDtos;

namespace StockProject.API.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [Authorize(Roles = "Member")]
        [HttpGet]
        [Route("getMyBasket")]
        public async Task<IActionResult> GetMyBasket()
        {
            var myBasket = await _basketService.GetByUserIdAsync();
            return Ok(myBasket);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("addProductToMyBasket")]
        public async Task<IActionResult> AddProductToMyBasket(BasketProductCreateDto dto)
        {
            var addProduct = await _basketService.CreateProductForBasketAsync(dto);
            return Created(string.Empty, addProduct);
        }
        [Authorize(Roles = "Member")]
        [HttpDelete]
        [Route("removeProductFromMyBasket")]
        public async Task<IActionResult> RemoveProductFromMyBasket(int productId)
        {
            var removeProduct = await _basketService.RemoveProductFromBasketAsync(productId);
            return NoContent();
        }
    }
}
