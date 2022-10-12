using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Business.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return int.Parse(httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
