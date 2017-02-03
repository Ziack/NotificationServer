CREATE PROCEDURE Notificacion.[pr_EventosNotificaciones_Save]
	@uid_Notificacion      UNIQUEIDENTIFIER,
	@cd_NombreEstado       VARCHAR(50)     ,
	@ds_DescripcionEstado   VARCHAR(500)   ,
	@cd_NombreServicio     VARCHAR(50)     ,
	@ds_Error              VARCHAR(MAX)    ,
	@id_Usuario            INT             
AS BEGIN
	INSERT INTO Notificacion.EventosNotificaciones (
		uid_Notificacion    ,
		cd_NombreEstado     ,
		ds_DescripcionEstado,
		cd_NombreServicio   ,
		ds_Error            ,
		id_Creado,
		id_Modificado
	) VALUES (
		@uid_Notificacion    , 
		@cd_NombreEstado     , 
		@ds_DescripcionEstado,
		@cd_NombreServicio   , 
		@ds_Error            , 
		@id_Usuario          ,
		@id_Usuario
	)
END
