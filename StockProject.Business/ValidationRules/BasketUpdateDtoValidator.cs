using FluentValidation;
using StockProject.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class BasketUpdateDtoValidator : AbstractValidator<BasketUpdateDto>
    {
        public BasketUpdateDtoValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.UserId).NotEmpty();
            this.RuleFor(x => x.SubTotal).NotEmpty();
        }
    }
}
