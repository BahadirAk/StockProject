using StockProject.Dtos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Dtos.CategoryDtos
{
    public class CategoryCreateDto : IDto
    {
        public string Name { get; set; }
    }
}
