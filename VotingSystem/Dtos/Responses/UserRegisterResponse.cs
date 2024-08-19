namespace VotingSystem.Dtos.Responses;

public class UserRegisterResponse
{
    public bool Sucesso { get; set; }
    public List<string> Errors { get; private set; }

    public UserRegisterResponse()
    {
        Errors = new List<string>();
    }

    public UserRegisterResponse(bool sucesso) : this() // Chama o construtor padrão para inicializar a lista
    {
        Sucesso = sucesso;
    }

    public void AdicionarErros(IEnumerable<string> erros)
    {
        Errors.AddRange(erros);
    }
}