using RouletteGameApi.Dtos;
using RouletteGameApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace RouletteGameApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayersService _playersService;
    public PlayersController(IPlayersService playersService) => _playersService = playersService;

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

    [HttpPost("LoginPlayer")]
    public async Task<IActionResult> LoginPlayer(LoginPlayerDto login)
    {
        var response = await _playersService.LoginPlayer(login);
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