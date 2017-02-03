CREATE PROCEDURE [Notificacion].[pr_Tags_Save]
	@uid_notificacion UNIQUEIDENTIFIER, 
	@ds_tags VARCHAR(500),
	@id_usuario INT
AS
BEGIN
	INSERT INTO Notificacion.Tags
	(
		[uid_notificacion]
		,[nm_tag]
		,[id_creado]
		,[id_modificado]
	) 
	SELECT @uid_notificacion, valor, 1, 1
	FROM dbo.fn_SplitString(@ds_tags, ',')
END
