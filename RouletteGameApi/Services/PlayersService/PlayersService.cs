using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
using RouletteGameApi.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace RouletteGameApi.Services;
public class PlayersService : IPlayersService
{
    private readonly IMongoCollection<Roulette> _context;
    private readonly IConfiguration _configuration;

    public PlayersService(IOptions<StoreDatabaseSettings> storeDatabaseSettings, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(storeDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(storeDatabaseSettings.Value.DatabaseName);
        _context = mongoDatabase.GetCollection<Roulette>(storeDatabaseSettings.Value.CollectionName);
        _configuration = configuration;
    }

    public async Task<ServiceResponseDto<string>> LoginPlayer(LoginPlayerDto login)
    {
        var result = new ServiceResponseDto<string>();
        try
        {
            var roulette = (await _context.FindAsync(r => r.Id == login.RouletteId)).FirstOrDefault();
            if (roulette is null) throw new Exception($"Roulette not found.");
            if (roulette.IsClosed) throw new Exception($"The roulette {login.RouletteId} is closed.");
            var player = roulette.Players.FirstOrDefault(p => p.Username.ToUpper() == login.Username.ToUpper());
            if (player is null) throw new Exception($"The player {login.Username} is not in the game.");
            result.Data = CreateToken(login);
            result.Message = $"Welcome player {login.Username}";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Data = null;
            result.Message = $"{ex.Message}";
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
            if (roulette.IsClosed) throw new Exception($"The roulette {register.RouletteId} is closed.");
            if (roulette.Players.FirstOrDefault(p => p.Username.ToUpper() == register.Username.ToUpper()) is not null) throw new Exception($"The player {register.Username} is already in the game.");
            roulette.Players.Add(new Player { Username = register.Username, Credit = register.Credit });
            // TODO: Validate concurrency
            await _context.ReplaceOneAsync(r => r.Id == register.RouletteId, roulette);
            var player = roulette.Players.Find(p => p.Username.ToUpper() == register.Username.ToUpper());
            if (player is null) throw new Exception($"Player not found.");
            result.Message = $"{register.Username} welcome to roulette {register.RouletteId}!";
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

    private string CreateToken(LoginPlayerDto player)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, player.Username),
            new Claim(ClaimTypes.Name, player.RouletteId),
        };
        var apiToken = _configuration.GetSection("RouletteStoreDatabase:ApiToken").Value;
        var key = new SymmetricSecurityKey
        (
            System.Text.Encoding.UTF8.GetBytes(apiToken)
        );
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(Int16.Parse(_configuration.GetSection("RouletteStoreDatabase:TokenExpirationDaysTime").Value)),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }    
}