using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.InputParameters.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ParametersController : ControllerBase
    {
        private readonly IParametersRepository _repository;
        private readonly ILogger<ParametersController> _logger;

        public ParametersController(
            IParametersRepository repository,
            ILogger<ParametersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ParameterValue), (int)HttpStatusCode.OK)]
        public async Task<ParameterValue> Get(string name)
        {
            return await _repository.GetParameterAsync(name);
        }

        [HttpPost("{name}")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        public async Task Put(string name, double value)
        {
            await _repository.SetParameterAsync(name, value);
        }
    }
}
