/******************************************************************************
** Descripción: Consulta de Planillas.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-14	María Zabaleta		Creación del procedimiento
*******************************************************************************/

CREATE PROCEDURE [Notificacion].[pr_Plantillas_Save]
	 @id_plantilla INT = NULL
    ,@nm_nombre VARCHAR(200) = NULL
	,@ds_descripcion VARCHAR(200) = NULL
	,@ds_contenido VARCHAR(MAX) = NULL
	,@dt_Creado DATETIME2(7) = NULL
	,@id_Creado INT = NULL
	,@dt_Modificado DATETIME2(7) = NULL
	,@id_Modificado INT = NULL

AS

BEGIN
	IF (@id_plantilla <> 0)
	BEGIN
		UPDATE [Notificacion].[Plantillas]
		SET	nm_nombre = @nm_nombre
			,ds_descripcion = @ds_descripcion
			,ds_contenido = @ds_contenido
			,dt_Modificado = @dt_Modificado
			,id_Modificado = @id_Modificado
		WHERE id_plantilla = @id_plantilla;
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT 1
				FROM [Notificacion].[Plantillas]
				WHERE nm_Nombre = @nm_Nombre)
		BEGIN
			DECLARE @id_ErrorNumber VARCHAR(10);
			SET @id_ErrorNumber = 'M50003';
			RAISERROR (@id_ErrorNumber, 16, 2)
			RETURN
		END

		INSERT INTO [Notificacion].[Plantillas](
			nm_nombre
			,ds_descripcion
			,ds_contenido
			,dt_Creado
			,id_Creado
			,dt_Modificado
			,id_Modificado
		) 
		VALUES( 
			@nm_nombre
			,@ds_descripcion
			,@ds_contenido
			,@dt_Creado
			,@id_Creado
			,@dt_Modificado
			,@id_Modificado
		)
	END
END