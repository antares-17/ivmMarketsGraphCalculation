using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients
{
    public class CalculationNodeService : ICalculationNodeService
    {
        private readonly HttpClient _apiClient;
        private readonly ILogger<CalculationNodeService> _logger;
        
        public string BaseUrl { get; set; }

        public CalculationNodeService(HttpClient httpClient, ILogger<CalculationNodeService> logger)
        {
            _apiClient = httpClient;
            _logger = logger;
        }

        public async Task<ParameterValue> GetResullt()
        {
            try
            {
                var uri = GetValueUri();
                var response = await _apiClient.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ParameterValue>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении результата");
            }
            return null;
        }        

        private string GetValueUri()
        {
            return $"{BaseUrl}/api/v1/value/";
        }
    }
}
