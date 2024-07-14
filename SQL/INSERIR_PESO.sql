use ControleDePeso
go

declare @Peso decimal(18,2) = 118.0
declare @DataDaPesagem date = '2024-02-13'



insert into PesoHistorico (Peso,DataDoRegistro) values(@Peso,@DataDaPesagem)


--select * from PesoHistorico order by DataDoRegistro desc
--update PesoHistorico set DataDoRegistro = '2024-02-03' where Peso = 120.2 and DataDoRegistro = '2024-02-04'




