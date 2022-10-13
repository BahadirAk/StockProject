using StockProject.Dtos.BasketProductDtos;
using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.BasketDtos
{
    public class BasketListDto : IDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public List<BasketProductListDto> BasketProducts { get; set; }
    }
}
