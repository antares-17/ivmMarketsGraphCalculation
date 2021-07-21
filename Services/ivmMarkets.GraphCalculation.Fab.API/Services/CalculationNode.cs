using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Fab.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly IParametersService _parametersService;
        private readonly string[] _dependsOn = new string[] { "a", "b" };

        public string Name => "Fab";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(IOptions<AppSettings> settings, IParametersService parametersService)
        {
            _parametersService = parametersService;
            _parametersService.BaseUrl = settings.Value.ParametersUrl;
        }        

        public async Task<double?> Calculate()
        {
            var a = await _parametersService.GetParameter("a");
            var b = await _parametersService.GetParameter("b");
            if (a != null && b != null)
                return a.Value + b.Value;
            return null;
        }
    }
}
