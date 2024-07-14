using ControleDePeso.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDePeso.Controllers
{
    public class PesagemController : Controller
    {
        private readonly BancoDeDados _bd;
        public PesagemController(ILogger<PesagemController> logger, BancoDeDados bd)
        {
            _bd = bd;
            _logger = logger;
        }
        private readonly ILogger<PesagemController> _logger;
        public IActionResult Cadastrar()
        {
            return View(new PesagemViewModel
            {
                DataDoRegistro = DateOnly.Parse(DateTime.Now.ToShortDateString())
            });
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(PesagemViewModel model)
        {
            await _bd.CadastrarPeso(model);
            return RedirectToAction("Index", "Home");
        }
    }
}
