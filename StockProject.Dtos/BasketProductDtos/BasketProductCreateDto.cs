using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.BasketProductDtos
{
    public class BasketProductCreateDto : IDto
    {
        //public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        //public decimal TotalPrice { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime ModifiedDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
