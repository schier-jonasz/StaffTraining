using System.Reflection;
using Euvic.Cqrs.PipelineBehaviors.Permissions;
using Euvic.Cqrs.PipelineBehaviors.Validation;
using Euvic.StaffTraining.Common;
using Euvic.StaffTraining.Infrastructure.EntityFramework;
using Euvic.StaffTraining.Infrastructure.Metrics;
using Euvic.StaffTraining.Infrastructure.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Euvic.StaffTraining
{
    public static class ModuleExtensions
    {
        public static void AddStaffTraining(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StaffTrainingContext>(
                          options => options.UseSqlServer(configuration.GetConnectionString("Sql"),
                          config => config.MigrationsHistoryTable(HistoryRepository.DefaultTableName, StaffTrainingContext.Schema)));

            services.AddDbContext<StaffTrainingReadonlyContext>(
               options => options.UseSqlServer(configuration.GetConnectionString("Sql"),
               config => config.MigrationsHistoryTable(HistoryRepository.DefaultTableName, StaffTrainingReadonlyContext.Schema)));

            services.AddMediatR(typeof(ModuleExtensions));
            services.AddValidatorsFromAssemblyContaining(typeof(ModuleExtensions)); // Rejestracja validatorów

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMeasurementBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.Scan(scan => scan // Rejestracja async validatorów
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(x => x.AssignableTo(typeof(IAsyncValidator<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(scan => scan // Rejestracja async validatorów
           .FromAssemblies(Assembly.GetExecutingAssembly())
           .AddClasses(x => x.AssignableTo(typeof(IPermission<>)))
                   .AsImplementedInterfaces()
                   .WithScopedLifetime());
        }

        public static void MigrateStaffTraining(this IApplicationBuilder app)
        {
            app.Migrate<StaffTrainingContext>();
            app.Migrate<StaffTrainingReadonlyContext>();
        }
    }
}
