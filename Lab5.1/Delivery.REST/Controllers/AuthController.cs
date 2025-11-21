using Delivery.Common.Entities;
using Delivery.REST.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Delivery.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<DeliveryUser> _userManager;
        private readonly SignInManager<DeliveryUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<DeliveryUser> userManager,
                              SignInManager<DeliveryUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        // -------------------------- Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new DeliveryUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User"); // роль за замовчуванням
            return Ok("Registered");
        }

        // -------------------------- Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized();
            if (!await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "ThisIsASuperSecretKey123456789012"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "DeliveryAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // -------------------------- SetRole
        [Authorize(Roles = "Admin")]
        [HttpPost("set-role")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest("Role does not exist");

            await _userManager.AddToRoleAsync(user, model.Role);
            return Ok($"Role '{model.Role}' assigned to {model.Email}");
        }
    }

    // -------------------------- DTOs
    public record RegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }

    public record LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public record SetRoleDto
    {
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
