﻿using StockProject.Dtos.ProductDtos;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Interfaces
{
    public interface IProductService : IService<ProductCreateDto, ProductUpdateDto, ProductListDto, Product>
    {
    }
}
