using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.UserRoleDtos
{
    public class UserRoleListDto : IDto
    {
        public int Id { get; set; }
        public int UserName { get; set; }
        public int RoleTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
