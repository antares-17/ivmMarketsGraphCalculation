using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;

using ivmMarkets.GraphCalculation.BuildingBlocks.EventBus.Abstractions;
using ivmMarkets.GraphCalculation.BuildingBlocks.Events;


namespace ivmMarkets.GraphCalculation.BuildingBlocks.Repository
{
    public class ParametersRepository: IParametersRepository
    {
        private readonly IEventBus _eventBus;
        private readonly IDatabase _database;
        private readonly ILogger<ParametersRepository> _logger;

        public ParametersRepository(
            ConnectionMultiplexer redis,
            IEventBus eventBus,
            ILoggerFactory loggerFactory)
        {            
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _database = redis.GetDatabase();
            _logger = loggerFactory.CreateLogger<ParametersRepository>();
        }

        public async Task<ParameterValue> GetParameterAsync(string name)
        {
            var data = await _database.StringGetAsync(name);

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<ParameterValue>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task SetParameterAsync(string name, double value)
        {
            await _database.StringSetAsync(name, JsonSerializer.Serialize(new ParameterValue(value, DateTime.Now)));
            _eventBus.Publish(new ValueChangedIntegrationEvent(name));
        }
    }
}
