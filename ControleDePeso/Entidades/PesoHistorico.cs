﻿namespace ControleDePeso.Entidades
{
    public class PesoHistorico
    {
        public DateTime DataDoRegistro { get; set; }
        public decimal Peso { get; set; }

        public override string ToString()
            => $"Data do Registro: {DataDoRegistro.ToShortDateString()} - Peso: {Peso}";
        
    }
}
