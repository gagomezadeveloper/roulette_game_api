namespace RouletteGameApi.Dtos;
public class LoginPlayerDto
{
    public string RouletteId { get; set; } = null!;
    public string Username { get; set; } = null!;

    public LoginPlayerDto() { }

    public LoginPlayerDto(LoginPlayerDto player)
    {
        RouletteId = player.RouletteId;
        Username = player.Username;
    }
}
