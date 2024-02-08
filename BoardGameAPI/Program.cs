using System.Text;
using BoardGameAPI.Data;
using BoardGameModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
	.AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<BoardGameDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("BoardGameDB")));

var jwtSection = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JWTSettings>(jwtSection);

//to validate the token which has been sent by clients
var appSettings = jwtSection.Get<JWTSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

builder.Services.AddAuthentication(x =>
	{
		x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(x =>
	{
		x.RequireHttpsMetadata = true;
		x.SaveToken = true;
		x.TokenValidationParameters = new TokenValidationParameters
		                              {
			                              ValidateIssuerSigningKey = true,
			                              IssuerSigningKey = new SymmetricSecurityKey(key),
			                              ValidateIssuer = false,
			                              ValidateAudience = false,
			                              ClockSkew = TimeSpan.Zero
		                              };
	});

builder.Services.AddSwaggerGen(gen =>
{
	gen.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Board Game API", Version = "v1.0" });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerUI(ui =>
{
	ui.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Board Game Wishlist API Endpoint");
});

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();
