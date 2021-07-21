using System.Threading.Tasks;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients
{
    public interface IParametersService
    {
        string BaseUrl { get; set; }

        Task<ParameterValue> GetParameter(string name);
        Task SetParameter(string name, double value);
    }
}
