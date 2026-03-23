using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicaB
{
    public static class LogicaReloj
    {
        public static async Task GuardaMarcacionesPorLoteEnTablasTemporales(
            DataGridView gv_Attlog, string sn, ProgressBar PrgSTA)
        {
            // ✅ Se extrae la data del grid en el hilo UI ANTES de ir al background
            List<MarcacionCheckInOut> marcaciones = ExtraeMarcacionesDeGrid(gv_Attlog, sn);
            var progreso = new Progress<int>(valor => PrgSTA.Value = valor);

            clsLogicaSDK oLogMarcaciones = new clsLogicaSDK();
            clsLogicaSDK.IniciaGrabacionMarcaciones(sn);

            await Task.Run(() =>
            {
                var oLogica = new clsLogicaSDK();
                oLogica.GuardaSoloMarcacionesTemporalesDepuradas(marcaciones, sn, progreso);
            });
        }
        private static List<MarcacionCheckInOut> ExtraeMarcacionesDeGrid(DataGridView gv, string sn)
        {
            var lista = new List<MarcacionCheckInOut>(gv.Rows.Count); // capacidad inicial = sin realocaciones
            foreach (DataGridViewRow fila in gv.Rows)
            {
                lista.Add(new MarcacionCheckInOut
                {
                    UserId = fila.Cells["User ID"].Value?.ToString(),
                    VerifyDate = Convert.ToDateTime(fila.Cells["Verify Date"].Value),
                    VerifyType = fila.Cells["Verify Type"].Value?.ToString(),
                    VerifyState = Convert.ToInt32(fila.Cells["Verify State"].Value),
                    WorkCode = Convert.ToInt32(fila.Cells["WorkCode"].Value),
                    Sn = sn
                });
            }
            return lista;
        }
        public static void GuardaUsuariosNuevosPorLoteEnTablasTemporales(DataGridView dgvUserinfo, ProgressBar PrgSTA)
        {
            int sizeLoteEnvio = 100;
            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();

            clsLogicaSDK oLogUsuarios = new clsLogicaSDK();
            for (int i = 0; i < dgvUserinfo.Rows.Count; i++)
            {
                if (dgvUserinfo.Rows[i].Cells[0].Value != null)
                {
                    dgvTemporal.Add(dgvUserinfo.Rows[i]);
                    if ((i + 1) % sizeLoteEnvio == 0 && dgvTemporal.Count > 0)
                    {
                        oLogUsuarios.GuardaUsuarios(dgvTemporal);
                        dgvTemporal.Clear();
                        ActualizaProgressBarInvoke((int)(i * (100.0 / dgvUserinfo.Rows.Count)), PrgSTA);
                    }
                }
            }
            if (dgvTemporal.Count > 0)
            {
                oLogUsuarios.GuardaUsuarios(dgvTemporal);
            }
        }

        private static void ActualizaProgressBarInvoke(int avance, ProgressBar Barra)
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

        private delegate void DelegadoProgressBar(int iAvance, ProgressBar progressBar);

        private static void ActualizaProgressBar(int j, ProgressBar progressBar)
        {
            progressBar.Value = j;
            progressBar.Refresh();
        }

        public static async void GuardaMarcacionesTemporalesDepuradasPorLotes(
            DataGridView gv_Attlog, string sn, ProgressBar PrgSTA)
        {
            List<MarcacionCheckInOut> marcaciones = ExtraeMarcacionesDeGrid(gv_Attlog, sn);
            var progreso = new Progress<int>(valor => PrgSTA.Value = valor);
            
            await Task.Run(() =>
            {
                var oLogHuellas = new clsLogicaSDK();
                oLogHuellas.GuardaSoloMarcacionesTemporalesDepuradas(marcaciones, sn, progreso);
            });            
        }

        public static void GuardaUsuariosPorLotes(DataGridView dgvUserinfo, IProgress<int> progress)
        {
            int j = 0;
            PoneLoteSize(dgvUserinfo.RowCount, out int paquete, out int iAvance);
            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
            var oLogHuellas = new clsLogicaSDK();

            dgvTemporal.Clear();
            for (int i = 0; i < dgvUserinfo.Rows.Count; i++)
            {
                dgvTemporal.Add(dgvUserinfo.Rows[i]);
                if ((i + 1) % paquete == 0 && dgvTemporal.Count > 0)
                {
                    oLogHuellas.GuardaUsuarios(dgvTemporal);
                    dgvTemporal.Clear();
                    j++;
                    if ((j * iAvance) > 100)
                        progress?.Report(100);
                    progress?.Report(j * iAvance);
                }
            }
            if (dgvTemporal.Count > 0)
            {
                oLogHuellas.GuardaUsuarios(dgvTemporal);
            }
        }

        public static void PoneLoteSize(int numRegistros, out int numRegsEnPaquete, out int porcentAvance)
        {
            if (numRegistros <= 100)
            {
                numRegsEnPaquete = numRegistros;
                porcentAvance = 75;
            }
            else if (numRegistros <= 1000)
            {
                numRegsEnPaquete = (numRegistros / 10) + 1;
                porcentAvance = 10;
            }
            else
            {
                numRegsEnPaquete = (numRegistros / 100) + 1;
                porcentAvance = 1;
            }
        }

    }
}
