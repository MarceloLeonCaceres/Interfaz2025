using LogicaB;
using System;
using System.Windows.Forms;
using Utilitarios;

namespace AdminDispositivosBiometricos
{
    public static class ClsInforma
    {

        public delegate void InformaBitacora(string evento, DataGridView dataGrid, string equipo);
        public static void ReportaBitacoraInvoke(string mensaje, DataGridView dgv, string nombreReloj)
        {
            if (dgv.InvokeRequired)
            {
                var delegado = new InformaBitacora(ReportaBitacora);
                dgv.Invoke(delegado, mensaje, dgv);
            }
            else
            {
                ReportaBitacora(mensaje, dgv, nombreReloj);
            }
        }
        public static void ReportaBitacora(string evento, DataGridView dgvBitacora, string sEquipoActual)
        {
            dgvBitacora.Rows.Insert(0, dgvBitacora.Rows.Count + 1, evento, sEquipoActual, DateTime.Now.ToString());
            dgvBitacora.Refresh();
        }

        public static void notificaRespuestaBddBitacora(DataGridView dgvBitacora, string sn, string sEquipoActual, int respuesta, string msjExito, bool ambos = true, int orden = 0, int idProceso = 0)
        {
            clsLogicaSDK oLogAuditoria = new clsLogicaSDK();
            try
            {
                switch (respuesta)
                {
                    case -1024:
                        oLogAuditoria.RegistraLogEventoBdd(orden, sn, idProceso, sEquipoActual, "El equipo no está conectado");
                        if (ambos) ClsInforma.ReportaBitacoraInvoke("El equipo no está conectado", dgvBitacora, sEquipoActual);
                        break;
                    case 1:
                        oLogAuditoria.RegistraLogEventoBdd(orden, sn, idProceso, sEquipoActual, msjExito);
                        if (ambos) ClsInforma.ReportaBitacoraInvoke(msjExito, dgvBitacora, sEquipoActual);
                        break;
                    case 0:
                        oLogAuditoria.RegistraLogEventoBdd(orden, sn, idProceso, sEquipoActual, "No hay datos en el dispositivo!");
                        if (ambos) ClsInforma.ReportaBitacoraInvoke("No hay datos en el dispositivo!", dgvBitacora, sEquipoActual);
                        break;
                    case 2:
                        ClsInforma.ReportaBitacoraInvoke(msjExito, dgvBitacora, sEquipoActual);
                        break;
                    default:
                        oLogAuditoria.RegistraLogEventoBdd(orden, sn, idProceso, sEquipoActual, "Error en comunicación con el equipo, ErrorCode: " + respuesta.ToString());
                        if (ambos) ClsInforma.ReportaBitacoraInvoke("Error en comunicación con el equipo, ErrorCode: " + respuesta.ToString(), dgvBitacora, sEquipoActual);
                        break;
                }
            }
            catch (clsDataBaseException dbEx)
            {
                throw dbEx;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
