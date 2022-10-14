using StockProject.Dtos.BasketDtos;
using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.OrderDtos
{
    public class OrderListDto : IDto
    {
        public int Id { get; set; }
        public BasketListDto Basket { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
