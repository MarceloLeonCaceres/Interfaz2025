using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilitarios;
using SDK;
using LogicaB;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using AdminDispositivosBiometricos.Helper;
using static SDK.SDKHelper;
using AdminDispositivosBiometricos.Auxiliares;

namespace AdminDispositivosBiometricos
{
    public partial class fControlReloj : Form
    {

        #region Inicializar
        public Utilitarios.ClsReloj datosReloj = new ClsReloj();

        public SDK.SDKHelper reloj = new SDK.SDKHelper();
        public string sEquipoActual = "";
        public string sn = "";
        public int userAdmin = -1;
        bool bEnviaBitacora = true;

        public List<string> RelojesValidos { get; set; } = new List<string>();

        DateTime fechaVigencia = new DateTime(2021, 12, 31);

        Task tarea;
        CancellationTokenSource _cancellation = new CancellationTokenSource();
        CancellationToken token;         

        public fControlReloj()
        {
            InitializeComponent();
        }

        public fControlReloj(ClsReloj datosRelojFake)
        {
            datosReloj = datosRelojFake;
            InitializeComponent();
        }
        private void fControlReloj_Load(object sender, EventArgs e)
        {
            dgvNombresUsuarios.AutoGenerateColumns = false;

            try
            {
                sEquipoActual = datosReloj.sNombreReloj;
                this.Text = "Administración del Equipo: '" + sEquipoActual + "'";
                sn = datosReloj.sSN;
                tsbConectar_Click(null, null);
                //DateTime fechaVigencia = oSeguridadFechaVigencia.retornaFechaVigencia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error en Inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        #endregion

        #region Invoke de Tareas
        int _avance = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ActualizaProgressBarInvoke(_avance, PrgSTA);
            _avance += 10;
            if (_avance >= 100)
                _avance = 0;

        }
        
        
        private delegate void ActivaToolStrip(bool _bool, ToolStrip strip);
        private void ActivaBotonInvoke(bool si_no, ToolStrip toolStrip  )
        {
            if (toolStrip.InvokeRequired)
            {
                var delegado = new ActivaToolStrip(ActivaBoton);
                toolStrip.Invoke(delegado, si_no, toolStrip);
            }
            else
            {
                ActivaBoton(si_no, toolStrip);
            }
        }
        private void ActivaBoton(bool act_des, ToolStrip ts)
        {
            ts.Enabled = act_des;
        }

        private delegate void DelegadoProgressBar(int iAvance, ProgressBar progressBar);
        private void ActualizaProgressBarInvoke(int avance, ProgressBar Barra)
        {
            if (Barra.InvokeRequired)
            {
                var delegado = new DelegadoProgressBar(ActualizaProgressBar);
                Barra.Invoke(delegado, avance, Barra);
            }
            else
            {
                ActualizaProgressBar(avance, Barra);
            }
        }
        private void ActualizaProgressBar(int j, ProgressBar progressBar)
        {
            progressBar.Value = j;
            progressBar.Refresh();
        }
        private delegate void DelegadoDataGridView(DataTable data, DataGridView dataGrid);
        private void ActualizaNewDgvMarcacionesInvoke(DataTable dt, DataGridView dgv)
        {
            if(dgv.InvokeRequired)
            {
                var delegado = new DelegadoDataGridView(ActualizaNewDgvMarcaciones);
                dgv.Invoke(delegado, dt, dgv);
            }
            else
            {
                ActualizaNewDgvMarcaciones(dt, dgv);
            }

        }
        private void ActualizaNewDgvMarcaciones(DataTable dt, DataGridView dgv)
        {
            dgv.AutoGenerateColumns = true;
            dgv.DataSource = dt;
        }
        #endregion

        #region Accesorios

        private bool confirmaEliminación(string msjPregunta)
        {
            DialogResult oRespuesta = MessageBox.Show(msjPregunta + "\n\n" + "Esta acción no puede deshacerse.", "Confirmar borrado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return oRespuesta == DialogResult.Yes;
        }

        #endregion

        #region Botones Menu 

        private void tsbAvanzado_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tpAvanzado;
        }

        private void tsbConectar_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tpCapacidad;
            btnTCPConnect_Click(null, null);
        }

        private void tsbDesconectar_Click(object sender, EventArgs e)
        {
            btnTCPConnect_Click(null, null);
        }
        #endregion

        #region Comunicación
        private void btnTCPConnect_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;

            ClsInforma.ReportaBitacora("Comunicándose con reloj " + sEquipoActual, dgvBitacora, sEquipoActual);
            dgvBitacora.Refresh();

            int ret = reloj.sta_ConnectTCP(datosReloj.sIP.ToString(), datosReloj.iPuerto.ToString(), "0");

