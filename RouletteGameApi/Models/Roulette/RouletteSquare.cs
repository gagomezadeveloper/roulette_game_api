using System.Collections.Generic;
namespace RouletteGameApi.Models;
public class RouletteSquare
{
    public int Id { get; set; }
    public RouletteSquareColor Color { get; set; }
    public List<RouletteBet> Bets { get; set; } = new List<RouletteBet>();
}