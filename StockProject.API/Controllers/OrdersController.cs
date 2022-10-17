using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StockProject.Business.Interfaces;
using StockProject.Business.Services;
using StockProject.Dtos.OrderDtos;
using StockProject.Dtos.ProductDtos;
using System.Security.Claims;

namespace StockProject.API.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders.Data);
        }
        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //[Route("getActiveOrders")]
        //public async Task<IActionResult> GetActiveOrders()
        //{
        //    var orders = await _orderService.GetActivesAsync();
        //    return Ok(orders.Data);
        //}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getOrdersByUserId")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders.Data);
        }
        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //[Route("getActiveOrdersByUserId")]
        //public async Task<IActionResult> GetActiveOrdersByUserId(int userId)
        //{
        //    var orders = await _orderService.GetActiveOrdersByUserIdAsync(userId);
        //    return Ok(orders.Data);
        //}
        [Authorize(Roles = "Member")]
        [HttpPost]
        [Route("createOrder")]
        public async Task<IActionResult> Create()
        {
            var order = await _orderService.CreateAsync();
            return Created(string.Empty, order);
        }
        //[Authorize(Roles = "Member")]
        //[HttpPost]
        //[Route("updateOrder")]
        //public async Task<IActionResult> Update(OrderUpdateDto dto)
        //{
        //    var checkOrder = await _orderService.GetByIdAsync(dto.Id);
        //    if (checkOrder == null)
        //    {
        //        return NotFound(checkOrder.Data.Id);
        //    }
        //    await _orderService.UpdateAsync(dto);
        //    return NoContent();
        //}
        //[Authorize(Roles = "Member")]
        //[HttpDelete]
        //[Route("removeOrder")]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var checkOrder = await _orderService.GetByIdAsync(id);
        //    if (checkOrder == null)
        //    {
        //        return NotFound(checkOrder.Data.Id);
        //    }
        //    await _orderService.RemoveAsync(id);
        //    return NoContent();
        //}
        [Authorize(Roles = "Member")]
        [HttpGet]
        [Route("getMyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var myOrders = await _orderService.GetOrdersByAuthorizedUserIdAsync();
            return Ok(myOrders);
        }
    }
}