            if (reloj.GetConnectState())
            {
                reloj.sta_getBiometricType();
            }
            if (ret == 1)
            {
                int resultado = reloj.sta_GetDeviceTime(lbDeviceTime, out DateTime fechaActual);

                ClsInforma.ReportaBitacora("Conectado con reloj " + sEquipoActual, dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();
                this.btnGetSystemInfo.Enabled = true;
                this.btnGetDataInfo.Enabled = true;

                if(getDeviceInfo(reloj).Item1 == false)
                {
                    string mensaje = "No es un equipo compatible con el sistema. \n";
                    mensaje += "'" + datosReloj.sNombreReloj + "', de serial No.: " + getDeviceInfo(reloj).Item2.ToString() + ".";
                    MessageBox.Show(mensaje, "Equipo no registrado.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                
                getCapacityInfo(reloj);

                aplicaConexionBotones(true);
                this.btnGetSystemInfo.Enabled = false;
                this.btnGetDataInfo.Enabled = false;
            }
            else if (ret == -2)
            {
                ClsInforma.ReportaBitacora("Desconectado con reloj " + sEquipoActual, dgvBitacora, sEquipoActual);
                sEquipoActual = "";
                aplicaConexionBotones(false);
                this.Close();
            }
            else 
            {
                string mensaje = "No se pudo establecer la comunicación con el dispositivo \n";
                mensaje += "'" + datosReloj.sNombreReloj + "', de dirección IP: " + datosReloj.sIP.ToString() + ".";
                MessageBox.Show(mensaje, "Error de comunicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            
            dgvBitacora.Refresh();
            Cursor = Cursors.Default;
        }

        private void aplicaConexionBotones(Boolean conecta)
        {
            if (conecta)
            {
                tsbConectar.Visible = false;
                tsbDesconectar.Enabled = true;
                tsbDescargar.Enabled = true;
            }
            else
            {
                this.btnGetSystemInfo.Enabled = false;
                this.btnGetDataInfo.Enabled = false;
                tsbConectar.Visible = true;
                tsbConectar.Enabled = true;
                tsbDesconectar.Enabled = false;
                tsbDescargar.Enabled = false;
            }
        }

        private void getCapacityInfo(SDKHelper reloj)
        {
            int adminCnt = 0;
            int userCount = 0;
            int fpCnt = 0;
            int recordCnt = 0;
            int pwdCnt = 0;
            int oplogCnt = 0;
            int faceCnt = 0;
            reloj.sta_GetCapacityInfo(out adminCnt, out userCount, out fpCnt, out recordCnt, out pwdCnt, out oplogCnt, out faceCnt);

            txtAdminCnt.Text = adminCnt.ToString();
            txtUserCnt.Text = userCount.ToString();
            txtFPCnt.Text = fpCnt.ToString();
            txtAttLogCnt.Text = recordCnt.ToString();
            txtPWDCnt.Text = pwdCnt.ToString();
            txtOpLogCnt.Text = oplogCnt.ToString();
            txtFaceCnt.Text = faceCnt.ToString();
        }

        private (bool, string) getDeviceInfo(SDKHelper reloj)
        {
            int idMaquina = 0;
            string sFirmver = "";
            string sMac = "";
            string sPlatform = "";
            string sSN = "";
            string sProductTime = "";
            string sDeviceName = "";
            int iFPAlg = 0;
            int iFaceAlg = 0;
            string sProducter = "";

            reloj.sta_GetDeviceInfo(out idMaquina, out sFirmver, out sMac, out sPlatform, out sSN, out sProductTime, out sDeviceName, out iFPAlg, out iFaceAlg, out sProducter);

            if (RelojesValidos.Contains(sSN) == false)
            {
                return (false, sSN);
            }
            ClsInforma.ReportaBitacora("Conectado con reloj", dgvBitacora, sEquipoActual);
            dgvBitacora.Refresh();

            //if (fPrincipal.ConfimaRelojEsValido(sSN) == false)
            //{

            //}

            txtFirmwareVer.Text = sFirmver;
            txtMac.Text = sMac;
            txtSerialNumber.Text = sSN;
            txtPlatForm.Text = sPlatform;
            txtDeviceName.Text = sDeviceName;
            txtFPAlg.Text = iFPAlg.ToString().Trim();
            txtFaceAlg.Text = iFaceAlg.ToString().Trim();
            txtManufacturer.Text = sProducter;
            txtManufactureTime.Text = sProductTime;
            txtSensorId.Text = idMaquina.ToString();
            return (true, sSN);
        }
        #endregion

        #region Descarga FP

        private List<string> lstEmpleadosChequedos(DataGridView dgv)
        {
            List<string> lstEmpleados = new List<string>();
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                if(fila.Cells["Chk"].Value != null) 
                    if((bool)fila.Cells["Chk"].Value)
                        lstEmpleados.Add(fila.Cells["Badgenumber"].Value.ToString());
            }
            return lstEmpleados;
        }


        List<string> lstEmpleadosChequedosDescarga = new List<string>();

        private bool ActualizaLstEmpleadosChequedosDescarga()
        {
            lstEmpleadosChequedosDescarga = lstEmpleadosChequedos(dgvUsuariosEnBase);
            lstEmpleadosChequedosDescarga.AddRange(lstEmpleadosChequedos(dgvUsuariosNuevos));
            if (lstEmpleadosChequedosDescarga.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un empleado para descargar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }
        private void BtnDescargarHuellas_Click(object sender, EventArgs e)
        {
            if (!ActualizaLstEmpleadosChequedosDescarga())
                return;

            Cursor = Cursors.WaitCursor;
            dgvLeeUsuarios.Rows.Clear();
            ClsInforma.ReportaBitacora("Inicia lectura de huellas del dispositivo", dgvBitacora, sEquipoActual);
            
            DataTable dtEmpleados = PrepareColumnas.UsuariosBiometrico();
            reloj.sta_GetAllUserFPInfo(dgvBitacora, PrgSTA, dgvLeeUsuarios, dtEmpleados, 0, 
                lstEmpleadosChequedosDescarga);

            if (dgvLeeUsuarios.RowCount == 0)
            {
                MessageBox.Show("No hay huellas para guardar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }
            
            try
            {
                ClsInforma.ReportaBitacora("Inicia grabación de huellas en base de datos", dgvBitacora, sEquipoActual);

                LogicaB.clsLogicaSDK oLogHuellas = new clsLogicaSDK();
                oLogHuellas.IniciaGrabacionHuellas();
                PrgSTA.Visible = true;
                PrgSTA.Value = 0;
                PrgSTA.Refresh();
                GuardaHuellasPorLoteEnTablasTemporales(oLogHuellas);
                ActualizaCreaDatosUsuarios(dtEmpleados, oLogHuellas);
                oLogHuellas.FinalizaGrabacionHuellas();

                int k = dgvLeeUsuarios.Rows.Count;
                string mensaje = k == 1 ? "Se ha grabado 1 huella" : "Se han grabado " + k.ToString() + " huellas";
                MessageBox.Show(mensaje, "Grabación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClsInforma.ReportaBitacora(mensaje, dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();

            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                PrgSTA.Visible = false;
                Cursor = Cursors.Default;
            }

        }

        private void GuardaHuellasPorLoteEnTablasTemporales(clsLogicaSDK oLogHuellas)
        {
            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
            
            for (int i = 0; i < dgvLeeUsuarios.Rows.Count; i++)
            {
                dgvTemporal.Add(dgvLeeUsuarios.Rows[i]);
                if ((i + 1) % 100 == 0 && dgvTemporal.Count > 0)
                {
                    oLogHuellas.GuardaHuellasEnTemporal(dgvTemporal);
                    dgvTemporal.Clear();
                    ActualizaProgressBarInvoke((int)(i * (100.0 / dgvLeeUsuarios.Rows.Count)), PrgSTA);
                }
            }
            if (dgvTemporal.Count > 0)
            {
                oLogHuellas.GuardaHuellasEnTemporal(dgvTemporal);
                ActualizaProgressBarInvoke(100, PrgSTA);
            }
        }

        private void ActualizaCreaDatosUsuarios(DataTable dtEmpleados, clsLogicaSDK oLogHuellas)
        {
            ActualizaProgressBarInvoke(0, PrgSTA);
            oLogHuellas.ActualizaCreaDatosUsuarios(dtEmpleados);
            ActualizaProgressBarInvoke(100, PrgSTA);
        }

        #endregion

        #region Carga FP
        private void btnTraerHuellasDeBdd_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            RecuperaHuellas();
            this.Cursor = Cursors.Default;
        }

        private void RecuperaHuellas()
        {
            dgvUserFP.DataSource = null;
            LogicaB.clsLogicaSDK oLogHuellas = new clsLogicaSDK();
            BindingSource bs = new BindingSource();
            DataTable dt = new DataTable();
            dt = oLogHuellas.RetornaHuellas(tsTxtBusqueda.Text);
            bs.DataSource = null;
            bs.DataSource = dt;
            dgvUserFP.DataSource = bs;
        }

        private void btnEnviarHuellasABiometrico_Click(object sender, EventArgs e)
        {
            if (dgvUserFP.RowCount == 0)
            {
                MessageBox.Show("No hay huellas para enviar al dispositivo.", "Atención", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int numeroHuellasAEnviar = dgvUserFP.SelectedRows.Count;
            if (numeroHuellasAEnviar == 0)
            {
                MessageBox.Show("Debe seleccionar las huellas para enviar al dispositivo.", "Atención", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Cursor = Cursors.WaitCursor;
            PrgSTA.Visible = true;
            PrgSTA.Value = 0;
            PrgSTA.Refresh();

            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
            int respuestaBiometrico = -1;

            ClsInforma.ReportaBitacora("Inicio envío de huellas a dispositivo", dgvBitacora, sEquipoActual);
            dgvBitacora.Refresh();

            Func<ProgressBar, List<DataGridViewRow>, int> EnvioUnoXUno = reloj.sta_SetAllUserFPInfo;
            Func<ProgressBar, List<DataGridViewRow>, int> EnvioBatch = reloj.sta_batch_SetAllUserFPInfo;

            if (cbBatchUpload.Checked)
            {
                EnviaDgvHuellasASdkMetodoElegido(dgvTemporal, ref respuestaBiometrico, numeroHuellasAEnviar, 
                    tamanioEnvioLote: 100,
                    formaEnvio: EnvioBatch);
            }
            else
            {
                EnviaDgvHuellasASdkMetodoElegido(dgvTemporal, ref respuestaBiometrico, numeroHuellasAEnviar, 
                    tamanioEnvioLote: 10,
                    formaEnvio: EnvioUnoXUno);
            }
            if (respuestaBiometrico == 1)
            {
                PrgSTA.Value = 100;
                PrgSTA.Refresh();
                MessageBox.Show("Las huellas seleccionadas han sido transferidas exitosamente al dispositivo", "Transferencia Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrgSTA.Visible = false;
            }

            Cursor = Cursors.Default;
        }


        private void EnviaDgvHuellasASdkMetodoElegido(
            List<DataGridViewRow> dgvTemporal, 
            ref int respuesta, int numHuellasAEnviar, int tamanioEnvioLote,
            Func<ProgressBar, List<DataGridViewRow>, int> formaEnvio)
        {
            //respuestaBiometrico = reloj.sta_batch_SetAllUserFPInfo(this.lbSysOutputInfo, this.PrgSTA, dgvUserFP.SelectedRows);
            int contadorLotesEnviados = 0;
            for (int i = 0; i < numHuellasAEnviar; i++)
            {
                dgvTemporal.Add(dgvUserFP.SelectedRows[i]);
                if ((i + 1) % tamanioEnvioLote == 0 && dgvTemporal.Count > 0)
                {
                    //respuesta = reloj.sta_batch_SetAllUserFPInfo(this.PrgSTA, dgvTemporal);
                    respuesta = formaEnvio(this.PrgSTA, dgvTemporal);

                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuesta, "Fin carga rápida");
                    if (respuesta == -1024)
                    {
                        return;
                    }
                    dgvTemporal.Clear();
                    contadorLotesEnviados++;
                    PrgSTA.Value = contadorLotesEnviados * (100 * tamanioEnvioLote / numHuellasAEnviar);
                    PrgSTA.Refresh();
                }
            }
            if (dgvTemporal.Count > 0)
            {
                respuesta = formaEnvio(this.PrgSTA, dgvTemporal);
                if (respuesta == -1024)
                {
                    return;
                }
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuesta, "Fin carga rápida");
                dgvBitacora.Refresh();
            }
        }

        #endregion

        #region Descarga Marcaciones


        private void tsbDescargar_Click(object sender, EventArgs e)
        {
            
            tsbDescargar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            
            DescargarMarcaciones();
            tsbDescargar.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void DescargarMarcaciones()
        {
            DataTable dt_Marcaciones = Prepara_Tabla_y_GridView();
            clsIntString respuestaSsn = new clsIntString();
            sn = "-292";

            respuestaSsn.respuesta = -292;
            respuestaSsn.sn = sn;

            try
            {
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, "", sEquipoActual, 2, "Inicio de Lectura de marcaciones");
                respuestaSsn = reloj.bio_LeeMarcaciones(dt_Marcaciones, dgvUserinfo);
                sn = respuestaSsn.sn;

                if (respuestaSsn.respuesta != 1 || dt_Marcaciones.Rows.Count == 0)
                {
                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuestaSsn.respuesta, "");
                }
                else
                {
                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuestaSsn.respuesta, "Las marcaciones del reloj han sido leidas: " + dt_Marcaciones.Rows.Count.ToString());
                    GuardarMarcacionesConUsuariosNuevos(gv_Attlog, dgvUserinfo, sn);
                }
            }
            catch (FechaNoValidaException ex)
            {                
                MessageBox.Show(ex.DatosLeidos, "Al leer datos del biométrico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(clsDataBaseException exBdd)
            {
                MessageBox.Show(exBdd.DataErrorDescription, "Al descargar marcaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace.ToString() + ex.InnerException, "Al descargar marcaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private DataTable Prepara_Tabla_y_GridView()
        {
            // columnas marcaciones
            DataTable dt_Marcaciones = new DataTable("dt_Marcaciones");
            dt_Marcaciones = PrepareColumnas.SetColumnas(dt_Marcaciones, dgvUserinfo);

            gv_Attlog.AutoGenerateColumns = true;
            gv_Attlog.Columns.Clear();
            gv_Attlog.DataSource = dt_Marcaciones;


            tabControl.SelectedTab = tpMarcaciones;
            tabControl.Refresh();

            PrgSTA.Value = 0;
            PrgSTA.Visible = true;

            return dt_Marcaciones;
        }

        public void GuardarMarcacionesConUsuariosNuevos(DataGridView gv_Attlog, DataGridView dgvUserinfo, string sn)
        {
            try
            {
               
                ClsInforma.ReportaBitacoraInvoke("Enviando marcaciones a Base de Datos", dgvBitacora, sEquipoActual);
                PrgSTA.Visible = true;
                ActualizaProgressBarInvoke(0, PrgSTA);

                LogicaReloj.GuardaMarcacionesPorLoteEnTablasTemporales(gv_Attlog, sn, PrgSTA);

                LogicaReloj.GuardaUsuariosNuevosPorLoteEnTablasTemporales(dgvUserinfo, PrgSTA);

                DataTable dtNewMarcaciones = clsLogicaSDK.GrabacionMarcacionesYUsuariosNuevosEnTablasDefinitivas(sn);
                ActualizaNewDgvMarcacionesInvoke(dtNewMarcaciones, dgvNewMarcaciones);
                ActualizaProgressBarInvoke(100, PrgSTA);

                string sMensaje = dtNewMarcaciones.Rows.Count == 1 ? "Se guardó 1 marcación nueva" : "Se guardaron " + dtNewMarcaciones.Rows.Count.ToString() + " marcaciones nuevas";
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, 1, sMensaje);
                MessageBox.Show(sMensaje, "Grabación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrgSTA.Visible = false;
            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                PrgSTA.Visible = false;
            }
        }


        private void GuardarMarcaciones(DataGridView gv_Attlog, DataGridView dgvUserinfo, string sn, IProgress<int>progress, CancellationToken cancelToken)
        {
            if (gv_Attlog.RowCount == 0)
            {
                MessageBox.Show("No hay marcaciones para guardar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                // Marcaciones
                progress?.Report(0);

                clsLogicaSDK.IniciaGrabacionMarcaciones(sn);

                ClsInforma.ReportaBitacoraInvoke("Enviando marcaciones a Base de Datos", dgvBitacora, sEquipoActual);

                LogicaReloj.GuardaMarcacionesTemporalesDepuradasPorLotes(gv_Attlog, sn, progress);

                ClsInforma.ReportaBitacoraInvoke("Revisando usuarios nuevos...", dgvBitacora, sEquipoActual);

                // Usuarios
                progress?.Report(0);
                
                LogicaReloj.GuardaUsuariosPorLotes(dgvUserinfo, progress);
                progress?.Report(50);

                ClsInforma.ReportaBitacoraInvoke("Validación final...", dgvBitacora, sEquipoActual);
                DataTable dtNewMarcaciones = clsLogicaSDK.GrabacionMarcacionesYUsuariosNuevosEnTablasDefinitivas(sn);
                ActualizaNewDgvMarcacionesInvoke(dtNewMarcaciones, dgvNewMarcaciones);
                progress?.Report(100);

                string sMensaje = dtNewMarcaciones.Rows.Count == 1 ? "Se guardó 1 marcación nueva" : "Se guardaron " + dtNewMarcaciones.Rows.Count.ToString() + " marcaciones nuevas";
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, 1, sMensaje);
                MessageBox.Show(sMensaje, "Grabación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //private void btnReporteDescargas_Click(object sender, EventArgs e)
        //{
        //    DataTable dtReporteDescargas = new DataTable();
        //    BindingSource bs = new BindingSource();
        //    clsLogicaSDK oLogs = new clsLogicaSDK();
        //    dtReporteDescargas = oLogs.DtReporteDescargas(dtpDesde.Value, dtpHasta.Value);
        //    bs.DataSource = dtReporteDescargas;
        //    dgvDetalleDescargasMasivas.DataSource = bs;
        //    Utilitarios.ClsDataGridView.PoneFiltros(dgvDetalleDescargasMasivas);
        //}
        #endregion

        #region Borrar Datos
        private void btn_clearAdmin_Click(object sender, EventArgs e)
        {
            // Borra Administradores
            if (confirmaEliminación("Confirma que desesa quitar los administradores del equipo?"))
            {
                Cursor = Cursors.WaitCursor;
                int resultado = reloj.sta_ClearAdmin();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se borraron los administradores");
                Cursor = Cursors.Default;
            }

        }

        private void btn_clearAllLogs_Click(object sender, EventArgs e)
        {
            // Borra todos los registros
            if (confirmaEliminación("Confirma que desesa borrar todos los registros?"))
            {
                Cursor = Cursors.WaitCursor;
                int resultado = reloj.sta_ClearAllLogs();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se borraron todos los registros");
                Cursor = Cursors.Default;
            }

        }

        private void btn_clearAllFp_Click(object sender, EventArgs e)
        {
            // Borra todas las huellas
            if (confirmaEliminación("Confirma que desesa eliminar todas las huellas?"))
            {
                Cursor = Cursors.WaitCursor;
                int resultado = reloj.sta_ClearAllFps();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se borraron todas las huellas");
                Cursor = Cursors.Default;
            }

        }

        private void btn_clearAllUser_Click(object sender, EventArgs e)
        {
            // Borra todos los usuarios
            if (confirmaEliminación("Confirma que desesa eliminar todos los usuarios?"))
            {
                Cursor = Cursors.WaitCursor;
                int resultado = reloj.sta_ClearAllUsers();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se borraron todos los usuarios");
                Cursor = Cursors.Default;
            }

        }

        private void btn_clearAllData_Click(object sender, EventArgs e)
        {
            // Borra Todo ClearKeeperData
            if (confirmaEliminación("Confirma que desesa borrar todos los datos?"))
            {
                Cursor = Cursors.WaitCursor;
                int resultado = reloj.sta_ClearAllData();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se borraron todos los datos");
                Cursor = Cursors.Default;
            }

        }

        private void btn_delAttLog_Click(object sender, EventArgs e)
        {
            int respuesta = 0;
            if (confirmaEliminación("Confirma que desesa eliminar los registros?"))
            {
                Cursor = Cursors.WaitCursor;
                respuesta = reloj.sta_DeleteAttLog();
                ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuesta, "Se borraron los registros de asistencia");
                Cursor = Cursors.Default;
            }

        }
        #endregion

        #region Sincronización y Control Reloj
        private void btnSYNCTime_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int resultado = reloj.sta_SYNCTime();
            ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "La hora del equipo está sincronizada");
            Cursor = Cursors.Default;
        }

        private void btnGetDeviceTime_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DateTime fechaActual;
            int resultado = reloj.sta_GetDeviceTime(lbDeviceTime, out fechaActual);
            ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se leyó la hora del equipo");
            Cursor = Cursors.Default;
        }

        private void btnSetDeviceTime_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int resultado = reloj.sta_SetDeviceTime((DateTime)dtDeviceTime.Value);
            ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "Se cambió la hora del equipo");
            Cursor = Cursors.Default;
        }

        private void btnRestartDevice_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int resultado = reloj.sta_btnRestartDevice();
            ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "El dispositivo se reinicia");
            Cursor = Cursors.Default;
            this.Close();
        }

        private void btnPowerOffDevice_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int resultado = reloj.sta_btnPowerOffDevice();
            ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, resultado, "El dispositivo se apagó");
            Cursor = Cursors.Default;
            this.Close();
        }

        #endregion

        // Usuarios en Reloj, sin separar si ya esá en la base de datos o no    
        DataTable dtUsuariosReloj = new DataTable();
        // Usuarios en la base de datos, sin deferenciar si están en el reloj o no
        DataTable dtUsuariosBdd = new DataTable();

        private void tsbUsuarios_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tpLeerUsuarios;
            tabControl.Refresh();
            Cursor = Cursors.WaitCursor;

            dgvUserFP.DataSource = null;

            PreparaDtUsuariosReloj();
            int numUsuarios = 0;
            int ret = reloj.ConsultaUsuariosReloj(dtUsuariosReloj, out numUsuarios);

            dtUsuariosBdd = clsLogicaEmpleados.RetornaBadgeNombre();
            dtUsuariosBdd.PrimaryKey = new DataColumn[] { dtUsuariosBdd.Columns["Badge"] };

            ActualizaLecturaUsuarios();
            chkUsuariosEnBaseDatos.Checked = true;
            chkUsuariosNuevos.Checked = true;

            Cursor = Cursors.Default;
        }

        private void PreparaDtUsuariosReloj()
        {
            dtUsuariosReloj.Rows.Clear();
            if(dtUsuariosReloj.Columns.Count == 0)
            {
                dtUsuariosReloj.Columns.Add("Badgenumber", typeof(string));
                dtUsuariosReloj.Columns.Add("Name", typeof(string));
                dtUsuariosReloj.Columns.Add("privilege", typeof(string));
                // dtUsuariosReloj.PrimaryKey = new DataColumn[] { dtUsuariosReloj.Columns["Badgenumber"] };
            }
            
            tsTxtBusqueda.Text = "";
        }

        private void ActualizaLecturaUsuarios()
        {

            DataTable DtUsersRelojYBase = new DataTable();
            var tablaUsersRelojYBase = from r in dtUsuariosReloj.AsEnumerable()
                                       join bdd in dtUsuariosBdd.AsEnumerable()
                                       on r["Badgenumber"].ToString().ToUpper() equals bdd["Badge"].ToString().ToUpper()
                                       into cruce
                                       where (cruce.Count() > 0 && r.Field<string>("Name").ToString().ToUpper().Contains(tsTxtBusqueda.Text.ToUpper()))
                                       select r;
            if (tablaUsersRelojYBase.Any())
            {
                DtUsersRelojYBase = tablaUsersRelojYBase.CopyToDataTable();
            }
            dgvUsuariosEnBase.DataSource = DtUsersRelojYBase;
            chkUsuariosEnBaseDatos.Text = "Usuarios ya en el Programa: " + DtUsersRelojYBase.Rows.Count;
            if (dgvUsuariosEnBase.Rows.Count > 0)
                DaFormatoDgvUsuariosReloj(dgvUsuariosEnBase);

            DataTable DtUsersNuevos = new DataTable();
            var tablaNuevos = dtUsuariosReloj.Rows.OfType<DataRow>().Where(
                a => dtUsuariosReloj.Rows.OfType<DataRow>().Select(k => k["Badgenumber"]).Except(
                    dtUsuariosBdd.Rows.OfType<DataRow>().Select(k => k["Badge"]).ToList()).Contains(
                    a["Badgenumber"]));
            var tablaNuevosFiltrados = from n in tablaNuevos
                                       where n.Field<string>("Name").ToUpper().Contains(tsTxtBusqueda.Text.ToUpper())
                                       select n;
            if (tablaNuevosFiltrados.Any())
            {
                DtUsersNuevos = tablaNuevosFiltrados.CopyToDataTable();
            }

            dgvUsuariosNuevos.DataSource = DtUsersNuevos;
            chkUsuariosNuevos.Text = "Usuarios Nuevos: " + DtUsersNuevos.Rows.Count;
            if (dgvUsuariosNuevos.Rows.Count > 0)
                DaFormatoDgvUsuariosReloj(dgvUsuariosNuevos);

            ChequeaDgvUsuarios();
        }

        private void ChequeaDgvUsuarios()
        {
            controladorCheck = false;
            chkUsuariosEnBaseDatos.Checked = false;
            chkUsuariosNuevos.Checked = false;
            controladorCheck = true;
            chkUsuariosEnBaseDatos.Checked = true;
            chkUsuariosNuevos.Checked = true;
        }

        private void DaFormatoDgvUsuariosReloj(DataGridView dgv)
        {
            if (!dgv.Columns.Contains("Chk"))
            {
                DataGridViewCheckBoxColumn dgvChekCol = new DataGridViewCheckBoxColumn();
                dgvChekCol.ValueType = typeof(bool);
                dgvChekCol.Name = "Chk";
                dgvChekCol.HeaderText = "[  ]";
                dgvChekCol.Width = 26;
                dgvChekCol.FalseValue = false;
                dgvChekCol.TrueValue = true;
                dgv.Columns.Add(dgvChekCol);
            }
            
            dgv.Columns["Chk"].DisplayIndex = 1;
            dgv.Columns["Badgenumber"].DisplayIndex = 2;
            dgv.Columns["Name"].DisplayIndex = 3;

            dgv.RowHeadersVisible = false;
            dgv.Columns["privilege"].Visible = false;
            dgv.Columns["Badgenumber"].HeaderText = "Codigo";
            dgv.Columns["Badgenumber"].Width = 75;
            dgv.Columns["Name"].HeaderText = "Nombre";
            dgv.Columns["Name"].Width = 250;
        }

        private void tsbInfoReloj_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tpCapacidad;
            getCapacityInfo(reloj);
            getDeviceInfo(reloj);
        }


        private void btnGetDataInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnGetSystemInfo_Click(object sender, EventArgs e)
        {

        }

        private CancellationTokenSource cts;
        private async void TsbDescargaAsync_Click(object sender, EventArgs e)
        {
            #region CancellationToken, progress, etc
            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;
            var progress = new Progress<int>(v => PrgSTA.Value = v);
            #endregion

            tsbDescargaAsync.Enabled = false;
            Cursor = Cursors.WaitCursor;

            DataTable dt_Marcaciones = Prepara_Tabla_y_GridView();
            try
            {
                getCapacityInfo(reloj);
                ClsInforma.ReportaBitacoraInvoke("Leyendo usuarios / marcaciones ...", dgvBitacora, sEquipoActual);
                int countAttLog = int.Parse(txtAttLogCnt.Text);
                clsIntString retorno = new clsIntString();
                await Task.Run(() =>
                {
                    retorno = reloj.bio_LeeMarcaciones_ProgressBar(PrgSTA, dt_Marcaciones, dgvUserinfo, progress, cancelToken, countAttLog);
                }, cts.Token);

                if (retorno.respuesta != 1 || dt_Marcaciones.Rows.Count == 0)
                {
                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, retorno.sn, sEquipoActual, retorno.respuesta, "");
                }
                else
                {
                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, retorno.sn, sEquipoActual, retorno.respuesta, "Las marcaciones del reloj han sido leidas: " + dt_Marcaciones.Rows.Count.ToString());
                    GuardarMarcaciones(gv_Attlog, dgvUserinfo, retorno.sn, progress, cancelToken);
                }
            }
            catch (Exception)
            {
                throw;
            }

            PrgSTA.Visible = false;

            //var progress = new Progress<int>(porcentaje =>
            //{
            //    PrgSTA.Value = porcentaje;
            //});
            // await Task.Run(() => DescargarMarcaciones(progress, out respuestaBiometrico, out sn));
            
            tsbDescargar.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seguro que quiere cancelar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
                cts?.Cancel();
        }


        private void tsbDescargarUsuarios_Click(object sender, EventArgs e)
        {

            // Tarea Paralelo
            token = _cancellation.Token;
            tarea = Task.Run(() =>
            {
                timer1.Enabled = true;
                if (token.IsCancellationRequested)
                {
                    timer1.Enabled = false;
                }
            }, token);

            Cursor = Cursors.WaitCursor;

            dgvUserFP.DataSource = null;
            DataTable dtEmpleados = PrepareColumnas.UsuariosBiometrico();
            int ret = reloj.sta_GetAllUserFPInfo(this.dgvBitacora, this.PrgSTA, dgvLeeUsuarios, dtEmpleados);
            Cursor = Cursors.Default;

            _cancellation.Cancel();

        }

        private void chkUsuariosEnBaseDatos_CheckedChanged(object sender, EventArgs e)
        {
            checaDataGridView(dgvUsuariosEnBase, chkUsuariosEnBaseDatos.Checked);
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ActualizaLecturaUsuarios();
            this.Cursor = Cursors.Default;
        }

        private void chkUsuariosNuevos_CheckedChanged(object sender, EventArgs e)
        {
            checaDataGridView(dgvUsuariosNuevos, chkUsuariosNuevos.Checked);
        }

        bool controladorCheck = true;
        private void checaDataGridView(DataGridView dgv, bool si_no)
        {
            if(controladorCheck)
            {
                foreach (DataGridViewRow fila in dgv.Rows)
                {
                    fila.Cells["Chk"].Value = si_no;
                }
            }
        }

        private void dgvUsuariosEnBase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
            ch1 = (DataGridViewCheckBoxCell)dgvUsuariosEnBase.Rows[dgvUsuariosEnBase.CurrentRow.Index].Cells["Chk"];
            if (ch1.Value == null)
                ch1.Value = true;
            else
                ch1.Value = !(bool)ch1.Value;
        }

