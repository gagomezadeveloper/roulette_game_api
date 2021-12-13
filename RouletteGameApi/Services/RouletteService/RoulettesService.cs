using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
using RouletteGameApi.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Claims;
namespace RouletteGameApi.Services;
public class RoulettesService : IRoulettesService
{
    private readonly IMongoCollection<Roulette> _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RoulettesService(IOptions<StoreDatabaseSettings> storeDatabaseSettings, IHttpContextAccessor httpContextAccessor)
    {
        var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
        _context = mongoDatabase.GetCollection<Roulette>(storeDatabaseSettings.Value.CollectionName);
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponseDto<List<GetPlayerRouletteDto>>> GetPlayerRoulettes()
    {
        var result = new ServiceResponseDto<List<GetPlayerRouletteDto>>();
        try
        {
            var player = GetUserId();
            var query = await _context.Find(r => r.Id != null && r.Id.Equals(player.RouletteId)).ToListAsync();
            if (query is null) throw new Exception($"Roulette not found.");
            result.Message = $"{player.Username}'s open roulettes.";
            result.Data = (from r in query select new GetPlayerRouletteDto(r)).ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"{ex.Message}";
        }
        return result;
    }

    public async Task<ServiceResponseDto<List<GetRouletteDto>>> GetRoulettes()
    {
        var result = new ServiceResponseDto<List<GetRouletteDto>>();
        try
        {
            var data = await _context.Find(_ => true).ToListAsync();
            var query = from r in await _context.Find(_ => true).ToListAsync()
                        orderby r.Created descending
                        select new GetRouletteDto(r);
            result.Data = query.ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"{ex.Message}";
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
            result.Message = $"{ex.Message}";
        }
        return result;
    }

    private LoginPlayerDto GetUserId()
    {
        if (_httpContextAccessor.HttpContext is null) throw new NullReferenceException("HttpContext");
        return new LoginPlayerDto
        {
            Username = $"{_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)}",
            RouletteId = $"{_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name)}"
        };
    }

}
