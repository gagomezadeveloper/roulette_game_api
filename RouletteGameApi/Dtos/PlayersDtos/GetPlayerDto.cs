using RouletteGameApi.Models;
namespace RouletteGameApi.Dtos;
public class GetPlayerDto
{
    public string Username { get; set; } = null!;
    public Double Credit { get; set; }
    public string RegisterAt { get; set; } = null!;

    public GetPlayerDto(Player player)
    {
        Username = player.Username;
        Credit = player.Credit;
        RegisterAt = $"{player.Created:yyyy-MM-dd:HH:mm:ss}Z";
    }
}