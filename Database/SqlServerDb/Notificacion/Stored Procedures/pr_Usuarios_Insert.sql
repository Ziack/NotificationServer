CREATE PROCEDURE Notificacion.[pr_Usuarios_Insert] 
    @nombre_cd  VARCHAR(50),
    @clave_cd   NVARCHAR(200),
    @uid_cd     UNIQUEIDENTIFIER,
	@id_UsuarioCreador BIGINT
AS BEGIN
	INSERT INTO Notificacion.Usuarios (
		nombre_cd,
		clave_cd ,
		uid_cd   ,
		id_Creado,
		id_Modificado
	) VALUES (
		@nombre_cd,
		@clave_cd,
		@uid_cd,
		@id_UsuarioCreador,
		@id_UsuarioCreador
	)
END
