DECLARE @dt DATETIME2(7) = dbo.fn_GetLocalDate(),
		@id INT,
		@nombre VARCHAR(200) = 'TripleD/Mail/NuevoDocumento'

SELECT TOP 1 @id = id_plantilla FROM Notificacion.Plantillas WHERE nm_nombre = @nombre

PRINT 'Insertar plantilla Nuevo Documento'
EXEC [Notificacion].[pr_Plantillas_Save] @id_plantilla = @id, @nm_nombre = @nombre, @ds_descripcion = 'Notificar a un ciudadano de la recepción de un documento.', @ds_contenido = '@model NotificationServer.Core.Senders.NotificationData
@using System.Linq

@functions {
    object Prop(string key){
        return Model.Properties.FirstOrDefault(p => p.Key == key).Value;
    }
}

<div>
    <p>
       Señor(a) @Prop("ciudadano_nombre") @Prop("ciudadano_apellido"),
    </p>
    <p>
      Le ha llegado un nuevo documento <em>@Prop("tipo_documento")</em> con número <strong>@Prop("numero_documento")</strong>
  </p>
</div>', @dt_Creado = @dt, @id_Creado = 1, @dt_Modificado = @dt, @id_Modificado = 1