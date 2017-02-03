-- CORRER ESTO EN REPEXT! --

DECLARE @url_notificador VARCHAR(500) = ''-- Poner aquí la url del notificador --
DECLARE @token VARCHAR(500) = ''-- Poner aquí el token de seguridad del notificador --

UPDATE PLColabEmision.Definiciones SET
json_datos_contexto_ds = REPLACE(json_datos_contexto_ds, @url_notificador, @url_notificador + '#' + @token)