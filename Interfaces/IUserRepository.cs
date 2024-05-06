using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<SystemUser>> GetUsersAsync();
        Task<SystemUser> GetUserByIdAsync(int id);
        Task<SystemUser> GetUserByUserNameAsync(string userName);
		void Update(SystemUser user);
		Task<bool> SaveAllAsync();
	}
}
