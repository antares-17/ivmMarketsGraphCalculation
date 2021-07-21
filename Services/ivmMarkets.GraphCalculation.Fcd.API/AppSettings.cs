using ivmMarkets.GraphCalculation.ServiceBase;

namespace ivmMarkets.GraphCalculation.Fcd.API
{
    public class AppSettings: AppSettingsBase
    {
        public string ParametersUrl { get; set; }
        public string FabUrl { get; set; }
        public string FyUrl { get; set; }
    }
}
