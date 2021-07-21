using ivmMarkets.GraphCalculation.ServiceBase;

namespace ivmMarkets.GraphCalculation.Fe.API
{
    public class AppSettings: AppSettingsBase
    {
        public string ParametersUrl { get; set; }
        public string FcdUrl { get; set; }
        public string FxUrl { get; set; }
    }
}
