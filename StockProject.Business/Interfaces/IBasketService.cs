using StockProject.Common;
using StockProject.Dtos.BasketDtos;
using StockProject.Dtos.BasketProductDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Interfaces
{
    public interface IBasketService : IService<BasketCreateDto, BasketUpdateDto, BasketListDto, Basket>
    {
        Task<IResponse<BasketListDto>> GetByUserIdAsync();
        Task<IResponse<BasketProductCreateDto>> CreateProductForBasketAsync(BasketProductCreateDto dto);
        Task<IResponse<BasketProductUpdateDto>> UpdateProductFromBasketAsync(BasketProductUpdateDto dto);
        Task<IResponse> RemoveProductFromBasketAsync(int productId);
    }
}
