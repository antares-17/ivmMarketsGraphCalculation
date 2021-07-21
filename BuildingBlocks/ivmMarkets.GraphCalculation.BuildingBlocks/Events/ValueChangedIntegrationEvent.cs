using ivmMarkets.GraphCalculation.BuildingBlocks.EventBus.Events;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.Events
{
    public record ValueChangedIntegrationEvent : IntegrationEvent
    {
        public string ParameterName { get; private init; }

        public ValueChangedIntegrationEvent(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}