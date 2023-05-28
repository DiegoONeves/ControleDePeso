﻿using ControleDePeso.Models;
using Dapper;
using System.Data.SqlClient;

namespace ControleDePeso
{
    public class BancoDeDados
    {
        public string ConnectionString { get; set; } = "";
        public BancoDeDados(IConfiguration config) => ConnectionString = config.GetConnectionString("DefaultConnection");


        public async Task<List<PesoHistoricoDashBoardViewModel>> BuscarHistoricoPesoAsync(int ultimosAno)
        {

            List<PesoHistoricoDashBoardViewModel> r = new();
            List<int> anos = new();
            int anoAtual = DateTime.Now.Year;
            int anoInicial = DateTime.Now.AddYears(-ultimosAno).Year;

            for (int i = anoInicial; i <= anoAtual; i++)
                anos.Add(i);

            anos = anos.OrderByDescending(x => x).ToList();

            foreach (var ano in anos)
            {
                var l = new PesoHistoricoDashBoardViewModel { Ano = ano };

                using (var conn = new SqlConnection(ConnectionString))
                {
                    var temp = await conn.QueryAsync<PesoHistoricoDashBoardViewModel>(@"
                                            SELECT AVG(Peso) AS Peso FROM ControleDePeso WHERE YEAR(DataDoRegistro) = @Ano",
                                            new { @Ano = ano });

                    var primeiro = temp.First();

                    if (primeiro.Peso > 0)
                        l.Peso = primeiro.Peso;

                    r.Add(l);
                }
            }

            return r;

        }

        public async Task<MediaMovelPeso> BuscarMediaMovelSemanalAsync()
        {
            MediaMovelPeso r = new();
            DateOnly dataAtual = DateOnly.FromDateTime(DateTime.Now.Date);
            var ultimos7Dias = dataAtual.AddDays(-7);
            var ultimos14Dias = ultimos7Dias.AddDays(-7);
            var ultimos21Dias = ultimos14Dias.AddDays(-7);
            var ultimos28Dias = ultimos21Dias.AddDays(-7);

            List<string> datasPrimeiroPeriodo = new();
            List<string> datasSegundoPeriodo = new();
            List<string> datasTerceiroPeriodo = new();
            List<string> datasQuartoPeriodo = new();

            datasPrimeiroPeriodo.Add(dataAtual.ToString("yyyy-MM-dd"));
            datasPrimeiroPeriodo.Add(ultimos7Dias.ToString("yyyy-MM-dd"));

            datasSegundoPeriodo.Add(ultimos7Dias.AddDays(-1).ToString("yyyy-MM-dd"));
            datasSegundoPeriodo.Add(ultimos14Dias.ToString("yyyy-MM-dd"));

            datasTerceiroPeriodo.Add(ultimos14Dias.AddDays(-1).ToString("yyyy-MM-dd"));
            datasTerceiroPeriodo.Add(ultimos21Dias.ToString("yyyy-MM-dd"));

            datasQuartoPeriodo.Add(ultimos21Dias.AddDays(-1).ToString("yyyy-MM-dd"));
            datasQuartoPeriodo.Add(ultimos28Dias.ToString("yyyy-MM-dd"));

            using (var conn = new SqlConnection(ConnectionString))
            {
                //ÚLTIMOS 7 DIAS - PESO
                r.MediaMovel1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                //ÚLTIMOS 14 DIAS - PESO
                r.MediaMovel2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                //ÚLTIMOS 21 DIAS - PESO
                r.MediaMovel3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                //ÚLTIMOS 28 DIAS - PESO
                r.MediaMovel4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

            }

            return r;



        }

