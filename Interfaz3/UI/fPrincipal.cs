using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Windows.Forms;
using LogicaB;
using Utilitarios;

namespace AdminDispositivosBiometricos
{
    public partial class fPrincipal : Form
    {

        DataTable dtRelojes = new DataTable();
        private DataGridViewRow filaReloj;      

        public fPrincipal()
        {
            InitializeComponent();
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvRelojes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un dispositivo para poder editar sus valores", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            filaReloj = dgvRelojes.SelectedRows[0];
            fEditaReloj vEdicionReloj = new fEditaReloj();
            LeeDatosFilaDgv(vEdicionReloj.datosReloj);

            vEdicionReloj.ShowDialog();
            cargaRelojes();
        }

        public bool ConfirmaRelojEsValido(string snReloj)
        {
            foreach(DataRow fila in dtRelojes.Rows)
            {
                if(snReloj == fila[0].ToString())
                {
                    return true;
                }
            }
            return false;
        }
        private void LeeDatosFilaDgv(Utilitarios.ClsReloj datosReloj)
        {
            string ip = string.IsNullOrWhiteSpace(filaReloj.Cells[2].Value.ToString()) ? "192.168.1.201" : filaReloj.Cells[2].Value.ToString();
            datosReloj.idReloj = Int32.Parse(filaReloj.Cells[0].Value.ToString());
            datosReloj.sNombreReloj = filaReloj.Cells[1].Value.ToString();
            datosReloj.sIP = IPAddress.Parse(ip); 
            datosReloj.iPuerto = Int32.Parse(filaReloj.Cells[3].Value.ToString());
            datosReloj.iNumero = Int32.Parse(filaReloj.Cells[4].Value.ToString());
            datosReloj.sSN = filaReloj.Cells[5].Value.ToString();
        }

        private void tsbConectar_Click(object sender, EventArgs e)
        {
            filaReloj = dgvRelojes.SelectedRows[0];
            string nombreReloj = filaReloj.Cells[1].Value.ToString();
            if (!DetectaVentanaAbierta(nombreReloj))
            {
                fControlReloj vReloj = new fControlReloj();
                LeeDatosFilaDgv(vReloj.datosReloj);
                vReloj.RelojesValidos = PrepareColumnas.DatatableToList(dtRelojes, "NumeroSerie");
                vReloj.Show();
            }
        }

        private void fPrincipal_Load(object sender, EventArgs e)
        {
            cargaRelojes();
        }

