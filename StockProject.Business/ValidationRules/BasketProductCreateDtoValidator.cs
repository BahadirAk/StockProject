using FluentValidation;
using StockProject.Dtos.BasketProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class BasketProductCreateDtoValidator : AbstractValidator<BasketProductCreateDto>
    {
        public BasketProductCreateDtoValidator()
        {
            this.RuleFor(x => x.ProductId).NotEmpty();
            this.RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
