using System;
using System.Text.Json.Serialization;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.Repository
{
    public record ParameterValue
    {
        [JsonConstructor]
        public ParameterValue(double value, DateTime calculationTime)
        {
            Value = value;
            CalculationTime = calculationTime;
        }

        [JsonInclude]
        public double Value { get; private init; }

        [JsonInclude]
        public DateTime CalculationTime { get; private init; }
    }
}
