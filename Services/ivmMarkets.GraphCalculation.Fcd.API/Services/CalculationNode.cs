using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Fcd.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly IParametersService _parametersService;
        private readonly ICalculationNodeService _fabService;
        private readonly ICalculationNodeService _fyService;
        private readonly string[] _dependsOn = new string[] { "c", "d", "Fab", "Fy" };

        public string Name => "Fcd";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(
            IOptions<AppSettings> settings, 
            IParametersService parametersService,
            ICalculationNodeService fabService,
            ICalculationNodeService fyService)
        {
            _parametersService = parametersService;
            _parametersService.BaseUrl = settings.Value.ParametersUrl;
            _fabService = fabService;
            _fabService.BaseUrl = settings.Value.FabUrl;
            _fyService = fyService;
            _fyService.BaseUrl = settings.Value.FyUrl;
        }        

        public async Task<double?> Calculate()
        {
            var c = await _parametersService.GetParameter("c");
            var d = await _parametersService.GetParameter("d");
            var fab = await _fabService.GetResullt();
            var fy = await _fyService.GetResullt();
            if (c != null && d != null && fab != null && fy != null)
                return c.Value + d.Value + fab.Value + fy.Value;
            return null;
        }
    }
}
