using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
using RouletteGameApi.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace RouletteGameApi.Services;
public class PlayersService : IPlayersService
{
    private readonly IMongoCollection<Roulette> _context;

    public PlayersService(IOptions<StoreDatabaseSettings> storeDatabaseSettings)
    {
        var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
        _context = mongoDatabase.GetCollection<Roulette>(storeDatabaseSettings.Value.CollectionName);
    }

    public Task<ServiceResponseDto<string>> LoginPlayer(LoginPlayerDto login)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponseDto<List<GetRouletteDto>>> GetRoulettes()
    {
        var result = new ServiceResponseDto<List<GetRouletteDto>>();
        try
        {
            var data = await _context.Find(_ => true).ToListAsync();
            var query = from r in await _context.Find(_ => true).ToListAsync()
                        orderby r.Created descending
                        select new GetRouletteDto
                        {
                            Id = r.Id,
                            CurrentAmount = r.CurrentAmount,
                            Players = (from p in r.Players select new GetPlayerDto(p)).ToList(),
                            State = r.IsClosed ? "closed" : "open",
                            Created = $"{r.Created:yyyy-MM-dd:HH:mm:ss}Z"
                        };
            result.Data = query.ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"{ex}";
        }
        return result;
    }

    public async Task<ServiceResponseDto<GetPlayerDto>> RegisterPlayer(RegisterPlayerDto register)
    {
        var result = new ServiceResponseDto<GetPlayerDto>();
        try
        {
            if (register.Credit < 0) throw new Exception("Player's credit must be greater than zero.");
            var roulette = (await _context.FindAsync(r => r.Id == register.RouletteId)).FirstOrDefault();
            if (roulette is null) throw new Exception($"Roulette not found.");
            if (roulette.Players.FirstOrDefault(p => p.Username.ToUpper() == register.Username.ToUpper()) is not null) throw new Exception($"The player {register.Username} is already in the game.");
            roulette.Players.Add(new Player { Username = register.Username, Credit = register.Credit });
            // TODO: Validate concurrency
            await _context.ReplaceOneAsync(r => r.Id == register.RouletteId, roulette);
            var player = roulette.Players.Find(p => p.Username.ToUpper() == register.Username.ToUpper());
            if (player is null) throw new Exception($"Player not found.");
            result.Data = new GetPlayerDto(player);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Data = null;
            result.Message = $"{ex.Message}";
        }
        return result;
    }

}