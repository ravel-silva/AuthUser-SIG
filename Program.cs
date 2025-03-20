using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuth.Authorization;
using UserAuth.Data;
using UserAuth.Model;
using UserAuth.Profiles;
using UserAuth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Injeção do banco de dados
var connectionString = builder.Configuration.GetConnectionString("UserConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// injeção do automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();
// injeção do serviço de autenticação
builder.Services.AddSingleton<IAuthorizationHandler, AcessoAuthorization>();
//configuração de policy de autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.AddRequirements(new NivelDeAcesso("Administrador"))); //policy de admin acesso total
    options.AddPolicy("RequireSupervisor", policy => policy.AddRequirements(new NivelDeAcesso("Supervisor"))); //policy de supervisor acesso parcial
    options.AddPolicy("RequireStandardUser", policy => policy.AddRequirements(new NivelDeAcesso("Padrao"))); //policy de usuario padrão acesso basico
    options.AddPolicy("RequireElevatedAccess", policy => policy.AddRequirements(new NivelDeAcesso("RequireElevatedAccess"))); //policy de acesso elevado acesso para modificação de dados


    //options.AddPolicy("RequireSupervisor", options => options.RequireRole("Supervisores"));
    //options.AddPolicy("RequireStandardUser", options => options.RequireRole("Padrao"));
    //options.AddPolicy("RequireElevatedAccess", options => options.RequireRole("RequireElevatedAccess"));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopásdfghjklçzxcvbnm7531522HJBHKNJLMKFDKGSSHSAEW")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
