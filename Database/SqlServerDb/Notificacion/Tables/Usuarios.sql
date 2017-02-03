CREATE TABLE Notificacion.[Usuarios]
(
	[usuario_id] BIGINT NOT NULL  IDENTITY, 
    [nombre_cd] VARCHAR(50) NOT NULL , 
    [clave_cd] NVARCHAR(200) NOT NULL, 
    [uid_cd] UNIQUEIDENTIFIER NOT NULL,
	[id_Creado]      BIGINT             NOT NULL,
	[dt_Creado]      DATETIMEOFFSET (7) CONSTRAINT [DF_Notificacion_Usuarios_dt_Creado] DEFAULT (sysdatetimeoffset()) NOT NULL,
	[id_Modificado]  BIGINT             NOT NULL,
	[dt_Modificado]  DATETIMEOFFSET (7) CONSTRAINT [DF_Notificacion_Usuarios_dt_Modificado] DEFAULT (sysdatetimeoffset()) NOT NULL, 
    PRIMARY KEY ([usuario_id] DESC), 
    CONSTRAINT [UQ_Usuarios_nombre_cd] UNIQUE (nombre_cd), 
    CONSTRAINT [UQ_Usuarios_uid] UNIQUE ([uid_cd])
)
