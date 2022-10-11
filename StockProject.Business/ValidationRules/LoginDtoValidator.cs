using FluentValidation;
using StockProject.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            this.RuleFor(x => x.Username).NotEmpty();
            this.RuleFor(x => x.Password).NotEmpty();
        }
    }
}
