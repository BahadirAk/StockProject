using FluentValidation;
using StockProject.Dtos.BasketProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class BasketProductUpdateDtoValidator : AbstractValidator<BasketProductUpdateDto>
    {
        public BasketProductUpdateDtoValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.BasketId).NotEmpty();
            this.RuleFor(x => x.ProductId).NotEmpty();
            this.RuleFor(x => x.Quantity).NotEmpty();
            this.RuleFor(x => x.TotalPrice).NotEmpty();
        }
    }
}
