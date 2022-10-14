using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities
{
    public class Order : BaseEntity
    {
        public int BasketId { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
