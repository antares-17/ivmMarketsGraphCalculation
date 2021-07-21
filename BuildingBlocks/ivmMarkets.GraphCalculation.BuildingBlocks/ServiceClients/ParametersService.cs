using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients
{
    public class ParametersService : IParametersService
    {
        private readonly HttpClient _apiClient;
        private readonly ILogger<ParametersService> _logger;
        
        public string BaseUrl { get; set; }

        public ParametersService(HttpClient httpClient, ILogger<ParametersService> logger)
        {
            _apiClient = httpClient;
            _logger = logger;
        }

        public async Task<ParameterValue> GetParameter(string name)
        {
            try
            {
                var uri = GetParameterUri(name);
                var response = await _apiClient.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ParameterValue>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Getting parameter value error");
            }
            return null;
        }        

        public async Task SetParameter(string name, double value)
        {
            try
            {
                var uri = GetParameterUri(name) + $"?value={value}";
                var response = await _apiClient.PostAsync(uri, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setting parameter value error");
            }
        }

        private string GetParameterUri(string name)
        {
            return $"{BaseUrl}/api/v1/parameters/{name}";
        }
    }
}
