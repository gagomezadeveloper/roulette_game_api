using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RouletteGameApi.Models;
public class Roulette
{
    private const int rouletteSize = 36;
    private const double maxAmount = 10_000;
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public List<RouletteSquare> Squares { get; set; } = new List<RouletteSquare>();
    public List<Player> Players { get; set; } = new List<Player>();
    public bool IsClosed { get; set; }
    public Double MaxAmount { get; set; }
    public Double CurrentAmount
    {
        get
        {
            var totalFromBets = (from s in Squares
                                 select ((from b in s.Bets select b.Amount).Sum())).Sum();
            return totalFromBets;
        }
    }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Modified { get; set; }

    public Roulette()
    {
        BuildRoulette();
    }

    private void BuildRoulette(double _maxAmount = maxAmount)
    {
        for (var i = 0; i <= rouletteSize; i++)
        {
            Squares.Add(new RouletteSquare
            {
                Id = i,
                Color = i % 2 == 0 ? RouletteSquareColor.Red : RouletteSquareColor.Black,
                Bets = new List<RouletteBet>()
            });
        }
        MaxAmount = _maxAmount;
    }

}