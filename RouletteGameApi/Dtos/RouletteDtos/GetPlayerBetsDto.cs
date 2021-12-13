using RouletteGameApi.Models;
namespace RouletteGameApi.Dtos;
public class GetPlayerBetsDtos
{
    public string RouletteId { get; set; } = null!;
    public string PlayerUsername { get; set; } = null!;
    public Double TotalAmount
    {
        get
        {
            return Bets.Sum(s => s.Bets.Where(b => b.PlayerUsername.ToUpper() == PlayerUsername.ToUpper()).Sum(b => b.Amount));
        }
    }
    public List<RouletteSquare> Bets { get; set; } = new List<RouletteSquare>();
    public GetPlayerBetsDtos(LoginPlayerDto player, List<RouletteSquare> squares)
    {
        RouletteId = player.RouletteId;
        PlayerUsername = player.Username;
        Bets = squares;
    }
}