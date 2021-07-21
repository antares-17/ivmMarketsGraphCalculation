using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;
using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;

namespace ivmMarkets.GraphCalculation.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParametersService _parametersSvc;
        private readonly ICalculationNodeService _fabSvc;
        private readonly ICalculationNodeService _fcdSvc;
        private readonly ICalculationNodeService _feSvc;
        private readonly ICalculationNodeService _fxSvc;
        private readonly ICalculationNodeService _fySvc;
        private readonly ICalculationNodeService _resultSvc;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IOptions<AppSettings> settings,
            IParametersService parametersSvc,
            ICalculationNodeService fabSvc,
            ICalculationNodeService fcdSvc,
            ICalculationNodeService feSvc,
            ICalculationNodeService fxSvc,
            ICalculationNodeService fySvc,
            ICalculationNodeService resultSvc, 
            ILogger<HomeController> logger)
        {
            _parametersSvc = parametersSvc;
            _parametersSvc.BaseUrl = settings.Value.ParametersUrl;
            _fabSvc = fabSvc;
            _fabSvc.BaseUrl = settings.Value.FabUrl;
            _fcdSvc = fcdSvc;
            _fcdSvc.BaseUrl = settings.Value.FcdUrl;
            _feSvc = feSvc;
            _feSvc.BaseUrl = settings.Value.FeUrl;
            _fxSvc = fxSvc;
            _fxSvc.BaseUrl = settings.Value.FxUrl;
            _fySvc = fySvc;
            _fySvc.BaseUrl = settings.Value.FyUrl;
            _resultSvc = resultSvc;
            _resultSvc.BaseUrl = settings.Value.ResultUrl;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> SetParameterValue(string parameterName, double parameterValue)
        {
            try
            {
                await _parametersSvc.SetParameter(parameterName, parameterValue);
                var parameter = await _parametersSvc.GetParameter(parameterName);
                if (parameter != null)
                    return Json(parameter.CalculationTime);
                return Json(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Setting parameter error.");
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> GetCalculationResults()
        {
            try
            {
                Dictionary<string, ParameterValue> calculationResults = new Dictionary<string, ParameterValue>();
                calculationResults.Add("Fab", await _fabSvc.GetResullt());
                calculationResults.Add("Fcd", await _fcdSvc.GetResullt());
                calculationResults.Add("Fe", await _feSvc.GetResullt());
                calculationResults.Add("Fx", await _fxSvc.GetResullt());
                calculationResults.Add("Fy", await _fySvc.GetResullt());
                calculationResults.Add("Result", await _resultSvc.GetResullt());
                return Json(calculationResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Calculation results getting error.");
                return Json(ex.Message);
            }
        }
    }
}
