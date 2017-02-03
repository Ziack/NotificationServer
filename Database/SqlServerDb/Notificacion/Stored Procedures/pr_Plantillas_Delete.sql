/******************************************************************************
** Descripción: Consulta de Planillas.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-14	María Zabaleta		Creación del procedimiento
*******************************************************************************/

CREATE PROCEDURE [Notificacion].[pr_Plantillas_Delete]
	 @id_plantilla INT = NULL

AS

BEGIN
	DECLARE @id_ErrorNumber INT

	BEGIN TRY
		SET @id_ErrorNumber = 51093;

		DELETE FROM [Notificacion].[Plantillas]
		WHERE id_plantilla = @id_plantilla;
	END TRY

	BEGIN CATCH
		IF ( @id_ErrorNumber = 51093 )
		BEGIN
			RAISERROR (@id_ErrorNumber, 16, 2)
			RETURN
		END
	END CATCH
END