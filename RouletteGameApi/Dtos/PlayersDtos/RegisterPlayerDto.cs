using RouletteGameApi.Models;
namespace RouletteGameApi.Dtos;
public class RegisterPlayerDto
{
    public string RouletteId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public Double Credit { get; set; }
}