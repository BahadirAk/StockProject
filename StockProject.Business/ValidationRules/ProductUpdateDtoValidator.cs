using FluentValidation;
using StockProject.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();
            this.RuleFor(x => x.Quantity).NotEmpty();
            this.RuleFor(x => x.Price).NotEmpty();
            this.RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
