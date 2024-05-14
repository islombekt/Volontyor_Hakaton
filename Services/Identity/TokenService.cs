using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Volontyor_Hakaton.Data;
using Volontyor_Hakaton.DTOs.Identity;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Services.Identity
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TokenService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<TokenResponse> LoginAsync(LoginRequest model)
        {
            TokenResponse rep = new()
            {
                Token = "401"
            };

            var user = await FindUser(model.Login);
            if (user is null)
            {
                return rep;
            }

            if (VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                
                rep.RefreshToken = "refresh token";
                rep.Token = CreateToken(user);
               
                return rep;
            }
            
            return rep;
        }

        public async Task<string> RegisterAsync(RegisterUser user)
        {
            var userList = await _context.Users.Select(o => o.UserName).FirstOrDefaultAsync(c => c == user.UserName);

            if (userList != null)
            {
                return "Existing user";

            }
            try
            {
                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var newUser = new Users
                {
                    UserName = user.UserName,
                    UserRole = user.UserRole,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    PhoneNumber = user.PhoneNumber,
                    FIO = user.FIO,
                  

                };
                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return "Successfully created";
            }
            catch (Exception ex)
            {
                return $"Could not create user, ex: {ex.Message}";
            }
        }
        private async Task<Users?> FindUser(string login)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(d => d.UserName.Equals(login));
            }
            catch { return null; }
        }
        private string CreateToken(Users user)
        {


            List<Claim> claims = new List<Claim>
            {

                 new Claim("id",user.UserId.ToString()),
                 new Claim("Email", user.UserName),
                new Claim("Role", user.UserRole),
                 new Claim("FIO", user.FIO),



            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppConfiguration:Secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
       
    }
}
