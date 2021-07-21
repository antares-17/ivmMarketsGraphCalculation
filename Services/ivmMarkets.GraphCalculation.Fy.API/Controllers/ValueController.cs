using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;
using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;

namespace ivmMarkets.GraphCalculation.Fab.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ValueController : ControllerBase
    {
        private readonly ILogger<ValueController> _logger;
        private readonly IParametersRepository _repository;
        private readonly ICalculationNode _calculationNode;

        public ValueController(
            IParametersRepository repository,
            ICalculationNode calculationNode,
            ILogger<ValueController> logger)
        {
            _repository = repository;
            _calculationNode = calculationNode;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ParameterValue), (int)HttpStatusCode.OK)]
        public async Task<ParameterValue> Get()
        {
            return await _repository.GetParameterAsync(_calculationNode.Name);
        }
    }
}
