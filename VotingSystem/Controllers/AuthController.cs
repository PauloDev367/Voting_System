using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotingSystem.Dtos.Request;
using VotingSystem.Dtos.Responses;
using VotingSystem.Entities;
using VotingSystem.Services;

namespace VotingSystem.Controllers;

[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private IdentityService _identityService;
    private readonly UserManager<User> _userManager;
    public AuthController(IdentityService identityService, UserManager<User> userManager)
    {
        _identityService = identityService;
        _userManager = userManager;
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await _identityService.CadastrarUsuario(request);

        if (resultado.Sucesso)
            return Ok(resultado);
        else if (resultado.Errors.Count > 0)
            return BadRequest(resultado);

        return StatusCode(500, new { error = "Internal server error" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await _identityService.Login(request);
        if (resultado.Sucesso)
            return Ok(resultado);

        return Unauthorized(resultado);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Ok(new SimpleUserResponse(user, _userManager));
            }
        }

        return Unauthorized();
    }
}