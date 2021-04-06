using SiwesApp.Dtos.All;
using SiwesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface IAuthenticationRepo
    {
        public Task<string> GenerateJwtToken(User user, string secretKey);
        //public Task<ToRespond> ResendUserEmailVerificationLink(EmailVerificationRequest emailVerificationRequest);
        public string GetUserEmailVerificationLink(string userToken);
        public Task<ToRespond> LoginUser(UserForLogin userLoginDetails, string secretKey);

    }
}
