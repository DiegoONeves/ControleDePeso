using ControleDePeso.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ControleDePeso.Controllers
{
    public class HomeController : Controller
    {
        private readonly BancoDeDados _bd;
        public HomeController(ILogger<HomeController> logger, BancoDeDados bd)
        {
            _bd = bd;
            _logger = logger;
        }
        private readonly ILogger<HomeController> _logger;

        private List<SelectListItem> ObterMesesHistorico()
        {
            return new List<SelectListItem>
            {
                new() { Value ="1", Text =  "Janeiro" },
                new() { Value ="2",Text ="Fevereiro"},
                new() { Value ="3",Text ="Março"},
                new() { Value ="4",Text = "Abril"},
                new() { Value ="5",Text = "Maio"},
                new() { Value ="6",Text = "Junho"},
                new() { Value ="7",Text = "Julho"},
                new() { Value ="8",Text = "Agosto"},
                new() { Value ="9",Text = "Setembro"},
                new() { Value ="10",Text = "Outubro"},
                new() { Value ="11",Text = "Novembro"},
                new() { Value ="12",Text = "Dezembro"}
            };
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashBoardViewModel();
            var t1 = _bd.BuscarHistoricoAsync(10);
            var t2 = _bd.BuscarUltimosMesesAsync(7);
            var t3 = _bd.BuscarMediaMovelSemanalAsync();
            var t4 = _bd.BuscarMediaMovelTrimestralAsync();
            var t5 = _bd.BuscarMediaMovelQuinzenalAsync();

            vm.Resultado.PesoHistorico = await t1;
            vm.Resultado.UltimosMeses = await t2;
            vm.Resultado.MediaMovelSemanal = await t3;
            vm.Resultado.MediaMovelTrimestral = await t4;
            vm.Resultado.MediaMovelQuinzenal = await t5;

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}