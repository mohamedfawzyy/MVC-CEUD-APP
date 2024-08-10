using Microsoft.Extensions.DependencyInjection;
using MVC.BLL;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.PL.Helpers;
using MVC.PL.Services.SendEmailService;

namespace MVC.PL.Extentisions
{
    public static class AppExtentions
    {
        public static void ApplyAppServices(this IServiceCollection services) {

            //---------------------------Dependancy injection for Repos--------------------//
            //allow dependancy injection for IDepartment Repo
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //allow dependancy injection for Employee Repo
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //---------------------Dependancy injection For IunitOfWork------------------------------//

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //------------------------------------------------dependancy injection for Imapper
            //allow dependancy injection for IMapper
            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            //allow dependancy injection for IsenderEmail Service
            services.AddTransient<ISenderEmail,SendEmail>();

        }
    }
}
