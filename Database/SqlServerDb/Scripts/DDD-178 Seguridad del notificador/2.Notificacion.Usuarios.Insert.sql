DECLARE @uid UNIQUEIDENTIFIER = NEWID()

EXEC Notificacion.pr_Usuarios_Insert
	@nombre_cd = 'host',
	@clave_cd = N'忂躃︐ｆ﹕컫থ࿜㢐⇭䄂녛໩섭钌᷂㉭৖꼒䕷Ȏ뉶褴೻꟱䢥絈촰撋얤馴', --Equivale a: abc123$
	@uid_cd     = @uid,
	@id_UsuarioCreador = 1
