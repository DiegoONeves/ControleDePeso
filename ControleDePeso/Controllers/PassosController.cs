using ControleDePeso.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDePeso.Controllers
{
    public class PassosController : Controller
    {
        private readonly BancoDeDados _bd;
        public PassosController(ILogger<PassosController> logger, BancoDeDados bd)
        {
            _bd = bd;
            _logger = logger;
        }
        private readonly ILogger<PassosController> _logger;
        public IActionResult Gravar()
        {
            return View(new GravarPassosViewModel
            {
                DataDoRegistro = DateOnly.Parse(DateTime.Now.ToShortDateString())
            });
        }

        [HttpPost]
        public async Task<IActionResult> Gravar(GravarPassosViewModel model)
        {
            await _bd.GravarPassosAsync(model);
            return RedirectToAction("Index", "Home");
        }
    }
}
