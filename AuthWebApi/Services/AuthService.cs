using AuthWebApi.Data;
using AuthWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly JWT _jwt;
        public AuthService( UserManager<MyUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;

            _jwt = jwt.Value;

        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {

           var user = await _userManager.FindByEmailAsync(model.Email);

            if( user is  null || ! await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthModel { IsAuthenticated = false, Message = "username or password is incorrect !!" };

            }


            List<string> roles = (List<string>) await _userManager.GetRolesAsync(user);

           

            var token = await CreateJwtTokenAsync(user);

            return new AuthModel
            {
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                Rolles = roles,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireOn =  token.ValidTo
            };
      
          
            

        }

        public async Task<AuthModel> RegisterAsync (RegisterModel model)
        {
            if( await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModel { Message = "email is already registerd"};
            }

            if( await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new AuthModel { Message = $"the username  ${model.UserName}  is already registered" };
            }

             
            MyUser user = new MyUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if( !result.Succeeded)
            {
                string errors = string.Empty;

                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                return new AuthModel { Message = errors};


            }



            await _userManager.AddToRoleAsync(user, "Default");



            var jwtSecurityToken =  await CreateJwtTokenAsync(user);

            var str = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new AuthModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Rolles =  (List<string>)  await _userManager.GetRolesAsync(user),
                IsAuthenticated = true,
                ExpireOn = jwtSecurityToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),


            };


            




        }





        private async Task<JwtSecurityToken> CreateJwtTokenAsync(MyUser user)
        {


            var userClaims = await _userManager.GetClaimsAsync(user) ;

            var roles = await _userManager.GetRolesAsync(user);


            List<Claim> roleClaims = new List<Claim>();

             
            foreach (string role in roles)
            {
                roleClaims.Add(new Claim("roles",role));
            }


            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid",user.Id)

            }.Union(userClaims).Union(roleClaims).ToList();




            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var sigingCredentials = new SigningCredentials(symetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationOnDays),
                signingCredentials: sigingCredentials);

            return jwtSecurityToken;







        }






    }
}
