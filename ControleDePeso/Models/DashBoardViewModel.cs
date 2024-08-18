namespace ControleDePeso.Models
{
    public class DashBoardViewModel
    {
        public ResultadosDashBoard Resultado { get; set; } = new();

    }

    public class ResultadosDashBoard
    {
        public List<HistoricoDashBoardViewModel> PesoHistorico { get; set; } = [];
        public MediaMovel MediaMovelSemanal { get; set; } = new();

        public MediaMovel MediaMovelTrimestral { get; set; } = new();

        public MediaMovel MediaMovelQuinzenal { get; set; } = new();

        public List<UltimosMesesHistorico> UltimosMeses { get; set; } = [];
    }

    public class MediaMovel
    {
        public string MediaMovelPeso1 { get; set; } = string.Empty;
        public string MediaMovelPassos1 { get; set; } = string.Empty;
        public string MediaMovelPeso2 { get; set; } = string.Empty;
        public string MediaMovelPassos2 { get; set; } = string.Empty;
        public string MediaMovelPeso3 { get; set; } = string.Empty;
        public string MediaMovelPassos3 { get; set; } = string.Empty;
        public string MediaMovelPeso4 { get; set; } = string.Empty;
        public string MediaMovelPassos4 { get; set; } = string.Empty;

    }
   
    public class HistoricoDashBoardViewModel 
    {
        public int Ano { get; set; }
        public decimal Peso { get; set; }
        public int Passos { get; set; }
    }

    public class MedioPorPeriodo
    {
        public string Peso { get; set; } = "";
        public string Passos { get; set; } = "";
    }
    public class UltimosMesesHistorico
    {
        public DateTime ReferenciaDateTime { get; set; }
        public string Referencia { get; set; } = "";
        public string MediaDePeso { get; set; } = "";
        public string MediaDePassos { get; set; } = "";

    }

}
