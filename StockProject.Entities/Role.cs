using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Entities
{
    public class Role : BaseEntity
    {
        public string Definition { get; set; }

        //NavigationProperties
        public List<UserRole> UserRoles { get; set; }
    }
}
