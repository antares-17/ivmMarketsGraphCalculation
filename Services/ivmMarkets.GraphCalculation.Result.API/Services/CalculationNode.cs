using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Result.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly ICalculationNodeService _feService;
        private readonly string[] _dependsOn = new string[] { "Fe" };

        public string Name => "Result";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(
            IOptions<AppSettings> settings,
            ICalculationNodeService feService)
        {
            _feService = feService;
            _feService.BaseUrl = settings.Value.FeUrl;
        }        

        public async Task<double?> Calculate()
        {
            var fe = await _feService.GetResullt();
            if (fe != null)
                return fe.Value;
            return null;
        }
    }
}
