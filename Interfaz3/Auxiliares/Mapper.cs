using SDK;
using System;
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

            foreach (DataGridViewRow row in selectedRows)
            {
                if (row.IsNewRow) continue;

                Utilitarios.ClsEncriptacion encripter = new Utilitarios.ClsEncriptacion();

                string idEmpleado = row.Cells["NumCA"]?.Value?.ToString() ?? "Desconocido";
                string nombreEmpleado = row.Cells["NombreUsuario"]?.Value?.ToString() ?? "Sin Nombre";
                object rawPass = row.Cells["mverifyPass"].Value;

                var rawPrivilege = row.Cells["emPrivilege"].Value;
                int privilege = 0;

                if (rawPrivilege != null && rawPrivilege != DBNull.Value)
                {
                    privilege = Convert.ToInt32(rawPrivilege);
                }

                try
                {
                    var empleado = new SDKHelper.Employee()
                    {
                        pin = idEmpleado,
                        name = nombreEmpleado,
                        password = (rawPass == null || rawPass == DBNull.Value ||
                string.IsNullOrWhiteSpace(rawPass.ToString()))
                ? ""
                : encripter.Desencripta(rawPass.ToString().Replace(" ", "+")),
                        privilege = privilege,
                        cardNumber = row.Cells["cardNumber"].Value?.ToString()?.Trim() ?? ""
                    };
                    empleados.Add(empleado);
                }
                catch (System.Exception ex)
                {
                    throw;
                }
            }
            return empleados;
        }
    }
}
