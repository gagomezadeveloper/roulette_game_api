using RouletteGameApi.Dtos;
using RouletteGameApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace RouletteGameApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly PlayersService _playersService;
    public PlayersController(PlayersService playersService) => _playersService = playersService;

    [HttpPost("RegisterPlayer")]
    public async Task<IActionResult> RegisterPlayer(RegisterPlayerDto register)
    {
        var response = await _playersService.RegisterPlayer(register);
        if (response.Success)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("GetRoulettes")]
    public async Task<IActionResult> GetRoulettes()
    {
        var response = await _playersService.GetRoulettes();
        if (response.Success)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }
}