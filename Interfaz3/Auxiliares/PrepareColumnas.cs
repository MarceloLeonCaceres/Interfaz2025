using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace AdminDispositivosBiometricos
{
    public static class PrepareColumnas
    {

        public static DataTable SetColumnas(DataTable dt_Marcaciones, DataGridView dgvUserinfo)
        {
            // columnas Userinfo
            dgvUserinfo.Columns.Clear();
            dgvUserinfo.Columns.Add("sEnrollNumber", "sEnrollNumber");
            dgvUserinfo.Columns.Add("bEnabled", "bEnabled");
            dgvUserinfo.Columns.Add("sName", "sName");
            dgvUserinfo.Columns.Add("sPassword", "sPassword");
            dgvUserinfo.Columns.Add("iPrivilege", "iPrivilege");
            dgvUserinfo.Columns.Add("sCardumber", "sCardumber");

            dt_Marcaciones.Columns.Add("User ID", System.Type.GetType("System.String"));
            dt_Marcaciones.Columns.Add("Verify Date", System.Type.GetType("System.DateTime"));
            dt_Marcaciones.Columns.Add("Verify Type", System.Type.GetType("System.Int32"));
            dt_Marcaciones.Columns.Add("Verify State", System.Type.GetType("System.Int32"));
            dt_Marcaciones.Columns.Add("WorkCode", System.Type.GetType("System.Int32"));
            dt_Marcaciones.Columns.Add("sn", System.Type.GetType("System.String"));

            return dt_Marcaciones;
        }

        public static DataTable UsuariosBiometrico()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("pin", System.Type.GetType("System.String"));
            dt.Columns.Add("name", System.Type.GetType("System.String"));
            dt.Columns.Add("password", System.Type.GetType("System.String"));
            dt.Columns.Add("privilege", System.Type.GetType("System.Int32"));
            dt.Columns.Add("cardNumber", System.Type.GetType("System.String"));
            dt.Columns.Add("enprivilege", System.Type.GetType("System.Int32"));

            return dt;
        }

        public static List<string> DatatableToList(DataTable dt, string nombreColumna)
        {
            List<string> lista = new List<string>();
            foreach(DataRow fila in dt.Rows)
            {
                lista.Add(fila[nombreColumna].ToString().Trim());
            }
            return lista;
        }
    }
}