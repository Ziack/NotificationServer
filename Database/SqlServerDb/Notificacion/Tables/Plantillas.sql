CREATE TABLE Notificacion.[Plantillas]
(
	[id_plantilla] INT NOT NULL IDENTITY, 
    [nm_nombre] VARCHAR(200) NOT NULL,
	[ds_Descripcion] VARCHAR (200) NULL,
    [ds_contenido] VARCHAR(MAX) NOT NULL, 
	-- 
    [id_Creado] INT NOT NULL, 
    [dt_Creado] DATETIME2 NOT NULL CONSTRAINT [DF_Plantilla_dt_creado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    [id_Modificado] INT NOT NULL, 
    [dt_Modificado] DATETIME2 NOT NULL CONSTRAINT [DF_Plantilla_dt_modificado] DEFAULT ([dbo].[fn_GetLocalDate]()),
    PRIMARY KEY ([id_plantilla]), 
    CONSTRAINT [UQ_Plantillas_nm_nombre] UNIQUE (nm_nombre)
)
