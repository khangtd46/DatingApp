using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;

namespace DatingApp.Implementations
{
	public class _UserRepository : IUserRepository
	{
		private readonly AppDBContext _dbContext;
        public _UserRepository(AppDBContext dbContext)
        {
			_dbContext = dbContext;
		}
        public SystemUser CreateUser(CreateSystemUserDTO user)
		{
			throw new NotImplementedException();
		}

		public SystemUser User(int id)
		{
			throw new NotImplementedException();
		}

		public List<SystemUser> Users()
		{
			throw new NotImplementedException();
		}
	}
}
