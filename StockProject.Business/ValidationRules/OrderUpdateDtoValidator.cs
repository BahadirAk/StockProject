using FluentValidation;
using StockProject.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidator()
        {
            this.RuleFor(x => x.Id);
            this.RuleFor(x => x.UserId).NotEmpty();
            this.RuleFor(x => x.ProductId).NotEmpty();
            this.RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
