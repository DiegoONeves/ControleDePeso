use ControleDeVida
go

declare @Peso decimal(18,2) = 115.6
declare @DataDaPesagem date = '2023-05-20'



insert into ControleDePeso (Peso,DataDoRegistro) values(@Peso,@DataDaPesagem)