        private void dgvUsuariosNuevos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
            ch1 = (DataGridViewCheckBoxCell)dgvUsuariosNuevos.Rows[dgvUsuariosNuevos.CurrentRow.Index].Cells["Chk"];
            if (ch1.Value == null)
                ch1.Value = true;
            else
                ch1.Value = !(bool)ch1.Value;
        }

        private void btnDescargaRostros_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!ActualizaLstEmpleadosChequedosDescarga())
                return;

            dgvFaceInfo.Rows.Clear();
            ClsInforma.ReportaBitacora("Inicia lectura de rostros del dispositivo", dgvBitacora, sEquipoActual);
            PrgSTA.Visible = true;

            reloj.sta_GetAllUserFaceInfo(dgvBitacora, PrgSTA, dgvFaceInfo, 0, lstEmpleadosChequedosDescarga);
            
            if (dgvFaceInfo.RowCount == 0)
            {
                MessageBox.Show("No hay rostros para guardar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                PrgSTA.Visible = false;
                return;
            }

            try
            {
                LogicaB.clsLogicaSDK oLogRostros = new clsLogicaSDK();
                // Envía todo el DataGridView, la desventaja es que durante el tiempo de proceso no se recibe ningún mensaje y da lugar a pensar que ya se colgó
                // Fue la primera opción
                // oLogHuellas.GuardaHuellasEnTemporal(dgvUserFP.SelectedRows);

                // Quiere enviar de 100 en 100
                // int numeroUsuariosAEnviar = dgvUserFP.SelectedRows.Count / 100;
                int j = 0;
                oLogRostros.IniciaGrabacionRostros();
                ClsInforma.ReportaBitacora("Inicia grabación de rostros en base de datos", dgvBitacora, sEquipoActual);
                
                ActualizaProgressBarInvoke(j, PrgSTA);

                List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
                for (int i = 0; i < dgvFaceInfo.Rows.Count; i++)
                {
                    dgvTemporal.Add(dgvFaceInfo.Rows[i]);
                    if ((i + 1) % 100 == 0 && dgvTemporal.Count > 0)
                    {
                        oLogRostros.guardaRostros(dgvTemporal);
                        dgvTemporal.Clear();
                        j++;
                        ActualizaProgressBarInvoke((int)(j * (10000.0 / (dgvFaceInfo.Rows.Count+1))), PrgSTA);
                        // PrgSTA.Value = (int)contadorLotesEnviados * (10000 / dgvFaceInfo.Rows.Count);
                        // PrgSTA.Refresh();
                    }
                }
                if (dgvTemporal.Count > 0)
                {
                    oLogRostros.guardaRostros(dgvTemporal);
                }
                oLogRostros.FinalizaGrabacionRostros();

                string mensaje = "Se han grabado " + dgvFaceInfo.Rows.Count.ToString() + " rostros";
                MessageBox.Show(mensaje, "Grabación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClsInforma.ReportaBitacora(mensaje, dgvBitacora, sEquipoActual);
            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                PrgSTA.Visible = false;
                Cursor = Cursors.Default;
            }
        }

        private void btnEnviarRostrosABiometrico_Click(object sender, EventArgs e)
        {
            if (dgvUserFace.RowCount == 0)
            {
                MessageBox.Show("No hay rostros para enviar al dispositivo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Cursor = Cursors.WaitCursor;
            PrgSTA.Visible = true;
            PrgSTA.Value = 0;
            PrgSTA.Refresh();

            int j = 0;
            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
            int respuesta = -1;
            int n = dgvUserFace.Rows.Count;
            if (cbBatchUpload.Checked)
            {
                ClsInforma.ReportaBitacora("Inicio carga de rostros al equipo", dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();

                for (int i = 0; i < n; i++)
                {
                    dgvTemporal.Add(dgvUserFace.Rows[i]);
                    if ((i + 1) % 500 == 0 && dgvTemporal.Count > 0)
                    {
                        respuesta = reloj.sta_SetAllUserFaceInfo(this.PrgSTA, dgvTemporal);
                        ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuesta, "Fin carga rápida");
                        if (respuesta == -1024)
                        {
                            return;
                        }
                        dgvTemporal.Clear();
                        j++;
                        PrgSTA.Value = j * (50000 / n);
                        PrgSTA.Refresh();
                    }
                }
                if (dgvTemporal.Count > 0)
                {
                    respuesta = reloj.sta_SetAllUserFaceInfo(this.PrgSTA, dgvTemporal);
                    ClsInforma.NotificaRespuestaBddBitacora(dgvBitacora, sn, sEquipoActual, respuesta, "Fin carga rostros");
                    if (respuesta == -1024)
                    {
                        return;
                    }
                }
            }
            else
            {
                ClsInforma.ReportaBitacora("Inicio carga rostros", dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();
                //respuestaBiometrico = reloj.sta_SetAllUserFPInfo(this.lbSysOutputInfo, this.PrgSTA, dgvUserFP.SelectedRows);
                for (int i = 0; i < n; i++)
                {
                    dgvTemporal.Add(dgvUserFP.SelectedRows[i]);
                    if ((i + 1) % 100 == 0 && dgvTemporal.Count > 0)
                    {
                        respuesta = reloj.sta_SetAllUserFPInfo(this.PrgSTA, dgvTemporal);
                        if (respuesta == -1024)
                        {
                            return;
                        }
                        dgvTemporal.Clear();
                        j++;
                        PrgSTA.Value = j * (10000 / n);
                        PrgSTA.Refresh();
                    }
                }
                if (dgvTemporal.Count > 0)
                {
                    respuesta = reloj.sta_SetAllUserFPInfo(this.PrgSTA, dgvTemporal);
                    if (respuesta == -1024)
                    {
                        return;
                    }
                    ClsInforma.ReportaBitacora("Fin carga rostros", dgvBitacora, sEquipoActual);
                    dgvBitacora.Refresh();
                }
            }
            if (respuesta == 1)
            {
                PrgSTA.Value = 100;
                PrgSTA.Refresh();
                MessageBox.Show("Los rostros seleccionados han sido transferidos exitosamente al dispositivo", "Transferencia Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrgSTA.Visible = false;
            }

            Cursor = Cursors.Default;
        }

        private void btnTraeRostrosDeBdd_Click(object sender, EventArgs e)
        {
            RecuperaRostros();
        }

        private void RecuperaRostros()
        {
            this.Cursor = Cursors.WaitCursor;
            dgvUserFace.DataSource = null;
            LogicaB.clsLogicaSDK oLogRostros = new clsLogicaSDK();
            BindingSource bs = new BindingSource();
            DataTable dt = new DataTable();
            dt = oLogRostros.RetornaRostros(tsTxtBusqueda.Text);
            bs.DataSource = dt;
            dgvUserFace.DataSource = bs;
            this.Cursor = Cursors.Default;
        }

        private void btnEliminarUsuarios_Click(object sender, EventArgs e)
        {            
            if(!confirmaEliminación("Al eliminar un usuario de un reloj biométrico, la persona no podrá registrarse en el dispositivo"))
            {
                return;
            }
            Cursor = Cursors.WaitCursor;
            if (!ActualizaLstEmpleadosChequedosDescarga())
                return;

            try
            {
                reloj.sta_DeleteEnrollData(dgvBitacora, PrgSTA, 0, lstEmpleadosChequedosDescarga);

                PrgSTA.Value = 100;
                PrgSTA.Refresh();

                string mensaje = "Se han eliminado " + lstEmpleadosChequedosDescarga.Count.ToString() + " usuarios";
                MessageBox.Show(mensaje, "Eliminación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClsInforma.ReportaBitacora(mensaje, dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();
            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            Cursor = Cursors.Default;
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Cursor = Cursors.WaitCursor;
                ActualizaLecturaUsuarios();
                this.Cursor = Cursors.Default;
            }
                
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl.SelectedTab == tpEnviarHuellas || tabControl.SelectedTab == tpEnviarRostros || tabControl.SelectedTab == tpLeerUsuarios)
            {
                tsbBuscar.Visible = true;
                tsTxtBusqueda.Visible = true;
            }
            else
            {
                tsbBuscar.Visible = false;
                tsTxtBusqueda.Visible = false;
            }
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            switch (tabControl.SelectedTab.Name)
            {
                case "tpLeerUsuarios":
                    ActualizaLecturaUsuarios();
                    break;
                case "tpEnviarHuellas":
                    RecuperaHuellas();
                    break;
                case "tpEnviarRostros":
                    RecuperaRostros();
                    break;

            }
            
            this.Cursor = Cursors.Default;
        }

        private void tsTxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Cursor = Cursors.WaitCursor;
                ActualizaLecturaUsuarios();
                this.Cursor = Cursors.Default;
            }
        }

        private void dgvUserFP_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnTraerUsuariosDeBdd_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;
            GetTodosEmpleadosParaEnviarAReloj();
            this.Cursor = Cursors.Default;
        }

        private void GetEmpleadosSinHuellaNiRostro()
        {
            dgvNombresUsuarios.DataSource = null;
            BindingSource bs = new BindingSource();
            DataTable dt = clsLogicaEmpleados.GetEmpleadosSinHuellaNiRostro();
            bs.DataSource = dt;
            dgvNombresUsuarios.DataSource = bs;
        }
        private void GetTodosEmpleadosParaEnviarAReloj()
        {
            dgvNombresUsuarios.DataSource = null;
            BindingSource bs = new BindingSource();
            DataTable dt = clsLogicaEmpleados.GetTodosEmpleadosParaEnviarAReloj();
            bs.DataSource = dt;
            dgvNombresUsuarios.DataSource = bs;
        }
        
        private void btnEnviarNombresUsuarios_Click(object sender, EventArgs e)
        {
            if (dgvNombresUsuarios.RowCount == 0)
            {
                MessageBox.Show("No hay usuarios para enviar al dispositivo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int numeroUsuariosAEnviar = dgvNombresUsuarios.SelectedRows.Count;
            if (numeroUsuariosAEnviar == 0)
            {
                MessageBox.Show("Debe seleccionar los usuarios para enviar al dispositivo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Cursor = Cursors.WaitCursor;
            PrgSTA.Visible = true;
            PrgSTA.Value = 0;
            PrgSTA.Refresh();         
            
            ClsInforma.ReportaBitacora("Inicio envío de usuarios a dispositivo", dgvBitacora, sEquipoActual);
            dgvBitacora.Refresh();
            try
            {
                List<SDKHelper.Employee> empleados = Mapper.ConvertToDesencriptedEmployees(dgvNombresUsuarios.SelectedRows);
                reloj.sta_setEmployees(empleados);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            PrgSTA.Value = 100;
            PrgSTA.Refresh();
            MessageBox.Show($"Los usuarios seleccionados ({numeroUsuariosAEnviar}) han sido transferidos exitosamente al dispositivo", "Transferencia Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PrgSTA.Visible = false;

            Cursor = Cursors.Default;
        }

        private void BtnDescargarTarjetas_Click(object sender, EventArgs e)
        {
            if (!ActualizaLstEmpleadosChequedosDescarga())
                return;

            Cursor = Cursors.WaitCursor;
            dgvLeeUsuarios.Rows.Clear();
            ClsInforma.ReportaBitacora("Inicia lectura de tarjetas del dispositivo.", dgvBitacora, sEquipoActual);

            DataTable dtEmpleados = PrepareColumnas.UsuariosBiometrico();
            reloj.sta_GetAllUserTarjetaInfo(dgvBitacora, PrgSTA, dgvLeeUsuarios, dtEmpleados, 0, lstEmpleadosChequedosDescarga);

            if (dtEmpleados.Rows.Count == 0)
            {
                MessageBox.Show("No hay información de tarjetas para guardar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Cursor = Cursors.Default;
                return;
            }

            try
            {
                ClsInforma.ReportaBitacora("Inicia grabación de tarjetas en base de datos", dgvBitacora, sEquipoActual);

                LogicaB.clsLogicaSDK oLogHuellas = new clsLogicaSDK();
                PrgSTA.Visible = true;
                PrgSTA.Value = 0;
                PrgSTA.Refresh();
                ActualizaCreaDatosUsuarios(dtEmpleados, oLogHuellas);

                int k = dtEmpleados.Rows.Count;
                string mensaje = k == 1 ? "Se ha grabado 1 usuario con tarjeta." : "Se han grabado " + k.ToString() + " usuarios con tarjeta.";
                MessageBox.Show(mensaje, "Grabación Correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClsInforma.ReportaBitacora(mensaje, dgvBitacora, sEquipoActual);
                dgvBitacora.Refresh();

            }
            catch (clsLogicaException error)
            {
                MessageBox.Show(error.logErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                PrgSTA.Visible = false;
                Cursor = Cursors.Default;
            }
        }

        private void btnEnviaTarjetasAReloj_Click(object sender, EventArgs e)
        {

        }
    }
}
