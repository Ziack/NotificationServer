/******************************************************************************
** Descripción: Selecciona las configuraciones de los servicios dados disponibles 
** para una aplicación
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-22   Jeysson Guevara     Buscar por nombre del ServicioPorAplicacion en vez de por el nombre del servicio.
*******************************************************************************/
CREATE PROCEDURE [Notificacion].[pr_NotificacionesConfiguracion_List]
	@ds_nombreAplicacion VARCHAR(50),
	@ds_Servicios VARCHAR(500)
AS BEGIN

	DECLARE	@id_aplicacion AS INT

	SELECT @id_aplicacion = a.id_Aplicacion
	FROM Notificacion.Aplicaciones a
	WHERE a.ds_Titulo = @ds_nombreAplicacion

	SELECT 
		sa.id_servicio		   ,
		sa.nm_Nombre           ,
		s.[ds_Host]            ,
		s.[nm_Puerto]          ,
		s.[ds_Usuario]         ,
		s.[ds_Password]        ,
		s.[ds_Configuracion] AS  ds_ConfiguracionBase, 
		sa.[ds_Configuracion] AS ds_ConfiguracionSobreescrita
	FROM dbo.fn_SplitString(@ds_Servicios, ',')
	JOIN Notificacion.ServiciosPorAplicacion sa ON sa.nm_Nombre = valor
	INNER JOIN Notificacion.Servicios s ON s.id_Servicio = sa.id_Servicio
	WHERE sa.id_Applicacion = @id_aplicacion
END