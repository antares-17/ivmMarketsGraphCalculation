using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Fy.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly IParametersService _parametersService;
        private readonly string[] _dependsOn = new string[] { "y" };

        public string Name => "Fy";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(IOptions<AppSettings> settings, IParametersService parametersService)
        {
            _parametersService = parametersService;
            _parametersService.BaseUrl = settings.Value.ParametersUrl;
        }        

        public async Task<double?> Calculate()
        {
            var y = await _parametersService.GetParameter("y");
            if (y != null)
                return y.Value;
            return null;
        }
    }
}
