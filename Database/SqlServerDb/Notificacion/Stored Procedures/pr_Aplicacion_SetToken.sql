CREATE PROCEDURE Notificacion.[pr_Aplicacion_SetToken]
	@ds_Titulo      VARCHAR (50), 
	@ds_Token       NVARCHAR(MAX)
AS BEGIN
	UPDATE Notificacion.Aplicaciones
	SET ds_Token = @ds_Token,
		id_Modificado = 1,
		dt_Modificado = dbo.fn_GetLocalDate()
	WHERE ds_Titulo = @ds_Titulo

END
