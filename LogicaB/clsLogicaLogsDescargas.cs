using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DatosB;

namespace LogicaB
{
    public static class clsLogicaLogsDescargas
    {
        public static DataTable retornaLogs(int numLogs)
        {
            return clsDatosLogsDescargas.retornaLogs(numLogs);
        }
    }
}
