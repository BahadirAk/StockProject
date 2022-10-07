using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        //NavigationProperties
        public List<UserRole> UserRoles { get; set; }
        public List<Order> Orders { get; set; }
    }
}
