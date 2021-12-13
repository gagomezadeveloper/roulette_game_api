using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace RouletteGameApi.Dtos;
public class GetRouletteDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string State { get; set; } = null!;
    public List<GetPlayerDto> Players { get; set; } = new List<GetPlayerDto>();
    public Double CurrentAmount { get; set; }
    public string? Created { get; set; }
}