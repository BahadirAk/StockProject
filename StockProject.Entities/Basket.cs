using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities
{
    public class Basket : BaseEntity
    {
        public int UserId { get; set; }
        public decimal SubTotal { get; set; }

        //Navigation Properties
        public List<BasketProduct> BasketProducts { get; set; }
    }
}
