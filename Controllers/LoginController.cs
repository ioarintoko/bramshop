using Microsoft.AspNetCore.Mvc;
using Data;
using Models;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly TokenService _tokenService;

    public LoginController(ApplicationDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginModel login)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == login.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
