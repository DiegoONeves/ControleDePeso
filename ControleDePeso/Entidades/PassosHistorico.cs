namespace ControleDePeso.Entidades
{
    public class PassosHistorico
    {
        public DateTime DataDoRegistro { get; set; }
        public int Passos { get; set; }

        public override string ToString() => $"Data do Registro: {DataDoRegistro.ToShortDateString()} - Passos: {Passos}";

    }
}
