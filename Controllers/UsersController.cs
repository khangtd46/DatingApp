using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace DatingApp.Controllers
{
	[Authorize]
	public class UsersController : ApiController
	{
		private readonly AppDBContext _dbContext;
		private readonly ITokenService _tokenService;
		public UsersController(AppDBContext dbContext, ITokenService tokenService)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SystemUser>>> GetUsers()
		{
			return await _dbContext.Users.ToListAsync();
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<SystemUserOutputDTO>> GetUsers(int id)
		{
			SystemUser user = await _dbContext.Users.FindAsync(id);
			return new SystemUserOutputDTO()
			{
				Id = user.Id,
				UserName = user.UserName,
			};
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<ActionResult<string>> CreateUser(CreateSystemUserDTO User)
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


			return user.UserName;
		}
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<ActionResult<SystemUserLoginOutputDTO>> Login(SystemUserLoginDTO User)
		{
			byte[] inputPassword;
			SystemUser findUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == User.UserName.ToLower());
			if (findUser == null)
			{
				return NotFound();
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
				return BadRequest("Sai thông tin đăng nhập");
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
				return false;
			}

		}
	}
}
