truncate table CheckInOut
select count(*) from CHECKINOUT

truncate table USERINFO
select count(*) from USERINFO

select * from SystemLog
order by LogTime desc

select Top 100 idProceso, LogTime, sn, LogDescr as [Nombre Reloj], LogDetailed as Descripcion
from da_DetalleDescarga
order by LogTime desc

select * from da_ProcesosDescarga

select * from da_Parametros

/*
Adicionalmente, no se pudo enviar el correo de notificación de la novedad. 
ultimo@outlook.com, marcelo.leon.c@ejemplo.com
*/