
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(ApplicationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        user.PasswordHash = Hash(user.PasswordHash);
        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok(user);
    }

    [HttpPost("login")]
    public IActionResult Login(User login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
        if (user == null) return Unauthorized();

        if (user.PasswordHash != Hash(login.PasswordHash))
            return Unauthorized();

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }

    private string Hash(string input)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(
            sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
}
