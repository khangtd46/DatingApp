using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    [AllowAnonymous]
	public class UsersController : ApiController
	{
		private readonly IUserRepository _userRepository;
		public UsersController(IUserRepository userRepository)
		{
            _userRepository = userRepository;
		}
		[HttpGet]
		public async Task<ActionResult<List<SystemUser>>> GetUsers()
		{
			return Ok(await _userRepository.GetUsersAsync());
        }
		[HttpGet("{id}")]
		public async Task<ActionResult<SystemUserOutputDTO>> GetUsers(int id)
		{
			var user = await _userRepository.GetUserByIdAsync(id);
			return new SystemUserOutputDTO()
			{
				Id = user.Id,
				UserName = user.UserName,
			};

        }

	
		
	}
}
