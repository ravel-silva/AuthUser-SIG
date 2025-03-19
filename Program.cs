using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuth.Authorization;
using UserAuth.Data;
using UserAuth.Model;
using UserAuth.Profiles;
using UserAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Inje��o do banco de dados
var connectionString = builder.Configuration.GetConnectionString("UserConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// inje��o do automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();

//configura��o de policy de autoriza��o
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", options => options.RequireRole("Administrador"));
    options.AddPolicy("RequireSupervisor", options => options.RequireRole("Supervisores"));
    options.AddPolicy("RequireStandardUser", options => options.RequireRole("Padrao"));
    options.AddPolicy("RequireElevatedAccess", options => options.RequireRole("RequireElevatedAccess"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
