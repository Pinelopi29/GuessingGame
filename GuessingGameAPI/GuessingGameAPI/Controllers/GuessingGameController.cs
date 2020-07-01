using System.Collections.Generic;
using GuessingGame;
using GuessingGameApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GuessingGameApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GuessingGameController : ControllerBase
    {
        private ILogger _logger;
        private IGuessingGameService _service;


        public GuessingGameController(ILogger<GuessingGameController> logger, IGuessingGameService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("/api/RandomNumber")]
        public ActionResult<int> GetRandomNumber()
        {
            return _service.GetRandomNumber();
        }

        [HttpPost("/api/ScoreBoard")]
        public ActionResult<List<Scoreboard>> UpdateScoreBoards(Scoreboard scoreboard)
        {
            return (_service.UpdateScoreBoards(scoreboard.UserName, scoreboard.Attempts, scoreboard.Seconds, scoreboard.DateTime));
            
        }
    }
}
