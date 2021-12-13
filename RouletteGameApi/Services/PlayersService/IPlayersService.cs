using System.Threading.Tasks;
using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
namespace RouletteGameApi.Services;
public interface IPlayersService
{
    Task<ServiceResponseDto<GetPlayerDto>> RegisterPlayer(RegisterPlayerDto register);
    Task<ServiceResponseDto<string>> LoginPlayer(LoginPlayerDto login);
}