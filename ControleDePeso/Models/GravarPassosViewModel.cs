using System.ComponentModel.DataAnnotations;

namespace ControleDePeso.Models
{
    public class GravarPassosViewModel
    {
        public int Passos { get; set; }
        [Display(Name = "Data do registro")]
        public DateOnly DataDoRegistro { get; set; }
    }
}
