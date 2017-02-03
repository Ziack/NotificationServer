DECLARE @id_plantilla INT = NULL,
		@dt DATETIME2(7) = dbo.fn_GetLocalDate()

SELECT @id_plantilla = id_plantilla FROM Notificacion.Plantillas WHERE nm_nombre = 'TripleD/Mail/NotificarAutorizacionExplicita'


EXEC Notificacion.pr_Plantillas_Save @id_plantilla, 'TripleD/Mail/NotificarAutorizacionExplicita', 'Email de autorización explícita de Documentos', '@model NotificationServer.Core.Senders.NotificationData
@using System.Linq

@functions {
    object Prop(string key){
        return Model.Properties.FirstOrDefault(p => p.Key == key).Value;
    }
}

<div>
    <p>
       Señor(a) @Prop("autorizado_nombre") @Prop("autorizado_apellido"),
    </p>
    <p>
      Por medio de la presente le informamos que el señor(a)
      <strong>@Prop("autorizador_nombre") @Prop("autorizador_apellido")</strong> le ha dado autorización para <strong>ver</strong> los siguientes documentos:
  </p>
  <ul>
    @foreach(var idDocumento in Model.Properties.Where(p => p.Key.StartsWith("0_")).Select(p => p.Value)){
    @: <li>@idDocumento</li>
    }
  </ul>
</div>', @dt, 1, @dt, 1