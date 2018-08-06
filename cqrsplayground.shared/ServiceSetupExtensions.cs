using cqrsplayground.authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public static class ServiceSetupExtensions
    {
        public static IServiceCollection AddHttpClientFor<IService>(this IServiceCollection services, String endpoint)
        {
            var client = RestService.For<ITradeService>(endpoint);
            services.AddSingleton(client);
            return services;
        }

        public static IServiceCollection AddAuthenticatedHttpClientFor<IService>(this IServiceCollection services, String targetServiceKey, String endpoint)
            where IService: class
        {
            var provider = new AuthenticatedClientProvider();
            var client = provider.GetClientFor<IService>(targetServiceKey, ServiceConstants.TradeServiceUrl);
            services.AddSingleton(client);
            return services;
        }

        public static IServiceCollection AddAuthenticationWorkflow(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            var serviceInstance = new ServiceInstance()
            {
                Key = configuration["identity:key"],
                Token = configuration["identity:token"],
                Id = Guid.NewGuid().ToString(),
                ServiceKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["identity:key"]))
            };

            services.AddSingleton<IServiceInstance>(serviceInstance);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = serviceInstance.Issuer,
                        IssuerSigningKey = serviceInstance.ServiceKey,
                    };

                });
            

            return services;
        }


    }
}
