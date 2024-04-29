using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
	public class CreateSystemUserDTO
	{
		[Required] 
		public string UserName { get; set;}
		[Required] 
		public string Password { get; set;}

    }
}
