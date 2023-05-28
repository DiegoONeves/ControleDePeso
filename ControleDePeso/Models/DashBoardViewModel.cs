namespace ControleDePeso.Models
{
    public class DashBoardViewModel
    {
        public ResultadosDashBoard Resultado { get; set; } = new();

    }

    public class ResultadosDashBoard
    {
        public List<PesoHistoricoDashBoardViewModel> PesoHistorico { get; set; } = new();
        public MediaMovelPeso MediaMovelSemanal { get; set; } = new();

        public MediaMovelPeso MediaMovelTrimestral { get; set; } = new();

        public MediaMovelPeso MediaMovelQuinzenal { get; set; } = new();

        public List<UltimosMesesHistorico> UltimosMeses { get; set; } = new List<UltimosMesesHistorico>();
    }

    public class MediaMovelPeso
    {
        public string MediaMovel1 { get; set; } = string.Empty;
        public string MediaMovel2 { get; set; } = string.Empty;
        public string MediaMovel3 { get; set; } = string.Empty;
        public string MediaMovel4 { get; set; } = string.Empty;

    }
   
    public class PesoHistoricoDashBoardViewModel 
    {
        public int Ano { get; set; }
        public decimal Peso { get; set; }
    }

    public class PesoMedioPorPeriodo
    {
        public string Peso { get; set; } = "";
    }
    public class UltimosMesesHistorico
    {
        public DateTime ReferenciaDateTime { get; set; }
        public string Referencia { get; set; } = "";
        public string MediaDePeso { get; set; } = "";
    }

}
