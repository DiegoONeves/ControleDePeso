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
                new SelectListItem { Value ="1", Text =  "Janeiro" },
                new SelectListItem { Value ="2",Text ="Fevereiro"},
                new SelectListItem { Value ="3",Text ="Março"},
                new SelectListItem { Value ="4",Text = "Abril"},
                new SelectListItem { Value ="5",Text = "Maio"},
                new SelectListItem { Value ="6",Text = "Junho"},
                new SelectListItem { Value ="7",Text = "Julho"},
                new SelectListItem { Value ="8",Text = "Agosto"},
                new SelectListItem { Value ="9",Text = "Setembro"},
                new SelectListItem { Value ="10",Text = "Outubro"},
                new SelectListItem { Value ="11",Text = "Novembro"},
                new SelectListItem { Value ="12",Text = "Dezembro"}
            };
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashBoardViewModel();
            var t1 = _bd.BuscarHistoricoPesoAsync(5);
            var t2 = _bd.BuscarUltimosMesesAsync(4);
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