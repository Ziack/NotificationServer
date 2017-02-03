CREATE PROCEDURE [Notificacion].pr_Notificaciones_Save
	@nm_nombre_aplicacion VARCHAR(50)             ,
    @nm_from              VARCHAR(60)             ,
    @ds_subject           VARCHAR(200)            ,
    @ds_json_to           VARCHAR(MAX)            ,
    @ds_json_propiedades  VARCHAR(MAX)            ,
    @id_Usuario           INT
AS BEGIN
	
	DECLARE @uid_notificacion UNIQUEIDENTIFIER = NEWID();

	INSERT INTO [Notificacion].Notificaciones
	(
		uid_notificacion    ,
		nm_nombre_aplicacion,
		nm_from             ,
		ds_subject          ,
		ds_json_to          ,
		ds_json_propiedades ,
		id_Creado,
		id_Modificado
	)VALUES(
		@uid_notificacion    ,
		@nm_nombre_aplicacion,
		@nm_from             ,
		@ds_subject          ,
		@ds_json_to          ,
		@ds_json_propiedades ,
		@id_Usuario          ,
		@id_Usuario
	)
		
	SELECT @uid_notificacion
END