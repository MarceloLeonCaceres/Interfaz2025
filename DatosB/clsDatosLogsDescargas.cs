using System;
using System.Data;

namespace DatosB
{
    public static class ClsDatosLogsDescargas
    {
        public static DataTable RetornaLogs(DateTime desde, DateTime hasta)
        {
            string rangoFechas = $"'{desde:dd/MM/yyyy}' AND '{hasta:dd/MM/yyyy}'";
            string query = $@"SELECT  Operator, LogTime, sn, LogDescr as [Nombre Reloj/Acción], LogDetailed as Descripcion
                FROM da_DetalleDescarga
                WHERE LogTime Between {rangoFechas}
                ORDER BY LogTime desc";
            return ConexionDatos.ClsAccesoDatos.RetornaDataTable(query);
        }
    }
}
