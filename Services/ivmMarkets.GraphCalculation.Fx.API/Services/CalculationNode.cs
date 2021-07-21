using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Fx.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly IParametersService _parametersService;
        private readonly string[] _dependsOn = new string[] { "x" };

        public string Name => "Fx";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(IOptions<AppSettings> settings, IParametersService parametersService)
        {
            _parametersService = parametersService;
            _parametersService.BaseUrl = settings.Value.ParametersUrl;
        }        

        public async Task<double?> Calculate()
        {
            var x = await _parametersService.GetParameter("x");
            if (x != null)
                return x.Value;
            return null;
        }
    }
}
