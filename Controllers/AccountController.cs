using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers
{
    public class AccountController : ApiController
    {
        private readonly AppDBContext _dbContext;
        private readonly ITokenService _tokenService;
        public AccountController(AppDBContext context,ITokenService tokenService)
        {
            _dbContext = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<SystemUserLoginOutputDTO>> Login(SystemUserLoginDTO User)
        {
            byte[] inputPassword;
            SystemUser findUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == User.UserName.ToLower());
            if (findUser == null)
            {
                return Unauthorized("Sai thông tin đăng nhập");
            }
            using (HMACSHA512 hmac = new HMACSHA512(findUser.PasswordSalt))
            {
                inputPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(User.Password));
            }
            bool loginResult = inputPassword.SequenceEqual(findUser.PasswordHash);
            if (loginResult)
            {
                return new SystemUserLoginOutputDTO()
                {
                    UserName = User.UserName,
                    Token = _tokenService.CreateToken(findUser)
                };
            }
            else
            {
                return Unauthorized("Sai thông tin đăng nhập");
            }
        }
        public async Task<bool> UserExist(string UserName)
        {
            SystemUser user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<SystemUserLoginOutputDTO>> CreateUser(RegisterDTO User)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(User.Password);
            byte[] PasswordHash;
            byte[] PasswordSalt;
            if (await UserExist(User.UserName)) return BadRequest("UserName Is Taken");
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                PasswordHash = hmac.ComputeHash(inputBytes);
                PasswordSalt = hmac.Key;
            }
            SystemUser user = new SystemUser()
            {
                UserName = User.UserName,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();


            return new SystemUserLoginOutputDTO()
            {
                UserName = User.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
