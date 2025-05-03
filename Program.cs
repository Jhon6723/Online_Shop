using TiendaOnline1.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TiendaOnline1.Core.Mappers;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------- Services Configuration --------------- //
// autoMapper configuration
builder.Services.AddAutoMapper(typeof(UserProfileMapper));
// Add services to the container.
builder.Services.AddControllersWithViews();
// Configurar JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validar el emisor (issuer)
            ValidateAudience = true, // Validar el público (audience)
            ValidateLifetime = true, // Validar la expiración del token
            ValidateIssuerSigningKey = true, // Validar la clave de firma
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Emisor válido
            ValidAudience = builder.Configuration["Jwt:Audience"], // Público válido
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"))) // Clave secreta
        };
    });
builder.Services.AddDbContext<MysqlDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 25))));
// ----------------------------------- App Configuration ----------------------------------- //
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();