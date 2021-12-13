namespace RouletteGameApi.Models;
public class GetRouletteBetDto
{
    public double Amount { get; set; }
    public DateTime Created { get; set; }

    public GetRouletteBetDto(RouletteBet rouletteBet)
    {
        Amount = rouletteBet.Amount;
        Created = rouletteBet.Created;
    }
}