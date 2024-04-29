using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
	public interface IUserRepository
	{
		List<SystemUser> Users();
		SystemUser User(int id);
		SystemUser CreateUser(CreateSystemUserDTO user);
	}
}
