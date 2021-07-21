using System.Threading.Tasks;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients
{
    public interface ICalculationNodeService
    {
        string BaseUrl { get; set; }

        Task<ParameterValue> GetResullt();
    }
}
