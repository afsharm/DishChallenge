using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DishChallenge.Controllers
{
    [ApiController]
    public class TotalController : ControllerBase
    {
        private readonly ILogger<TotalController> _logger;

        public TotalController(ILogger<TotalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Total")]
        public async Task<ContentResult> Total()
        {
            try
            {
                var result = await ChallengeService.CalcAsync();
                return Content(result.ToString());
            }
            catch (Exception exception)
            {
                return Content($"{exception.Message} - {exception.InnerException?.Message}");
            }
        }

        [HttpGet]
        [Route("TotalJ")]
        public async Task<IActionResult> TotalJ()
        {
            try
            {
                var result = await ChallengeService.CalcAsync();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest($"{exception.Message} - {exception.InnerException?.Message}");
            }
        }
    }
}
