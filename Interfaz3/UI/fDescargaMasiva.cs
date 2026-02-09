using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilitarios;
using LogicaB;
using SDK;


namespace AdminDispositivosBiometricos
{
    public partial class fDescargaMasiva : Form
    {
        public SDK.SDKHelper reloj = new SDK.SDKHelper();
        public string sEquipoActual = "";
        public string sn = "";
        public int userAdmin = -1;
        bool bEnviaBitacora = true;

        public List<ClsReloj> listaRelojes = new List<ClsReloj>();

        public enum EnumModo
        {
            Automático,
            Manual
        }

        public EnumModo modo;

        public fDescargaMasiva()
        {
            InitializeComponent();
        }

        public void DescargaMarcacionesRelojes(List<ClsReloj> lstRelojes)
        {

            clsLogicaSDK oLog = new clsLogicaSDK();
            LogHelpers.ReportaNovedad("Inicia Descarga Relojes");
            clsIntString respuestaSsn = new clsIntString();
            respuestaSsn.respuesta = 0;
            int idProceso = oLog.iniciaProcesoMasivo(userAdmin);
            string sMensaje = "Inicio descarga masiva.";

            string lstSn = string.Join(",", lstRelojes.Select(r => r.sSN).ToList());
            oLog.RegistraLogEventoBdd(0, lstRelojes.Count.ToString(), idProceso, sMensaje, lstSn);

            oLog.InicializaTablasDescargaTemporales();

            foreach (ClsReloj reloj in lstRelojes)
            {
                sEquipoActual = reloj.sNombreReloj;
                respuestaSsn.sn = reloj.sSN;
                sn = reloj.sSN;
                sMensaje = $"Comunicándose con reloj {reloj.sSN}";
                LogHelpers.ReportaNovedad(sMensaje);
                oLog.RegistraLogEventoBdd(1, reloj.sSN, idProceso, reloj.sNombreReloj, sMensaje);
                ClsInforma.ReportaBitacoraInvoke(sMensaje, dgvBitacora, reloj.sNombreReloj);
                dgvBitacora.Refresh();
                int estadoConexion = this.reloj.sta_ConnectTCP(reloj.sIP.ToString(), reloj.iPuerto.ToString(), "0");
                ReportaConexionDesconex(oLog, estadoConexion, idProceso, bEnviaBitacora);
                dgvBitacora.Refresh();

                if (estadoConexion != 1)
                {
                    LogHelpers.ReportaNovedad("Siguiente reloj (-1)");
                    continue;
                }

                DataTable dt_Marcaciones = new DataTable("dt_Marcaciones");
                dt_Marcaciones = PrepareColumnas.SetColumnas(dt_Marcaciones, dgvUserinfo);

                gv_Attlog.AutoGenerateColumns = true;
                gv_Attlog.Columns.Clear();

                respuestaSsn = this.reloj.bio_LeeMarcaciones(dt_Marcaciones, dgvUserinfo);
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, reloj.sSN, reloj.sNombreReloj, respuestaSsn.respuesta, "Las marcaciones del reloj han sido leidas: " + dt_Marcaciones.Rows.Count.ToString(), bEnviaBitacora, 3, idProceso);
                dgvBitacora.Refresh();

                oLog.RegistraLogEventoBdd(4, sn, idProceso, reloj.sNombreReloj, "Comunicándose con reloj ");
                estadoConexion = this.reloj.sta_ConnectTCP(reloj.sIP.ToString(), reloj.iPuerto.ToString(), "0");
                ReportaConexionDesconex(oLog, estadoConexion, idProceso, bEnviaBitacora);
                dgvBitacora.Refresh();
                if (estadoConexion != -2)
                {
                    LogHelpers.ReportaNovedad("Siguiente reloj (-2)");
                    continue;
                }

                gv_Attlog.DataSource = dt_Marcaciones;
                if (dt_Marcaciones.Rows.Count == 0) continue;

                try
                {
                    GuardarMarcacionesMasivas(dt_Marcaciones, dgvUserinfo, sn, reloj.sNombreReloj, idProceso);
                    dgvBitacora.Refresh();
                }
                catch (Exception ex)
                {
                    LogHelpers.ReportaNovedad($"Siguiente reloj {ex.ToString()}");
                    oLog.RegistraLogEventoBdd(4, sn, idProceso, reloj.sNombreReloj, ex.ToString());
                    continue;
                }
            }

