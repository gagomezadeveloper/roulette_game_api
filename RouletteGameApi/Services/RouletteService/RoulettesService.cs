using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
using RouletteGameApi.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace RouletteGameApi.Services;
public class RoulettesService : IRoulettesService
{
    private readonly IMongoCollection<Roulette> _context;

    public RoulettesService(IOptions<StoreDatabaseSettings> storeDatabaseSettings)
    {
        var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
        _context = mongoDatabase.GetCollection<Roulette>(storeDatabaseSettings.Value.CollectionName);
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

    public async Task<ServiceResponseDto<Roulette>> SetNewGame()
    {
        var result = new ServiceResponseDto<Roulette>();
        try
        {
            var newRoulette = new Roulette();
            await _context.InsertOneAsync(newRoulette);
            result.Data = newRoulette;
            result.Message = $"New game created successfully.";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"{ex}";
        }
        return result;
    }
}
