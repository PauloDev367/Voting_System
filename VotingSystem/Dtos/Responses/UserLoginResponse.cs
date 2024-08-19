using System.Text.Json.Serialization;

namespace VotingSystem.Dtos.Responses;

public class UserLoginResponse
{
    public bool Sucesso { get; set; }
    // Token JWT
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // Expiração do token
    public DateTime? DataExpiracao{ get; set; }
    public List<string> Errors { get; private set; }


    public UserLoginResponse() => Errors = new List<string>();

    public UserLoginResponse(bool sucesso = true): this()=>Sucesso = sucesso;
    public UserLoginResponse(bool sucesso, string token, DateTime dataExpiracao): this()
    {
        Token = token;
        DataExpiracao = dataExpiracao;
    }

    public void AdicionarErros(IEnumerable<string> erros) => Errors.AddRange(erros);
    public void AdicionarErro(string erro) => Errors.Add(erro);


}