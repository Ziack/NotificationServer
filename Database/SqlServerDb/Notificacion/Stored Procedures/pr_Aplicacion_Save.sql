/******************************************************************************
** Descripción: Personas Save.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-08	César Aguirre		Creación del procedimiento
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_Aplicacion_Save]
	@ds_Titulo VARCHAR(50) = NULL,
	@ds_Descripcion VARCHAR(200) = NULL, 
	@id_Creado BIGINT = NULL,
	@ds_Token NVARCHAR(MAX) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [Notificacion].[Aplicaciones] WHERE ds_Titulo = @ds_Titulo AND ds_Descripcion = @ds_Descripcion)
	BEGIN
		DECLARE @id_ErrorNumber VARCHAR(10);
		SET @id_ErrorNumber = 'M50001';
		RAISERROR (@id_ErrorNumber, 16, 2)
		RETURN
	END 
	INSERT INTO [Notificacion].[Aplicaciones]
           ([ds_Titulo]
		   ,[ds_Descripcion]
		   ,[id_Creado]
		   ,[dt_Creado]
		   ,[id_Modificado]
		   ,[dt_Modificado]
		   ,[ds_Token])
     VALUES
           (@ds_Titulo
           ,@ds_Descripcion
		   ,@id_Creado
		   ,SYSDATETIMEOFFSET()
		   ,@id_Creado
		   ,SYSDATETIMEOFFSET()
		   ,@ds_Token)
END