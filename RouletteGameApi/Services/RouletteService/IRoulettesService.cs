using RouletteGameApi.Dtos;
using RouletteGameApi.Models;
namespace RouletteGameApi.Services;
public interface IRoulettesService
{
    Task<ServiceResponseDto<Roulette>> SetNewGame();
    Task<ServiceResponseDto<List<GetRouletteDto>>> GetRoulettes();
    Task<ServiceResponseDto<List<GetPlayerRouletteDto>>> GetPlayerRoulettes();
}