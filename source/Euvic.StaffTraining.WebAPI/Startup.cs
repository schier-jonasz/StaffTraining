using System.Security.Claims;
using System.Security.Principal;
using Euvic.Cqrs.PipelineBehaviors.Permissions;
using Euvic.StaffTraining.Identity;
using Euvic.StaffTraining.WebAPI.Auth;
using Euvic.StaffTraining.WebAPI.Extensions;
using Euvic.StaffTraining.WebAPI.Filters;
using Euvic.StaffTraining.WebAPI.Permission;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Euvic.StaffTraining.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddHttpContextAccessor();
            services.AddScoped<IPrincipal, ClaimsPrincipal>(x => x.GetRequiredService<IHttpContextAccessor>().HttpContext.User);
            services.AddStaffTraining(Configuration);
            services.AddIdentityModule(Configuration);

            services.AddControllers();
            services.AddSwagger(Configuration);

            services.AddMvcCore(config =>
            {
                config.Filters.Add<ExceptionFilter>();
                config.Filters.Add<ValidationExceptionFilter>();
                config.Filters.Add<InsufficientPermissionsExceptionFilter>();
            });

            services.AddCors();
            services.AddJwtAuthtentication(Configuration);

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireClaim(JwtClaimTypes.Scope, "staff-training-api").Build();
                options.AddPolicy(AuthorizationPolicies.HROnlyRestricted, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("euvic-roles", "HR");
                });

                options.AddPolicy(AuthorizationPolicies.LecturerOnlyRestricted, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("euvic-roles", "Lecturer");
                });
            });

            services.AddScoped<IUserPermissionsEvaluator, PrincipalPermissionsEvaluator>();
            services.AddCors(options =>
            {
                var allowedOrigins = Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId("staff-training-web");
                    c.OAuthAppName("Euvic - Identity Server");
                    c.OAuthUsePkce();
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Euvic.StaffTraining.WebAPI v1");
                });

            app.UseSerilogRequestLogging();
            app.MigrateStaffTraining();

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
