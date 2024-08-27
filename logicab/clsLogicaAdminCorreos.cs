using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosB;
using System.Data;

namespace LogicaB
{
    public static class clsLogicaAdminCorreos
    {
        public static DataTable dtRetornaCorreos()
        {
            return DatosB.clsDatosAdminCorreos.dtRetornaCorreos();
        }

        public static void AgregaCorreoNuevo()
        {
            clsDatosAdminCorreos.AgregaCorreoNuevo("direccion@ejemplo.com");
        }

        public static void EliminaCorreo(int idCorreo)
        {
            clsDatosAdminCorreos.EliminaCorreo(idCorreo);
        }

        public static void EditaCorreo(int idCorreo, string sCorreo)
        {
            clsDatosAdminCorreos.EditaCorreo(idCorreo, sCorreo);
        }

        public static List<string> lstCorreos()
        {
            DataTable dt = DatosB.clsDatosAdminCorreos.dtRetornaCorreos();
            List<string> listaCorreos = new List<string>();           
            
            foreach(DataRow fila in dt.Rows)
            {
                listaCorreos.Add(fila[1].ToString());
            }
            return listaCorreos;
        }

    }
}
