using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
	public class Auth
	{
		public required long Id { get; set; }
		public required string Account { get; set; }
		public required string Email { get; set; }
		public required string Phone { get; set; }
		public required string Password { get; set; }
	}

	public class AuthLoginReturnDTO
	{
		[Required]
		public required string Token { get; set; }
	}

	public class AuthLoginByAccountAndPasswordDTO
	{
		[Required]
		public required string Account { get; set; }
		[Required]
		public required string Password { get; set; }
	}
}
