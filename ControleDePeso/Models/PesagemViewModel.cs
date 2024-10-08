﻿using System.ComponentModel.DataAnnotations;

namespace ControleDePeso.Models
{
    public class PesagemViewModel
    {
        public decimal Peso { get; set; }
        [Display(Name = "Data do registro")]
        public DateOnly DataDoRegistro { get; set; }
    }
}
