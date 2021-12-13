using RouletteGameApi.Models;

namespace RouletteGameApi.Dtos;
public class SetPlayerBet
{    
    public int RouletteSquareId { get; set; }
    public RouletteSquareColor RouletteSquareColor { get; set; }
    public Double BetAmount { get; set; }
}