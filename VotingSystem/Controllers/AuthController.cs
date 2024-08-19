using Microsoft.AspNetCore.Mvc;
using VotingSystem.Dtos.Request;
using VotingSystem.Services;

namespace VotingSystem.Controllers;

[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private IdentityService _identityService;
    public AuthController(IdentityService identityService) => _identityService = identityService;

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
}