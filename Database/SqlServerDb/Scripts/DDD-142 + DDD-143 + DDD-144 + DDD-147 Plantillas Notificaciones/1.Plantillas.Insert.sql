DECLARE @dt DATETIME2(7) = dbo.fn_GetLocalDate()


PRINT 'Insertar plantilla Nuevo Documento'
EXEC [Notificacion].[pr_Plantillas_Save] @id_plantilla = NULL, @nm_nombre = 'TripleD/Mail/NuevoDocumento', @ds_descripcion = 'Notificar a un ciudadano de la recepción de un documento.', @ds_contenido = '@model NotificationServer.Core.Senders.NotificationData
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

PRINT 'Insertar plantilla Cargue de Sujetos'
EXEC [Notificacion].[pr_Plantillas_Save] @id_plantilla = NULL, @nm_nombre = 'TripleD/Mail/CargueSujetos', @ds_descripcion = 'Notificar a los administradores que los sujetos han sido cargados.', @ds_contenido = '@model NotificationServer.Core.Senders.NotificationData
@using System.Linq

@functions {
    object Prop(string key){
        return Model.Properties.FirstOrDefault(p => p.Key == key).Value;
    }
}

<div>
    <p>
       Se han cargado correctamente los @Prop("sujeto_tipo") y se han asociado a los ciudadanos correspondientes.
    </p>
    <p>
      Se hizo a traves de los archivos 
      <strong>@Prop("archivo_sujetos")</strong> y <strong>@Prop("archivo_ciudadanos")</strong>.
  </p>
  <p>
    Fecha de cargue @Prop("fecha_cargue")
  </p>
</div>', @dt_Creado = @dt, @id_Creado = 1, @dt_Modificado = @dt, @id_Modificado = 1

PRINT 'Insertar plantilla Quitar Sujeto'
EXEC [Notificacion].[pr_Plantillas_Save] @id_plantilla = NULL, @nm_nombre = 'TripleD/Mail/QuitarSujeto', @ds_descripcion = 'Notificación cuando ha sido retirado un sujeto de un usuario', @ds_contenido = '@model NotificationServer.Core.Senders.NotificationData
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
      Por medio de la presente le informamos que el
      <em>@Prop("sujeto_tipo")</em> con <strong>@Prop("sujeto_tipo_identificacion") @Prop("sujeto_numero_identificacion")</strong> ya no se encuentra a su nombre.
  </p>
</div>', @dt_Creado = @dt, @id_Creado = 1, @dt_Modificado = @dt, @id_Modificado = 1

PRINT 'Insertar plantilla Nuevo Sujeto'
EXEC [Notificacion].[pr_Plantillas_Save] @id_plantilla = NULL, @nm_nombre = 'TripleD/Mail/NuevoSujeto', @ds_descripcion = 'Notificación de que un nuevo sujeto ha sido agregado al usuario.', @ds_contenido = '@model NotificationServer.Core.Senders.NotificationData
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
      Por medio de la presente le informamos que ha sido registrado el 
      <em>@Prop("sujeto_tipo")</em> con <strong>@Prop("sujeto_tipo_identificacion") @Prop("sujeto_numero_identificacion")</strong> a su nombre en nuestro sistema.
  </p>
</div>', @dt_Creado = @dt, @id_Creado = 1, @dt_Modificado = @dt, @id_Modificado = 1

PRINT 'LISTO'