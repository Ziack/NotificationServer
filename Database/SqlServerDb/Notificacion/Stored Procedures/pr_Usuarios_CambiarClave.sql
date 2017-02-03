CREATE PROCEDURE Notificacion.[pr_Usuarios_CambiarClave]
	@nombre_cd  VARCHAR(50),
	@clave_cd   NVARCHAR(200)
AS BEGIN

	UPDATE Notificacion.Usuarios
	SET clave_cd = @clave_cd
	WHERE nombre_cd = @nombre_cd

END