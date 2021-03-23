using Microsoft.AspNetCore.Identity;
using SiwesApp.Interfaces;
using SiwesApp.Models;
using SiwesApp.Utils;
using SiwesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SiwesApp.Repositories
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthenticationRepo(ApplicationDataContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<string> GenerateJwtToken(User user, string secretKey)
        {
            //Get User info
            var claims = new List<Claim>();


            if (user.UserType == Helpers.SiwesAdmin)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else if (user.UserType == Helpers.SiwesSupervisor)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString()),
                };
            }
            else if (user.UserType == Helpers.Lecturer)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else if (user.UserType == Helpers.IndustrialSupervisor)
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString())
                };
            }
            else
            {
                claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(Helpers.ClaimType_UserEmail, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserTypeId.ToString()),
                    new Claim(Helpers.ClaimType_UserType, user.UserType.ToString()),
                };
            }

            var roles = _userManager.GetRolesAsync(user).Result;

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetUserEmailVerificationLink(string userToken)
        {
            var originUrls = new StringValues();
            //CHECK LATER TO SEE IF ANY ORIGIN HEADER WILL BE SENT WITH THE REQUEST IF THE FRONTEND AND BACKEND ARE IN THE SAME DOMAIN...THAT IS IF THERE IS NO CORS
            var originHeadersGotten = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Origin", out originUrls);
            if (originHeadersGotten)
            {
                var originUrl = originUrls.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(originUrl))
                {
                    return null;
                }

                var convertedUserToken = Uri.EscapeDataString(userToken);
                convertedUserToken = convertedUserToken.Replace('%', '-');
                string emailVerificationLink = originUrl + "/" + _configuration.GetValue<string>("UserEmailConfirmationLink") + "/" + convertedUserToken;
                return emailVerificationLink;
            }

            return null;
        }
    }
}
