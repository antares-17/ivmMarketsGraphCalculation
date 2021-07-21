using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.Fe.API.Services
{
    public class CalculationNode : ICalculationNode
    {
        private readonly IParametersService _parametersService;
        private readonly ICalculationNodeService _fcdService;
        private readonly ICalculationNodeService _fxService;
        private readonly string[] _dependsOn = new string[] { "e", "Fcd", "Fx" };

        public string Name => "Fe";

        public List<string> DependensOn => _dependsOn.ToList<string>();

        public CalculationNode(
            IOptions<AppSettings> settings, 
            IParametersService parametersService,
            ICalculationNodeService fcdService,
            ICalculationNodeService fxService)
        {
            _parametersService = parametersService;
            _parametersService.BaseUrl = settings.Value.ParametersUrl;
            _fcdService = fcdService;
            _fcdService.BaseUrl = settings.Value.FcdUrl;
            _fxService = fxService;
            _fxService.BaseUrl = settings.Value.FxUrl;
        }        

        public async Task<double?> Calculate()
        {
            var e = await _parametersService.GetParameter("e");
            var fcd = await _fcdService.GetResullt();
            var fx = await _fxService.GetResullt();
            if (e != null && fcd != null && fx != null)
                return e.Value + fcd.Value + fx.Value;
            return null;
        }
    }
}
