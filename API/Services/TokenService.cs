using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            //Symmetric key is a type of encryption where only one key is used to both encrypt
            //and decrypt electronic information. So the same key is used to both side JWT token.
            //And make sure the signature is verified.
            //Because this key doesn't need to leave the server.
            //It can remain on the server and doesn't need to go anywhere.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            //Creating the claims with user name. 
            //We're going to do everything using the user name that we store inside our JWT.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //Creating the credentials.
            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
            //Describing the token how it is going to look.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //howlong this token is going to be valid for
                SigningCredentials = creds
            };
            //Just creatng the handler to create the token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //returning the written token to whoever needs it.
            return tokenHandler.WriteToken(token);
        }
    }
}