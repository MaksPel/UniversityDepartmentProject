using UniversityDepartment;
using UniversityDepartment.Data;
using UniversityDepartment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace UniversityDepartment.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Session.Keys.Contains("starting"))
            {
                var dbContext = context.RequestServices.GetRequiredService<UniversityDepartmentContext>();
                
                DbInitializer.Initialize(dbContext);

                context.Session.SetString("starting", "Yes");
            }

            await _next(context);
        }
    }
}
