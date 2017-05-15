using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel.Client;

namespace fletnix.Helpers
{
    public static class Token
    {
        public class UserModel
        {
            public string Name { get; set; }
            public string Role { get; set; }
            public string Email { get; set; }
        }

        public static UserModel User(IEnumerable<Claim> userClaims)
        {
            var name = userClaims.Where(c => c.Type == "name")
                .Select(c => c.Value).FirstOrDefault();

            var role = userClaims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).FirstOrDefault();

            var email = userClaims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).FirstOrDefault();

            return new UserModel()
            {
                Email = email,
                Name = name,
                Role = role
            };
        }
    }
}