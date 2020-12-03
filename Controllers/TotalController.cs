using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DishChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TotalController : ControllerBase
    {
        private readonly ILogger<TotalController> _logger;

        public TotalController(ILogger<TotalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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
