using System.Collections.Generic;
using System.Threading.Tasks;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.Calculation
{
    public interface ICalculationNode
    {
        string Name { get; }
        List<string> DependensOn { get; }
        Task<double?> Calculate();
    }
}
