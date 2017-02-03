/******************************************************************************
** Descripción: Representa los servicios configurados por aplicación.
*******************************************************************************
** BITÁCORA:
*******************************************************************************
** Fecha:    	Autor:				Descripción:
** ----------	--------------		-------------------------------------------
** 2016-01-13	César Aguirre		Creación de la tabla
** 2016-01-22   Jeysson Guevara     Agregado campo nombre.
** 2016-01-22   Jeysson Guevara     Normalización de los nombres de los campos de la tabla.
*******************************************************************************/
CREATE TABLE [Notificacion].[ServiciosPorAplicacion] (
    [id_ServiciosPorAplicacion] INT                NOT NULL IDENTITY,
    [id_Applicacion]            INT                NOT NULL,
    [id_Servicio]               INT                NOT NULL,
	[nm_nombre]                 VARCHAR(200)       NOT NULL,
    [ds_Configuracion]          VARCHAR (MAX)      NULL,
    --
	[id_Creado]                 BIGINT             NULL,
    [dt_Creado]                 DATETIMEOFFSET (7) CONSTRAINT [DF_ServiciosPorAplicacion_dt_Creado] DEFAULT (sysdatetimeoffset()) NULL,
    [id_Modificado]             BIGINT             NULL,
    [dt_Modificado]             DATETIMEOFFSET (7) CONSTRAINT [DF_ServiciosPorAplicacion_dt_Modificado] DEFAULT (sysdatetimeoffset()) NULL,
    CONSTRAINT [PK_ServicesPerApplication] PRIMARY KEY CLUSTERED ([id_ServiciosPorAplicacion] ASC),
    CONSTRAINT [FK_ServiciosPorAplicacion_Aplicacion] FOREIGN KEY ([id_Applicacion]) REFERENCES [Notificacion].[Aplicaciones] ([id_Aplicacion]),
    CONSTRAINT [FK_ServiciosPorAplicacion_Servicios]  FOREIGN KEY ([id_Servicio]) REFERENCES [Notificacion].[Servicios] ([id_Servicio])
);

