using AspNetCoreWithDocker.Business.BusinessImplementation.People;
using AspNetCoreWithDocker.Business.BusinessImplementation.User;
using AspNetCoreWithDocker.Business.Rules.People;
using AspNetCoreWithDocker.Business.Rules.User;
using AspNetCoreWithDocker.HyperMedia;
using AspNetCoreWithDocker.Models.Contexts;
using AspNetCoreWithDocker.Repositories.Generic.Contract;
using AspNetCoreWithDocker.Repositories.Generic.Implementation;
using AspNetCoreWithDocker.Repositories.RepositoriesImplementation.People;
using AspNetCoreWithDocker.Repositories.RepositoriesImplementation.User;
using AspNetCoreWithDocker.Repositories.Repository.People;
using AspNetCoreWithDocker.Repositories.Repository.User;
using AspNetCoreWithDocker.Security.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using Tapioca.HATEOAS;

namespace AspNetCoreWithDocker
{
    public class Startup
    {
        public IConfiguration _Configuration { get; }
        public IHostingEnvironment _Enviroment { get; set; }

        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, IHostingEnvironment enviroment, ILogger<Startup> logger)
        {
            _Configuration = configuration;
            _Enviroment = enviroment;
            _logger = logger;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Connection wiht dataBase
            var connection = _Configuration["MySqlConnection:MySqlConnectionString"];
            services.AddDbContext<MySqlContext>(options => options.UseMySql(connection));


            // Adding Migration
            if (_Enviroment.IsDevelopment())
            {
                try
                {
                    var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    var evolve = new Evolve.Evolve("evolve.json", evolveConnection, msg => _logger.LogInformation(msg))
                    {
                        Locations = new List<string> { "DataBase/Migrations" },
                        IsEraseDisabled = true
                    };
                    evolve.Migrate();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Database migration failed.", ex);
                    throw;
                }
            }

            //Adding Authentication with JWT
            var signingConfigurations = new SigningConfiguration();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                _Configuration.GetSection("TokenConfigurations")
            )
            .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Validates the signing of a received token
                paramsValidation.ValidateIssuerSigningKey = true;

                // Checks if a received token is still valid
                paramsValidation.ValidateLifetime = true;

                // Tolerance time for the expiration of a token (used in case
                // of time synchronization problems between different
                // computers involved in the communication process)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Enables the use of the token as a means of
            // authorizing access to this project's resources
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            // Adding ContentNegociation for export data in other formats, like XML, CSV, PDF
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("application/json", MediaTypeHeaderValue.Parse("application/json"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("text/xml"));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            })
            .AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Adding versioning of API
            services.AddApiVersioning(option => option.ReportApiVersions = true);

            // Adding Swagger and configuring the generate documentation 
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Title = "RESTfull AspNetCore 2.0 with Docker",
                    Version = "v1",
                    Description = "A sample API to demo aspnet with docker",
                    TermsOfService = "Free bitch, rsrs",
                    Contact = new Contact
                    {
                        Name = "Andre Skrepchuk Caldeira",
                        Email = "andre.scaldeira@outlook.com"
                    }
                });
                config.EnableAnnotations();
            });



            // Adding Tapioca.HATEOAS
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ObjectContentResponseEnricherList.Add(new PersonEnricher());
            services.AddSingleton(filterOptions);

            // Adding injection dependency of Business layer 
            services.AddScoped<IPersonBusiness, PersonBusinessImpl>();
            services.AddScoped<IUserBusiness, UserBusinessImpl>();

            // Adding Generic Repository injection dependency and other repositories to
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryImpl<>));
            services.AddScoped<IUserRepository, UserRepositoryImpl>();
            services.AddScoped<IPersonRepository, PersonRepositoryImpl>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configuring logg 
            loggerFactory.AddConsole(_Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Enable and configure Swagger UI to better human-friendly experience
            app.UseSwagger();
            app.UseSwaggerUI(config => {
                config.RoutePrefix = "swagger";
                config.DocumentTitle = "RESTfull AspNetCore 2.0 with Docker";
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            // Supports to development stage
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // An configuration to tapioca.HATEOAS mount your routes and to work correctly
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "DefaultApi",
                    template: "{controller=Values}/{id?}");
            });
        }
    }
}
