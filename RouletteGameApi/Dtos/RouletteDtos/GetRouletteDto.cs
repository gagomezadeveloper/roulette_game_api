using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RouletteGameApi.Models;

namespace RouletteGameApi.Dtos;
public class GetRouletteDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string State { get; set; } = null!;
    public List<GetPlayerDto> Players { get; set; } = new List<GetPlayerDto>();
    public Double CurrentAmount { get; set; }
    public Double MaxAmount { get; set; }
    public string? Created { get; set; }

    public GetRouletteDto(Roulette roulette)
    {
        Id = roulette.Id;
        CurrentAmount = roulette.CurrentAmount;
        Players = (from p in roulette.Players select new GetPlayerDto(p)).ToList();
        State = roulette.IsClosed ? "closed" : "open";
        Created = $"{roulette.Created:yyyy-MM-dd:HH:mm:ss}Z";
    }
}