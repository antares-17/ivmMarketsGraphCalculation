using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.EventBus.Abstractions;
using ivmMarkets.GraphCalculation.BuildingBlocks.Calculation;
using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.Events
{
    public class ValueChangedIntegrationEventHandler : IIntegrationEventHandler<ValueChangedIntegrationEvent>
    {
        private readonly IParametersRepository _repository;
        private readonly ICalculationNode _calculationNode;
        private readonly ILogger<ValueChangedIntegrationEventHandler> _logger;

        public ValueChangedIntegrationEventHandler(
            ICalculationNode calculationNode,
            IParametersRepository repository,
            ILogger<ValueChangedIntegrationEventHandler> logger)
        {
            _calculationNode = calculationNode ?? throw new ArgumentNullException(nameof(calculationNode));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        public async Task Handle(ValueChangedIntegrationEvent @event)
        {
            if (_calculationNode.DependensOn.Contains(@event.ParameterName))
            {
                var result = await _calculationNode.Calculate();
                if (result.HasValue)
                    await _repository.SetParameterAsync(_calculationNode.Name, result.Value);
            }
        }
    }
}

