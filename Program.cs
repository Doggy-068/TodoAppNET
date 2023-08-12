using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApp.Models;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(options =>
{
	options.UseInMemoryDatabase("TodoList");
});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Permission.Root, policy => policy.RequireRole(Role.Root));
	options.AddPolicy(Permission.Admin, policy => policy.RequireRole(Role.Root, Role.Admin));
	options.AddPolicy(Permission.User, policy => policy.RequireRole(Role.Root, Role.Admin, Role.User));
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtSecret))
	};
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityRequirement(
		new OpenApiSecurityRequirement()
		{
			{
				new OpenApiSecurityScheme()
				{
					Reference = new OpenApiReference()
					{
						Id = "Bearer",
						Type = ReferenceType.SecurityScheme
					}
				},
				new List<string>()
			}
		});
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Description = "Bearer {token}",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey
	});
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
