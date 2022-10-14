using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.OrderDtos
{
    public class OrderCreateDto : IDto
    {
        public int BasketId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
