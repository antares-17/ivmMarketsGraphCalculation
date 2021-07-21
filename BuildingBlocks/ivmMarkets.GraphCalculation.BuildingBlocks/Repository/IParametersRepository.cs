using System.Threading.Tasks;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.Repository
{
    public interface IParametersRepository
    {
        Task<ParameterValue> GetParameterAsync(string name);
        Task SetParameterAsync(string name, double value);
    }
}
