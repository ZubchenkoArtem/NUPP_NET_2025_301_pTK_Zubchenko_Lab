using Delivery.Common.Entities;
using Delivery.Infrastructure;
using Delivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------------
// 1. DbContext (SQLite)
// --------------------------
builder.Services.AddDbContext<DeliveryDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// --------------------------
// 2. Identity
// --------------------------
builder.Services.AddIdentity<DeliveryUser, IdentityRole>()
    .AddEntityFrameworkStores<DeliveryDbContext>()
    .AddDefaultTokenProviders();

// --------------------------
// 3. JWT Authentication
// --------------------------
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsASuperSecretKey123456789012";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "DeliveryAPI";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// --------------------------
// 4. CORS
// --------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

// --------------------------
// 5. Controllers
// --------------------------
builder.Services.AddControllers();

// --------------------------
// 6. Swagger
// --------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery API",
        Version = "v1",
        Description = "REST API для доставки"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Вставте JWT токен у форматі: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// --------------------------
// 7. Створення ролей та Admin
// --------------------------
await CreateRolesAndAdminAsync(app.Services);

static async Task CreateRolesAndAdminAsync(IServiceProvider services)
{
    await using var scope = services.CreateAsyncScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DeliveryUser>>();

    string[] roles = new[] { "User", "Manager", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Створюємо Admin користувача, якщо його немає
    var adminEmail = "admin@test.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new DeliveryUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin"
        };
        await userManager.CreateAsync(adminUser, "Abc123!@#");
    }

    // Присвоюємо Admin роль
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        await userManager.AddToRoleAsync(adminUser, "Admin");
}

// --------------------------
// 8. Middleware
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
