using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wasm.Shared;

namespace wasm.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameStateController : ControllerBase
    {

        private readonly ILogger<GameStateController> logger;

        public GameStateController(ILogger<GameStateController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<GameState> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new GameState
                {
                    Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}