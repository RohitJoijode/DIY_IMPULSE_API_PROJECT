using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using DIY_IMPULSE_API_PROJECT.DAL;
using DIY_IMPULSE_API_PROJECT.DBENGINE;
using DIY_IMPULSE_API_PROJECT.MODEL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class AuthServices : IAuthServices 
    {
        private readonly IConfiguration _config;
        private readonly DBEngine _DbEngine;

        public AuthServices(IConfiguration config, DBEngine DbEngine)
        {
            _config = config;
            _DbEngine = DbEngine;
        }

       



        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateTokenString(Tbl_Users UsersObj)
        {
            var claims = new[]
            {
                new Claim("UserId",Convert.ToString(UsersObj.UserId)),
                new Claim("UserName",UsersObj.UserName ?? ""),
                new Claim("UserRole",UsersObj.UserRole ?? ""),
                new Claim("UserEmail",UsersObj.UserEmail ?? ""),
                new Claim("UserMobile",UsersObj.UserMobile ?? ""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("082F41538C1178DE768A9AC86291678D"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse("2")),
                signingCredentials: creds
          );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
        {
            Tbl_Users Tbl_Users1Obj = new Tbl_Users();
            DAL.Tbl_RefreshToken Tbl_RefreshToken = null;
            string userId = "", userName = "", UserRole = "", UserEmail = "", UserMobile = "";
            var principal = GetTokenPrincipal(model.JwtToken);
            //principal.Identity.Name
            var response = new LoginResponse();
            if (principal?.Identity?.IsAuthenticated == false)
                return response;


            if (principal?.Identity is ClaimsIdentity claimsIdentity)
            {
                // Now you can access the claims
                var UserIdClaim = claimsIdentity.FindFirst("UserId");
                var UserNameClaim = claimsIdentity.FindFirst("UserName");
                var UserRoleClaim = claimsIdentity.FindFirst("UserRole");
                var UserEmailClaim = claimsIdentity.FindFirst("UserEmail");
                var UserMobileClaim = claimsIdentity.FindFirst("UserMobile");

                // Get the value of a specific claim
                userId = UserIdClaim?.Value;
                userName = UserNameClaim?.Value;
                UserRole = UserRoleClaim?.Value;
                UserEmail = UserEmailClaim?.Value;
                UserMobile = UserMobileClaim?.Value;


                Tbl_Users1Obj.UserId = Convert.ToInt32(userId);
                Tbl_Users1Obj.UserName = userName;
                Tbl_Users1Obj.UserRole = UserRole;
                Tbl_Users1Obj.UserEmail = UserEmail;
                Tbl_Users1Obj.UserMobile = UserMobile;
            }

            var FoundRefreshToken = _DbEngine.Tbl_RefreshToken.Where(
                                                                          x => x.UserId == userId
                                                                          ).FirstOrDefault();

            if (principal?.Identity?.IsAuthenticated == false || FoundRefreshToken?.RefreshToken != model.RefreshToken || FoundRefreshToken?.RefreshTokenExpiry < DateTime.Now)
                return response;
            //if (identityUser is null || identityUser.RefreshToken != model.RefreshToken || identityUser.RefreshTokenExpiry > DateTime.Now)
            //return response;

            response.IsLogedIn = true;
            response.JwtToken = this.GenerateTokenString(Tbl_Users1Obj); //temporyt commented
            response.RefreshToken = this.GenerateRefreshTokenString();

            using (var transaction = _DbEngine.Database.BeginTransaction())
            {
                if (FoundRefreshToken == null)
                {
                    Tbl_RefreshToken = new DAL.Tbl_RefreshToken();
                    var maxRefreshTokenId = await _DbEngine.Tbl_RefreshToken.MaxAsync(u => (int?)u.Id) ?? 0;
                    Tbl_RefreshToken.Id = maxRefreshTokenId + 1;
                    Tbl_RefreshToken.Token = response.JwtToken;
                    Tbl_RefreshToken.UserId = userId;
                    Tbl_RefreshToken.RefreshToken = response.RefreshToken;
                    //Tbl_RefreshToken.RefreshTokenExpiry = DateTime.Now.AddHours(2);
                    Tbl_RefreshToken.RefreshTokenExpiry = DateTime.Now.AddMinutes(5);
                    Tbl_RefreshToken.IsActive = true;
                    _DbEngine.Tbl_RefreshToken.Add(Tbl_RefreshToken);
                    _DbEngine.SaveChanges();
                    transaction.Commit();
                }
                else if (FoundRefreshToken != null)
                {
                    Tbl_RefreshToken = new DAL.Tbl_RefreshToken();
                    Tbl_RefreshToken = FoundRefreshToken;
                    Tbl_RefreshToken.Token = response.JwtToken;
                    Tbl_RefreshToken.RefreshToken = response.RefreshToken;
                    //Tbl_RefreshToken.RefreshTokenExpiry = DateTime.Now.AddHours(2);
                    Tbl_RefreshToken.RefreshTokenExpiry = DateTime.Now.AddMinutes(5);
                    Tbl_RefreshToken.ModifyOn = DateTime.Now;
                    Tbl_RefreshToken.IsActive = true;
                    _DbEngine.Entry(Tbl_RefreshToken).State = EntityState.Modified;
                    _DbEngine.SaveChanges();
                    transaction.Commit();
                }
            }

            return response;
        }

        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("082F41538C1178DE768A9AC86291678D"));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securitykey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}
