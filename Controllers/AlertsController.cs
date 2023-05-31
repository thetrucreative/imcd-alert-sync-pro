using imcd_alert_sync_pro.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace imcd_alert_sync_pro.Controllers
{
    public class AlertsController : Controller
    { 
        private readonly ILogger<ProgramModel> _logger;

        public AlertsController(ILogger<ProgramModel> logger) 
        { 
            _logger = logger; 
        } 

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index()
        {
            var opgenieAlerts = await ProgramModel.GetOpsgenieAlerts();
            var model = new AlertsModel
            {
                Data = opgenieAlerts
            };

            await ProgramModel.PostToOpensearch(_logger);
            return View(model);
        }
    }
}
