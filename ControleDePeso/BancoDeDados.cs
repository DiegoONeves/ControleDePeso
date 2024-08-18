using ControleDePeso.Entidades;
using ControleDePeso.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ControleDePeso
{
    public class BancoDeDados
    {
        public string ConnectionString { get; set; } = "";
        public BancoDeDados(IConfiguration config) => ConnectionString = config.GetConnectionString("DefaultConnection");


        public async Task<List<HistoricoDashBoardViewModel>> BuscarHistoricoAsync(int ultimosAno)
        {

            List<HistoricoDashBoardViewModel> r = [];
            List<int> anos = [];
            int anoAtual = DateTime.Now.Year;
            int anoInicial = DateTime.Now.AddYears(-ultimosAno).Year;

            for (int i = anoInicial; i <= anoAtual; i++)
                anos.Add(i);

            anos = anos.OrderByDescending(x => x).ToList();

            foreach (var ano in anos)
            {
                var l = new HistoricoDashBoardViewModel { Ano = ano };

                using (var conn = new SqlConnection(ConnectionString))
                {
                    var tempo = await conn.QueryAsync<HistoricoDashBoardViewModel>(@"
                                            SELECT (SELECT AVG(Peso) FROM PesoHistorico WHERE YEAR(DataDoRegistro) = @Ano) AS Peso, (SELECT AVG(Passos) FROM PassosHistorico WHERE YEAR(DataDoRegistro) = @Ano) AS Passos",
                                            new { @Ano = ano });



                    var primeiro = tempo.First();

                    if (primeiro.Peso > 0)
                    {
                        l.Peso = primeiro.Peso;
                        l.Passos = primeiro.Passos;
                    }

                    r.Add(l);
                }
            }

            return r;

        }

        public async Task<MediaMovel> BuscarMediaMovelSemanalAsync()
        {
            MediaMovel r = new();
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
                r.MediaMovelPeso1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                //ÚLTIMOS 14 DIAS - PESO
                r.MediaMovelPeso2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                //ÚLTIMOS 21 DIAS - PESO
                r.MediaMovelPeso3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                //ÚLTIMOS 28 DIAS - PESO
                r.MediaMovelPeso4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });


                //ÚLTIMOS 7 DIAS - PASSOS
                r.MediaMovelPassos1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as INT) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                //ÚLTIMOS 14 DIAS - PASSOS
                r.MediaMovelPassos2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as INT) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                //ÚLTIMOS 21 DIAS - PASSOS
                r.MediaMovelPassos3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as INT) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                //ÚLTIMOS 28 DIAS - PASSOS
                r.MediaMovelPassos4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as INT) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

            }

            return r;



        }

        public async Task<MediaMovel> BuscarMediaMovelTrimestralAsync()
        {

            MediaMovel r = new();
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
                //PESO
                r.MediaMovelPeso1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovelPeso2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovelPeso3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovelPeso4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

                //PASSOS
                r.MediaMovelPassos1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovelPassos2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovelPassos3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovelPassos4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

            }

            return r;
        }

        public async Task<MediaMovel> BuscarMediaMovelQuinzenalAsync()
        {
            MediaMovel r = new();
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
                //peso
                r.MediaMovelPeso1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovelPeso2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovelPeso3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovelPeso4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });


                //passos
                r.MediaMovelPassos1 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasPrimeiroPeriodo[1], @DataFinal = datasPrimeiroPeriodo[0] });

                r.MediaMovelPassos2 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasSegundoPeriodo[1], @DataFinal = datasSegundoPeriodo[0] });

                r.MediaMovelPassos3 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasTerceiroPeriodo[1], @DataFinal = datasTerceiroPeriodo[0] });

                r.MediaMovelPassos4 = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE DataDoRegistro BETWEEN @DataInicial AND @DataFinal", new { @DataInicial = datasQuartoPeriodo[1], @DataFinal = datasQuartoPeriodo[0] });

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
                    h.MediaDePeso = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Peso) as DECIMAL(18,1)) as VARCHAR(255)) + ' Kg' FROM PesoHistorico WHERE YEAR(DataDoRegistro) = @Ano AND MONTH(DataDoRegistro) = @Mes;", new { @Mes = m.Month, @Ano = m.Year });
                    //Passos
                    h.MediaDePassos = await conn.ExecuteScalarAsync<string>(@"SELECT CAST(CAST(AVG(Passos) as int) as VARCHAR(255)) + ' passos' FROM PassosHistorico WHERE YEAR(DataDoRegistro) = @Ano AND MONTH(DataDoRegistro) = @Mes;", new { @Mes = m.Month, @Ano = m.Year });
                    r.Add(h);
                }
            }

            return r.OrderByDescending(x => x.ReferenciaDateTime).ToList();

        }

        public async Task CadastrarPesoAsync(PesagemViewModel model)
        {
            var peso = new PesoHistorico { DataDoRegistro = model.DataDoRegistro.ToDateTime(TimeOnly.MinValue), Peso = model.Peso };
            using var conn = new SqlConnection(ConnectionString);
            await conn.ExecuteAsync(@"insert into PesoHistorico (DataDoRegistro,Peso) values (@DataDoRegistro,@Peso)", peso);
        }

        public async Task GravarPassosAsync(GravarPassosViewModel model)
        {
            var passos = new PassosHistorico { DataDoRegistro = model.DataDoRegistro.ToDateTime(TimeOnly.MinValue), Passos = model.Passos };
            using var conn = new SqlConnection(ConnectionString);
            await conn.ExecuteAsync(@"insert into PassosHistorico (DataDoRegistro,Passos) values (@DataDoRegistro,@Passos)", passos);
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
