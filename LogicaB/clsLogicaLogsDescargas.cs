using DatosB;
using System;
using System.Data;

namespace LogicaB
{
    public static class ClsLogicaLogsDescargas
    {
        public static DataTable RetornaLogs(DateTime desde, DateTime hasta)
        {
            return ClsDatosLogsDescargas.RetornaLogs(desde, hasta.AddDays(1));
        }
    }
}
