namespace JwtApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Web.Http;

    using Microsoft.IdentityModel.Tokens;

    public class TokenController : ApiController
    {
        private string key = ConfigurationManager.AppSettings["JwtSymmetricSecurityKey"];

        private string issuer = ConfigurationManager.AppSettings["JwtIssuer"];

        private string audience = ConfigurationManager.AppSettings["JwtAudience"];

        [HttpGet]
        public string GetToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
                                 {
                                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                     new Claim(ClaimTypes.Name, username),
                                     new Claim(ClaimTypes.Role, "administrator")
                                 };

            // Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(
                this.issuer,
                this.audience,  
                permClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        [HttpGet]
        public string ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
                                                {
                                                    ValidateIssuer = true,
                                                    ValidateAudience = true,
                                                    ValidateIssuerSigningKey = true,
                                                    ValidIssuer = this.issuer,
                                                    ValidAudience = this.audience,
                                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key)),
                                                    ValidateLifetime = false
                                                };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            return principal.Identity.Name;
        }
    }
}