            LogHelpers.ReportaNovedad("Finaliza Descarga Relojes");
        }

        private void ReportaConexionDesconex(clsLogicaSDK oLog, int estadoConex, int idProceso, bool ambos = true)
        {
            string sMensaje = "";
            if (estadoConex == 1)
            {
                sMensaje = "Conectado con reloj";
                oLog.RegistraLogEventoBdd(2, sn, idProceso, sEquipoActual, sMensaje);
                if (ambos) ClsInforma.ReportaBitacora(sMensaje, dgvBitacora, sEquipoActual);
            }
            else if (estadoConex == -2)
            {
                sMensaje = "Desconectado con reloj";
                oLog.RegistraLogEventoBdd(5, sn, idProceso, sEquipoActual, sMensaje);
                if (ambos) ClsInforma.ReportaBitacora(sMensaje, dgvBitacora, sEquipoActual);
            }
            else
            {
                sMensaje = "No se pudo comunicar con el equipo. Error code: " + estadoConex.ToString();
                oLog.RegistraLogEventoBdd(9, sn, idProceso, sEquipoActual, sMensaje);
                EnviaCorreosError(idProceso, sEquipoActual);
                if (ambos) ClsInforma.ReportaBitacora(sMensaje, dgvBitacora, sEquipoActual);
            }
            dgvBitacora.Refresh();
        }

        public void btnDescargaMasiva_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DescargaMarcacionesRelojes(null);
            Cursor = Cursors.Default;
        }

        public void GuardarMarcacionesMasivas(DataTable dt_Marcaciones, DataGridView dgvUserinfo, string sn, string sEquipoActual, int idProceso)
        {
            DataTable dtNewMarcaciones = new DataTable();
            clsLogicaSDK oLog = new clsLogicaSDK();

            if (dt_Marcaciones.Rows.Count == 0)
            {
                oLog.RegistraLogEventoBdd(6, sn, idProceso, sEquipoActual, "No hay marcaciones en reloj");
                return;
            }
            try
            {
                oLog.guardaMarcacionesBulk(dt_Marcaciones, sn);
                oLog.GuardaUsuariosMasivos(dgvUserinfo);
                dtNewMarcaciones = clsLogicaSDK.GrabacionMarcacionesYUsuariosNuevosEnTablasDefinitivas(sn);
                string sMensaje = dtNewMarcaciones.Rows.Count == 1 ? "Se guardó 1 marcación nueva" : "Se guardaron " + dtNewMarcaciones.Rows.Count.ToString() + " marcaciones nuevas";
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, 1, sMensaje, bEnviaBitacora, 7, idProceso);
            }
            catch (clsLogicaException lerror)
            {
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, 1, lerror.logErrorDescription, bEnviaBitacora);
            }
            catch (Exception error)
            {
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, 1, error.Message, bEnviaBitacora);
            }
        }

        private void fDescargaMasiva_Load(object sender, EventArgs e)
        {

        }

        private const string pieCorreo = "Este fue un mensaje generado automáticamente por medio de una cuenta de correo sin lectura, por favor no lo responda. Si tiene alguna inquietud, por favor comuníquese con el Departamento de Recursos Humanos y/o Departamento de Tecnologías.";
        private void EnviaCorreosError(int idProceso, string nombreReloj)
        {
            
            StringBuilder sMensaje = new StringBuilder("Buenos días \n\n");
            sMensaje.Append( "Se presentó un error al intentar comunicarse con el equipo " + nombreReloj);
            sMensaje.Append("\n\nFecha Hora: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            sMensaje.Append( "\n\n" + pieCorreo);

            List<string> listaCorreos = clsLogicaAdminCorreos.lstCorreos();

            // Los parametros del correo están en el archivo AdminRelojesBio.exe.config
            //  Sí funciona correctamente
            try
            {
                ClsCorreo.clsLogicaCorreo oCorreo = new ClsCorreo.clsLogicaCorreo();    
                var remitente = oCorreo.ConfiguraCorreo();
                oCorreo.EnviaCorreo(listaCorreos, "Novedades en Descarga de Marcaciones", sMensaje.ToString());
            }
            catch (Exception ex)
            {
                clsLogicaSDK oLog = new clsLogicaSDK();
                oLog.RegistraLogEventoBdd(10, sn, idProceso, nombreReloj, 
                    "No se pudo enviar el correo de notificación de la novedad.\n" + String.Join(", ", listaCorreos));
            }
            
        }
    }
}
