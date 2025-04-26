using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stratzclone.Server.Interfaces;

namespace stratzclone.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;

        public MatchController(IMatchService service)
            => _service = service;

        [HttpGet("{steamId}")]
        public async Task<IActionResult> FetchAndSave(string steamId)
        {
            var saved = await _service.FetchAndSaveMatchesAsync(steamId);
            return Ok(new
            {
                Count   = saved.Count(),
                Matches = saved
            });
        }
    }
}
