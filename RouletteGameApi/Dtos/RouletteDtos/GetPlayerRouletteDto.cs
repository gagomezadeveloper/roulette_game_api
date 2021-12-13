using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RouletteGameApi.Models;

namespace RouletteGameApi.Dtos;
public class GetPlayerRouletteDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string State { get; set; } = null!;
    public Double CurrentAmount { get; set; }

    public GetPlayerRouletteDto(Roulette roulette)
    {
        Id = roulette.Id;
        State = roulette.IsClosed ? "closed" : "open";
        CurrentAmount = roulette.CurrentAmount;
    }
}