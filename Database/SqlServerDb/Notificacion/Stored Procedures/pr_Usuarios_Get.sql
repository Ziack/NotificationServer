CREATE PROCEDURE Notificacion.[pr_Usuarios_Get]
	@nombre_cd  VARCHAR(50),
	@clave_cd   NVARCHAR(200),
	@uid_cd     UNIQUEIDENTIFIER
AS BEGIN
	
	IF @nombre_cd IS NULL
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE u.uid_cd = @uid_cd
	ELSE IF @clave_cd IS NULL
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE u.nombre_cd = @nombre_cd
	ELSE 
		SELECT TOP 1 
			u.nombre_cd,
			u.clave_cd,
			u.uid_cd,
			u.usuario_id
		FROM Notificacion.Usuarios u
		WHERE 
			u.nombre_cd = @nombre_cd
			AND u.clave_cd = @clave_cd

END
