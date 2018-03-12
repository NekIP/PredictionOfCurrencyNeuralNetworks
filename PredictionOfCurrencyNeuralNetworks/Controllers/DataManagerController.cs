using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBase;
using DataManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PredictionOfCurrencyNeuralNetworks.Controllers {
    public class DataManagerController : Controller {
        public IUsdToRubCurrencyCollector UsdToRubCollector { get; set; }
        public DataManagerController(IUsdToRubCurrencyCollector usdToRubCollector) {
            UsdToRubCollector = usdToRubCollector;
        }

        // GET: /<controller>/
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public Task<List<Entity>> Get() {
            return UsdToRubCollector.List();
        }
    }
}
