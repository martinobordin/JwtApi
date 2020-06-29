

namespace JwtApiCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using JwtApiCore.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly AppSettings appSettings;

        public TokenController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }


        [HttpGet]
        [Route("GetToken")]
        public string GetToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.appSettings.Secret));
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
                this.appSettings.Issuer,
                this.appSettings.Audience,
                permClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        [HttpGet]
        [Route("ValidateToken")]
        public string ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
                                                {
                                                    ValidateIssuer = true,
                                                    ValidateAudience = true,
                                                    ValidateIssuerSigningKey = true,
                                                    ValidIssuer = this.appSettings.Issuer,
                                                    ValidAudience = this.appSettings.Audience,
                                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.appSettings.Secret)),
                                                    ValidateLifetime = false
                                                };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            return principal.Identity.Name;
        }
    }
}
