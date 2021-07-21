using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.Result.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]    
    public class ValueController : ControllerBase
    {
        private readonly ILogger<ValueController> _logger;
        private readonly IParametersRepository _repository;

        public ValueController(
            IParametersRepository repository,
            ILogger<ValueController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ParameterValue), (int)HttpStatusCode.OK)]
        public async Task<ParameterValue> Get()
        {
            return await _repository.GetParameterAsync("Result");
        }
    }
}
