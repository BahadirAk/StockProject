using FluentValidation;
using StockProject.Dtos.UserRoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.ValidationRules
{
    public class UserRoleCreateDtoValidator : AbstractValidator<UserRoleCreateDto>
    {
        public UserRoleCreateDtoValidator()
        {
            this.RuleFor(x => x.UserId).NotEmpty();
            this.RuleFor(x => x.RoleId).NotEmpty();
            this.RuleFor(x => x.CreatedDate).NotEmpty();
            this.RuleFor(x => x.ModifiedDate).NotEmpty();
            this.RuleFor(x => x.IsDeleted).NotEmpty();
        }
    }
}
