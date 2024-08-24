using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VotingSystem.Configurations;
using VotingSystem.Data;
using VotingSystem.Dtos.Request;
using VotingSystem.Dtos.Responses;
using VotingSystem.Entities;

namespace VotingSystem.Services;

public class IdentityService
{
    private readonly AppDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager; // geralmente usado p/ gerenciamente de users
    private readonly JwtOptions _jwtOptions;
    public string UserId { get; private set; }
    public IdentityService(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<JwtOptions> jwtOptions, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UserRegisterResponse> CadastrarUsuario(UserRegisterRequest usuarioCadastro)
    {
        var identityUser = new User
        {
            UserName = usuarioCadastro.Email,
            Email = usuarioCadastro.Email,
            EmailConfirmed = true 
        };
        var result = await _userManager.CreateAsync(identityUser, usuarioCadastro.Senha);
        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(identityUser, false); // desbloqueia o usuário

        var usuarioCadastroResponse = new UserRegisterResponse(result.Succeeded);
        if (!result.Succeeded && result.Errors.Count() > 0)
            usuarioCadastroResponse.AdicionarErros(result.Errors.Select(r => r.Description));

        return usuarioCadastroResponse;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest usuarioLogin)
    {
        var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

        
        var usuarioLoginResponse = new UserLoginResponse(result.Succeeded);
        if (result.Succeeded)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == usuarioLogin.Email);
            this.UserId = user.Id;
            var token = await GenerateToken(usuarioLogin.Email);
            Console.WriteLine(token);
            usuarioLoginResponse.Token =  token ;
        }

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                usuarioLoginResponse.AdicionarErro("Conta bloqueada");
            else if (result.IsNotAllowed)
                usuarioLoginResponse.AdicionarErro("Essa conta não tem permissão para essa ação");
            else if (result.RequiresTwoFactor)
                usuarioLoginResponse.AdicionarErro("É necessário confirmar o login com o código de 2 fatores");
            else
                usuarioLoginResponse.AdicionarErro("E-mail or password invalid");
        }

        return usuarioLoginResponse;
    }

    private async Task<string> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var tokenClaims = await GetClaims(user);

        var expDate = DateTime.Now.AddSeconds(_jwtOptions.Expiration);
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: tokenClaims,
            notBefore: DateTime.Now,
            expires: expDate,
            signingCredentials: _jwtOptions.SigningCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }

    private async Task<IList<Claim>> GetClaims(User user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        return claims;
    }

}