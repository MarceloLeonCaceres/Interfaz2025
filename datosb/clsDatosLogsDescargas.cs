using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ConexionDatos;

namespace DatosB
{
    public static class clsDatosLogsDescargas
    {
        public static DataTable retornaLogs(int numLogs)
        {
            string query = "SELECT TOP " + numLogs.ToString() + @" idProceso, LogTime, sn, LogDescr as [Nombre Reloj], LogDetailed as Descripcion
                FROM da_DetalleDescarga
                ORDER BY LogTime desc";
            return ConexionDatos.ClsAccesoDatos.RetornaDataTable(query);
        }
    }
}
