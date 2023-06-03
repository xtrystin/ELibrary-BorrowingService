using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ELibrary_BorrowingService.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                 .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                 {
                     jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Secrets:SecurityKey"))),
                         ValidateIssuer = false,
                         ValidateAudience = false,
                         ValidateLifetime = true,
                         ClockSkew = TimeSpan.FromMinutes(5)
                     };
                 });

            return services;
        }
    }
}
