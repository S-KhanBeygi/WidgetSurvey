using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DaraSurvey.Core
{
    public static class SwaggerConfiguration
    {
        public static void JwtConfig(this IServiceCollection services, AppSettings appSettings)
        {
            services
             .AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuerSigningKey = false, // athurize checked by token
                     ValidateAudience = false, // check validate token client side
                     ValidateLifetime = true, // token has expire date 
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.IdentitySettings.Jwt.SecurityKey)), // set a key for generate token
                     ValidateIssuer = true, // check token validation server side
                     ValidIssuer = appSettings.Hosts.Api, // server domain url
                     RequireExpirationTime = false,

                 };
             });
        }

        // --------------------

        public static void SwaggerConfig(this IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            // ------------------

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "Some Descrription Here ....",
                    Title = "DaraSurvey",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "Sohail.ahmadkhanbeygi@gmail.com",
                        Name = "name",
                    }
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new List<string>()
                        }
                    });
            });
        }

        // --------------------

        public static void UseSwagerDocumentation(this IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DaraSurveyV1");
            });
        }
    }
}
