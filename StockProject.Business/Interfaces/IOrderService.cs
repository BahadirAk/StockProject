using StockProject.Common;
using StockProject.Dtos.OrderDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Interfaces
{
    public interface IOrderService : IService<OrderCreateDto, OrderUpdateDto, OrderListDto, Order>
    {
        Task<IResponse<List<OrderListDto>>> GetOrdersByUserIdAsync(int userId);
        Task<IResponse<List<OrderListDto>>> GetActiveOrdersByUserIdAsync(int userId);
        Task<IResponse<List<OrderListDto>>> GetOrdersByProductIdAsync(int productId);
        Task<IResponse<List<OrderListDto>>> GetActiveOrdersByProductIdAsync(int productId);
    }
}
