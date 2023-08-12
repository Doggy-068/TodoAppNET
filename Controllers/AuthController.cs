using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Utils;

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
			if (authLoginByAccountAndPasswordDTO.Password == "123456")
			{
				if (authLoginByAccountAndPasswordDTO.Account == "root")
				{
					var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[] { new Claim(ClaimTypes.Role, Role.Root) }, signingCredentials: new SigningCredentials(algorithm: SecurityAlgorithms.HmacSha256, key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret))), expires: DateTime.Now.AddHours(1)));
					return new AuthLoginReturnDTO() { Token = token };
				}
				else if (authLoginByAccountAndPasswordDTO.Account == "admin")
				{
					var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[] { new Claim(ClaimTypes.Role, Role.Admin) }, signingCredentials: new SigningCredentials(algorithm: SecurityAlgorithms.HmacSha256, key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret))), expires: DateTime.Now.AddHours(1)));
					return new AuthLoginReturnDTO() { Token = token };
				}
				else if (authLoginByAccountAndPasswordDTO.Account == "user")
				{
					var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(claims: new[] { new Claim(ClaimTypes.Role, Role.User) }, signingCredentials: new SigningCredentials(algorithm: SecurityAlgorithms.HmacSha256, key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret))), expires: DateTime.Now.AddHours(1)));
					return new AuthLoginReturnDTO() { Token = token };
				}
				else
				{
					return NotFound();
				}
			}
			return NotFound();
		}
	}
}