        public async Task<MediaMovelPeso> BuscarMediaMovelTrimestralAsync()
        {

            MediaMovelPeso r = new();
            DateOnly dataAtual = DateOnly.FromDateTime(DateTime.Now.Date);
            var ultimoTrimestre = dataAtual.AddMonths(-3);
            var penultimoTrimestre = ultimoTrimestre.AddMonths(-3);
            var antePenultimoTrimestre = penultimoTrimestre.AddMonths(-3);
            var trimestreMaisAntifo = antePenultimoTrimestre.AddMonths(-3);

            List<string> datasPrimeiroPeriodo = new();
            List<string> datasSegundoPeriodo = new();
            List<string> datasTerceiroPeriodo = new();
            List<string> datasQuartoPeriodo = new();

            datasPrimeiroPeriodo.Add(dataAtual.ToString("yyyy-MM-dd"));
            datasPrimeiroPeriodo.Add(ultimoTrimestre.ToString("yyyy-MM-dd"));

            datasSegundoPeriodo.Add(ultimoTrimestre.AddDays(-1).ToString("yyyy-MM-dd"));
            datasSegundoPeriodo.Add(penultimoTrimestre.ToString("yyyy-MM-dd"));

            datasTerceiroPeriodo.Add(penultimoTrimestre.AddDays(-1).ToString("yyyy-MM-dd"));
            datasTerceiroPeriodo.Add(antePenultimoTrimestre.ToString("yyyy-MM-dd"));

            datasQuartoPeriodo.Add(antePenultimoTrimestre.AddDays(-1).ToString("yyyy-MM-dd"));
            datasQuartoPeriodo.Add(trimestreMaisAntifo.ToString("yyyy-MM-dd"));

            using (var conn = new SqlConnection(ConnectionString))
            {
                r.MediaMovel1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovel2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovel3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovel4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

            }

            return r;
        }

        public async Task<MediaMovelPeso> BuscarMediaMovelQuinzenalAsync()
        {
            MediaMovelPeso r = new();
            DateOnly dataAtual = DateOnly.FromDateTime(DateTime.Now.Date);
            var ultimaQuinzena = dataAtual.AddDays(-15);
            var penultimaQuinzena = ultimaQuinzena.AddDays(-15);
            var antePenultimaQuinzena = penultimaQuinzena.AddDays(-15);
            var quinzenaMaisAntiga = antePenultimaQuinzena.AddDays(-15);

            List<string> datasPrimeiroPeriodo = new();
            List<string> datasSegundoPeriodo = new();
            List<string> datasTerceiroPeriodo = new();
            List<string> datasQuartoPeriodo = new();

            datasPrimeiroPeriodo.Add(dataAtual.ToString("yyyy-MM-dd"));
            datasPrimeiroPeriodo.Add(ultimaQuinzena.ToString("yyyy-MM-dd"));

            datasSegundoPeriodo.Add(ultimaQuinzena.AddDays(-1).ToString("yyyy-MM-dd"));
            datasSegundoPeriodo.Add(penultimaQuinzena.ToString("yyyy-MM-dd"));

            datasTerceiroPeriodo.Add(penultimaQuinzena.AddDays(-1).ToString("yyyy-MM-dd"));
            datasTerceiroPeriodo.Add(antePenultimaQuinzena.ToString("yyyy-MM-dd"));

            datasQuartoPeriodo.Add(antePenultimaQuinzena.AddDays(-1).ToString("yyyy-MM-dd"));
            datasQuartoPeriodo.Add(quinzenaMaisAntiga.ToString("yyyy-MM-dd"));

            using (var conn = new SqlConnection(ConnectionString))
            {
                r.MediaMovel1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovel2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovel3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovel4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });
            }

            return r;
        }



        public async Task<List<UltimosMesesHistorico>> BuscarUltimosMesesAsync(int qtdMeses)
        {
            List<UltimosMesesHistorico> r = new();
            DateTime mesRef = DateTime.Now;
            List<DateTime> meses = new();

            for (int i = 0; i < qtdMeses; i++)
                meses.Add(mesRef.AddMonths(-i));

            foreach (var m in meses)
            {
                UltimosMesesHistorico h = new()
                {
                    ReferenciaDateTime = m,
                    Referencia = $"{ObterNomeMes(m.Month)}/{m.Year}"
                };

                using (var conn = new SqlConnection(ConnectionString))
                {
                    //PESO
                    h.MediaDePeso = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM ControleDePeso WHERE YEAR(DataDoRegistro) = @Ano AND MONTH(DataDoRegistro) = @Mes;", new { @Mes = m.Month, @Ano = m.Year });
                    r.Add(h);
                }
            }

            return r.OrderByDescending(x => x.ReferenciaDateTime).ToList();


        }

        private string ObterNomeMes(int mes)
        {
            if (mes == 1) return "Janeiro";
            if (mes == 2) return "Fevereiro";
            if (mes == 3) return "Março";
            if (mes == 4) return "Abril";
            if (mes == 5) return "Maio";
            if (mes == 6) return "Junho";
            if (mes == 7) return "Julho";
            if (mes == 8) return "Agosto";
            if (mes == 9) return "Setembro";
            if (mes == 10) return "Outubro";
            if (mes == 11) return "Novembro";
            if (mes == 12) return "Dezembro";
            return "Erro";
        }
    }
}
