using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class AuthController : ControllerBase
	{
		/// <summary>
		/// Login by account and password.
		/// </summary>
		/// <returns></returns>
		[HttpPost("Login")]
		public ActionResult<AuthLoginReturnDTO> Login(AuthLoginByAccountAndPasswordDTO authLoginByAccountAndPasswordDTO)
		{
			if (authLoginByAccountAndPasswordDTO.Account == "admin" && authLoginByAccountAndPasswordDTO.Password == "123456")
			{
				var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[] { new Claim(ClaimTypes.Authentication, "auth") }, signingCredentials: new SigningCredentials(algorithm: SecurityAlgorithms.HmacSha256, key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeySecretKeySecretKey"))), expires: DateTime.Now.AddHours(1)));
				return new AuthLoginReturnDTO() { Token = token };
			}
			return NotFound();
		}
	}
}
