using SDK;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdminDispositivosBiometricos.Auxiliares
{
    public static class Mapper
    {
        public static List<SDKHelper.Employee> ConvertToEmployees(DataGridViewSelectedRowCollection selectedRows)
        {
            List<SDKHelper.Employee> empleados = new List<SDKHelper.Employee>();
            foreach (DataGridViewRow row in selectedRows)
            {
                SDKHelper.Employee empleado = new SDKHelper.Employee()
                {
                    pin = row.Cells["NumCA"].Value.ToString(),
                    name = row.Cells["NombreUsuario"].Value.ToString(),
                    password = row.Cells["mverifyPass"].Value.ToString(),
                    privilege = (short)row.Cells["emPrivilege"].Value,
                    cardNumber = row.Cells["cardNumber"].Value.ToString()
                };
                empleados.Add(empleado);
            }
            return empleados;
        }

        public static List<SDKHelper.Employee> ConvertToDesencriptedEmployees(DataGridViewSelectedRowCollection selectedRows)
        {
            List<SDKHelper.Employee> empleados = new List<SDKHelper.Employee>();
            Utilitarios.ClsEncriptacion encripter = new Utilitarios.ClsEncriptacion();

            foreach (DataGridViewRow row in selectedRows)
            {
                SDKHelper.Employee empleado = new SDKHelper.Employee()
                {
                    pin = row.Cells["NumCA"].Value.ToString(),
                    name = row.Cells["NombreUsuario"].Value.ToString(),
                    password = encripter.Desencripta(row.Cells["mverifyPass"].Value.ToString()),
                    privilege = (short)row.Cells["emPrivilege"].Value,
                    cardNumber = row.Cells["cardNumber"].Value.ToString()
                };
                empleados.Add(empleado);
            }
            return empleados;
        }
    }
}