        private void cargaRelojes()
        {
            try
            {
                clsLogicaDispositivos oRelojes = new clsLogicaDispositivos();
                dtRelojes = oRelojes.dtRelojesValidos();
                ClsDataTableDgv.LlenaGridConDataTable(dtRelojes, dgvRelojes, new List<string> { "ID" });
            }
            catch(clsLogicaException errBdd)
            {
                MessageBox.Show(errBdd.logErrorDescription, "Error con Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n" + err.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidacionRelojes()
        {

            clsLogicaDispositivos oLogDispositivos = new clsLogicaDispositivos();            

            // Inicio modificación para BAN Ecuador
            //DateTime fechaVigencia = oSeguridadFechaVigencia.retornaFechaVigencia();
            DateTime fechaVigencia = new DateTime(2021, 12, 31);
            // Fin modificación para BAN Ecuador

            List<string> lstRelojesValidos = new List<string>();
        }

        private static bool DetectaVentanaAbierta(string nombreReloj)
        {
            bool abierto = false;
            foreach(Form ventana in Application.OpenForms)
            {
                if (ventana.Text.Contains(nombreReloj))
                {
                    return true;
                }
            }
            return abierto;
        }      

        private void tsbCorreos_Click(object sender, EventArgs e)
        {
            fAdminDestinatariosCorreos vCorreos = new fAdminDestinatariosCorreos();
            vCorreos.Show();
        }

        private void spBtnDescargaMasivaRelojes_ButtonClick(object sender, EventArgs e)
        {
            fDescargaMasiva fDescarga = new fDescargaMasiva();
            clsLogicaDispositivos oLog = new clsLogicaDispositivos();
            fDescarga.listaRelojes = oLog.ListRelojes();
            fDescarga.modo = fDescargaMasiva.EnumModo.Automático;
            fDescarga.Show();
            fDescarga.DescargaMarcacionesRelojes(fDescarga.listaRelojes);
        }

        private void tsmiLecturaLogs_Click(object sender, EventArgs e)
        {
            fLogsDescargas vLogsDescargas = new fLogsDescargas();
            vLogsDescargas.Show();
        }

        private void tsbUSB_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Archivo de registros reloj biometrico|*.dat";
            openFileDialog1.Title = "Seleccione un archivo de marcaciones";
            openFileDialog1.Multiselect = false;

            DialogResult respuesta = openFileDialog1.ShowDialog();

            switch (respuesta) {
                case DialogResult.OK:
                    string[] lines = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                    ImportaFilasDeArchivo(lines);
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    MessageBox.Show("El formato del archivo seleccionado no corresponde a un archivo de marcaciones de reloj biométrico", "Archivo inválido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
            
            return;
        }

        private void ImportaFilasDeArchivo(string[] lines)
        {
            this.Cursor = Cursors.WaitCursor;

            //int counter = 0;
            char[] delimiterChars = { ' ', ',', '.', '\t' };
            //                     List<Tuple>(badge, fecha,  hora,   1,   ent_sal, hue_ros, workCode) registros = new List<Tuple>();
            var lstRegistrosValidos = new List<Tuple<string, string, string, int, string, string, string>>();
            var lstRegistrosNoValidos = new List<Tuple<string, string, string, int, string, string, string>>();
            try
            {
                DateTime fecha;
                clsLogicaRegistrosUSB.EnceraRegistrosUSB();
                foreach (string line in lines)
                {
                    // procesa la línea line
                    string[] param = line.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                    if (DateTime.TryParse(param[1], out fecha))
                    {
                        if (fecha > DateTime.Now)
                        {
                            lstRegistrosNoValidos.Add(Tuple.Create(param[0], fecha.ToString("yyyy-MM-dd"), param[2], Int32.Parse(param[3]), param[4], param[5], param[6]));
                            //MessageBox.Show("Hay una inconsistencia en la información del archivo, por favor confirme que sea un archivo válido de marcaciones",
                            //    "Error en archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }
                        else
                        {
                            lstRegistrosValidos.Add(Tuple.Create(param[0], fecha.ToString("yyyy-MM-dd"), param[2], Int32.Parse(param[3]), param[4], param[5], param[6]));
                        }
                    }
                    else
                    {
                        // lstRegistrosValidos.Add(Tuple.Create(param[0], param[1], param[2], Int32.Parse(param[3]), param[4], param[5], param[6]));
                        MessageBox.Show("Hay una inconsistencia en el formato del archivo, por favor confirme que sea un archivo válido de marcaciones",
                                "Error en archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //if ((counter + 1) % 100 == 0 && lstRegistrosValidos.Count > 0)
                    //{
                    //    clsLogicaRegistrosUSB.GuardaRegistrosUsbEnTemporal(lstRegistrosValidos, "1234567890123");
                    //    lstRegistrosValidos.Clear();
                    //}
                    // counter++;
                }
                if (lstRegistrosValidos.Count > 0)
                    clsLogicaRegistrosUSB.GuardaRegistrosUsbEnTemporal(lstRegistrosValidos, "1234567890123");
                lstRegistrosValidos.Clear();
                int num = clsLogicaRegistrosUSB.IngresaNuevasMarcaciones();
                clsLogicaRegistrosUSB.EnceraRegistrosUSB();
                
                MessageBox.Show(GetMensajeCargaCorrecta(num), "Registros vía USB", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (clsLogicaException errBdd)
            {
                MessageBox.Show(errBdd.logErrorDescription, "Error con Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + "\n" + err.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private static string GetMensajeCargaCorrecta(int num)
        {
            return $"Carga Completa. Los registros del archivo han sido ingresados al sistema ProperTime.\r\n\r\n {num} marcaciones nuevas.";
        }

        private void tsbUsuarios_Click(object sender, EventArgs e)
        {
            fEmpleado vEmpleado = new fEmpleado();
            vEmpleado.ShowDialog();
        }
    }
}

