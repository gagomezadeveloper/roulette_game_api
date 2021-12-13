using RouletteGameApi.Dtos;
using RouletteGameApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RouletteGameApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayersService _playersService;
    private readonly IRoulettesService _roulettesService;
    public PlayersController(IPlayersService playersService, IRoulettesService roulettesService)
    {
        _playersService = playersService;
        _roulettesService = roulettesService;
    }

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

    [Authorize]
    [HttpPost("GetPlayerRoulettes")]
    public async Task<IActionResult> GetPlayerRoulettes()
    {
        var response = await _roulettesService.GetPlayerRoulettes();
        if (response.Success)
        {
            return Ok(response);
        }
        else
        {
            return BadRequest(response);
        }
    }

    [Authorize]
    [HttpPost("SetPlayerBet")]
    public async Task<IActionResult> SetPlayerBet(SetPlayerBet bet)
    {
        var response = await _roulettesService.SetPlayerBet(bet);
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