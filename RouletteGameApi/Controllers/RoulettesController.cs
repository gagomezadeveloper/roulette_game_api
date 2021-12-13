using RouletteGameApi.Models;
using RouletteGameApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace RouletteGameApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoulettesController : ControllerBase
{
    private readonly RoulettesService _roulettesService;
    public RoulettesController(RoulettesService roulettesService) => _roulettesService = roulettesService;

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var response = await _roulettesService.SetNewGame();
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
        var response = await _roulettesService.GetRoulettes();
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