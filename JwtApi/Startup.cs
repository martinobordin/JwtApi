using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JwtApi.Startup))]

namespace JwtApi
{
    using System.Configuration;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Owin.Security.Jwt;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = ConfigurationManager.AppSettings["JwtSymmetricSecurityKey"];
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var audience = ConfigurationManager.AppSettings["JwtAudience"];

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    }
                });
        }
    }
}
