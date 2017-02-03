/******************************************************************************
** Descripción: Servicio Save.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-08	César Aguirre		Creación del procedimiento
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_Servicio_Save]
	@nm_Nombre VARCHAR(50),
	@ds_Descripcion VARCHAR(200),
    @ds_Host VARCHAR(500),
    @nm_Puerto INT,
    @ds_Usuario VARCHAR(50),
    @ds_Password VARCHAR(50),
    @id_Creado BIGINT,
	@ds_Configuracion VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1
				FROM [Notificacion].[Servicios]
				WHERE nm_Nombre = @nm_Nombre)
	BEGIN
		DECLARE @id_ErrorNumber VARCHAR(10);
		SET @id_ErrorNumber = 'M50002';
		RAISERROR (@id_ErrorNumber, 16, 2)
		RETURN
	END
	INSERT INTO [Notificacion].[Servicios]
           ([nm_Nombre]
           ,[ds_Descripcion]
           ,[ds_Host]
           ,[nm_Puerto]
           ,[ds_Usuario]
           ,[ds_Password]
		   ,[ds_Configuracion]
           ,[id_Creado]
           ,[dt_Creado]
           ,[id_Modificado]
           ,[dt_Modificado])
     VALUES
           (@nm_Nombre
           ,@ds_Descripcion
           ,@ds_Host
           ,@nm_Puerto
           ,@ds_Usuario
           ,@ds_Password
		   ,@ds_Configuracion
           ,@id_Creado
           ,SYSDATETIMEOFFSET()
           ,@id_Creado
           ,SYSDATETIMEOFFSET())
END