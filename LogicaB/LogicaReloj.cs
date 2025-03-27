using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogicaB
{
    public static class LogicaReloj
    {
        public static void GuardaMarcacionesPorLoteEnTablasTemporales(DataGridView gv_Attlog, string sn, ProgressBar PrgSTA)
        {
            int sizeLoteEnvio = 500;
            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();

            clsLogicaSDK oLogMarcaciones = new clsLogicaSDK();
            clsLogicaSDK.IniciaGrabacionMarcaciones(sn);

            for (int i = 0; i < gv_Attlog.Rows.Count; i++)
            {
                dgvTemporal.Add(gv_Attlog.Rows[i]);
                if ((i + 1) % sizeLoteEnvio == 0 && dgvTemporal.Count > 0)
                {
                    oLogMarcaciones.GuardaSoloMarcacionesTemporalesDepuradas(dgvTemporal, sn);
                    dgvTemporal.Clear();
                    ActualizaProgressBarInvoke((int)(i * (100.0 / gv_Attlog.Rows.Count)), PrgSTA);
                }
            }
            if (dgvTemporal.Count > 0)
            {
                oLogMarcaciones.GuardaSoloMarcacionesTemporalesDepuradas(dgvTemporal, sn);
            }
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

        public static void GuardaMarcacionesTemporalesDepuradasPorLotes(DataGridView gv_Attlog, string sn, IProgress<int> progress)
        {
            int nRegistros = gv_Attlog.RowCount;            
            PoneLoteSize(nRegistros, out int paquete, out int iAvance);
            

            int j = 0;

            List<DataGridViewRow> dgvTemporal = new List<DataGridViewRow>();
            var oLogHuellas = new clsLogicaSDK();
            for (int i = 0; i < nRegistros; i++)
            {
                dgvTemporal.Add(gv_Attlog.Rows[i]);
                if ((i + 1) % paquete == 0 && dgvTemporal.Count > 0)
                {
                    oLogHuellas.GuardaSoloMarcacionesTemporalesDepuradas(dgvTemporal, sn);
                    dgvTemporal.Clear();
                    j++;
                    if ((j * iAvance) > 100)
                        progress?.Report(100);
                    progress?.Report(j * iAvance);
                }
            }
            if (dgvTemporal.Count > 0)
            {
                oLogHuellas.GuardaSoloMarcacionesTemporalesDepuradas(dgvTemporal, sn);
            }
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
