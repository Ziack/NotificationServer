CREATE PROCEDURE Notificacion.[pr_Adjuntos_Save]
    @uid_notificacion UNIQUEIDENTIFIER, 
    @nm_nombre        VARCHAR(300), 
    @bin_datos        VARBINARY(MAX), 
	-- 
    @id_usuario       INT
AS BEGIN
	INSERT INTO Notificacion.Adjuntos
	(
		uid_notificacion,
		nm_nombre       ,
		bin_datos       ,
		id_Creado,
		id_Modificado      
	) VALUES (
		@uid_notificacion,
		@nm_nombre       ,
		@bin_datos       ,
		
		@id_usuario      ,
		@id_usuario      
	)
END
