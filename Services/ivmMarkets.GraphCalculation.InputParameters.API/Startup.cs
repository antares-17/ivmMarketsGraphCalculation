using Microsoft.Extensions.Configuration;
using ivmMarkets.GraphCalculation.ServiceBase;

namespace ivmMarkets.GraphCalculation.InputParameters.API
{
    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration) { }        
    }
}
