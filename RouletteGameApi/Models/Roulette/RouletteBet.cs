namespace RouletteGameApi.Models;
public class RouletteBet
{
    public int Id { get; set; }
    public string PlayerUsername { get; set; } = null!;
    public double Amount { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Modified { get; set; }
}