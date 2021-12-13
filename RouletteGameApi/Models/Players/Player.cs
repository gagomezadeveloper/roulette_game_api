using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RouletteGameApi.Models;
public class Player
{
    public string Username { get; set; } = null!;
    public Double Credit { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Modified { get; set; }
}