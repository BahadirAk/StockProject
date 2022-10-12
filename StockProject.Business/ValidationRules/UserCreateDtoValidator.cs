using FluentValidation;
using StockProject.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            this.RuleFor(x => x.Firstname).NotEmpty();
            this.RuleFor(x => x.Surname).NotEmpty();
            this.RuleFor(x => x.Username).NotEmpty();
            this.RuleFor(x => x.Password).NotEmpty();
            this.RuleFor(x => x.Balance).NotEmpty();
        }
    }
}
