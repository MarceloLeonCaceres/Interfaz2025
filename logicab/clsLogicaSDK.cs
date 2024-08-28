using System;
using System.Collections.Generic;
using System.Data;

using ConexionDatos;
using DatosB;
using Utilitarios;
using System.Windows.Forms;

namespace LogicaB
{
    public class clsLogicaSDK
    {
        public DataTable conteoUsersFP()
        {
            try
            {
                DatosB.clsDatosSDK objDatos = new DatosB.clsDatosSDK();
                return objDatos.conteoUsersFP();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void IniciaGrabacionHuellas()
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.IniciaGrabacionHuellas();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void GuardaHuellasEnTemporal(List<DataGridViewRow> dgv)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.GuardaHuellasEnTemporal(dgv);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void ActualizaCreaDatosUsuarios(DataTable datosEmpleados)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.ActualizaCreaDatosUsuarios(datosEmpleados);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }
        public void FinalizaGrabacionHuellas()
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.FinalizaGrabacionHuellas();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public DataTable RetornaHuellas(string sBusqueda)
        {
            clsDatosSDK oDatos = new clsDatosSDK();
            return oDatos.RetornaHuellas(sBusqueda);
        }

        public DataTable RetornaRostros(string sBusqueda)
        {
            clsDatosSDK oDatos = new clsDatosSDK();
            return oDatos.RetornaRostros(sBusqueda);
        }

        public void IniciaGrabacionMarcaciones(string sn, int numeroMarcaciones = 0)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.RegistraLogEventoBdd(0, sn, -1, "Inicio", "Grabación Marcaciones");
                oDatos.IniciaGrabacionMarcaciones(sn);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void GuardaSoloMarcacionesTemporalesDepuradas(List<DataGridViewRow> dgv, string sn)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.GuardaSoloMarcacionesTemporales(dgv, sn);
                oDatos.DepuraMarcacionesMasivas(sn);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void InicializaTablasDescargaTemporales()
        {
            clsDatosSDK oDatos = new clsDatosSDK();
            oDatos.InicializaTablasDescargaTemporales();
        }
        //public void GuardaSoloMarcacionesTemporalesDepuradas(DataGridView dgv, string sn, int inicio, int fin)
        //{
        //    try
        //    {
        //        clsDatosSDK oDatos = new clsDatosSDK();
        //        oDatos.GuardaSoloMarcacionesTemporales(dgv, sn, inicio, fin);
        //        oDatos.DepuraMarcacionesMasivas(sn);
        //    }
        //    catch (clsDataBaseException errBdd)
        //    {
        //        throw new clsLogicaException(errBdd.DataErrorDescription);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new clsLogicaException(ex.Message);
        //    }
        //}

        public void guardaMarcacionesBulk(DataTable dt, string sn)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.IniciaGrabacionMarcaciones(sn);
                oDatos.CargaMasivaSqlBulkCopy(dt, "tmp_Marcaciones", sn);
                oDatos.DepuraMarcacionesMasivas(sn);
            }
            // oDatos.guardaMarcacionesMasivas(dt, sn)
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message.Replace("'", "*"));
            }
        }

        public void GuardaUsuarios(List<DataGridViewRow> dgvUserinfo)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.GuardaUsuarios(dgvUserinfo);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void GuardaUsuariosMasivos(DataGridView dgvUserinfo)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.GuardaUsuariosMasivos(dgvUserinfo);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public DataTable GrabacionMarcacionesYUsuariosNuevosEnTablasDefinitivas(string sn)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.EliminaMarcacionesDuplicadas(sn);
                return oDatos.GrabaMarcacionesYUsuariosNuevosEnTablasDefinitivas(sn);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void RegistraLogEventoBdd(int orden, string sn, int idProceso, string sTarea, string sTareaDetallada)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.RegistraLogEventoBdd(orden, sn, idProceso, sTarea, sTareaDetallada);
            }
            catch(clsDataBaseException dbEx)
            {
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void CargaMasivaSqlBulkCopy(DataTable dt, string nombreTabla)
        //{
        //    clsDatosSDK oConex = new clsDatosSDK();
        //    oConex.CargaMasivaSqlBulkCopy(dt, nombreTabla);
        //}

        public int iniciaProcesoMasivo(int idAministrador)
        {
            clsDatosSDK oConex = new clsDatosSDK();
            int N = oConex.iniciaProcesoMasivo(idAministrador);
            return N;
        }

        public DataTable DtReporteDescargas(DateTime fDesde, DateTime fHasta)
        {
            DataTable dt = new DataTable();
            clsDatosSDK oDatos = new clsDatosSDK();
            return oDatos.DtReporteDescargas(fDesde, fHasta.AddDays(1));
        }

        public void IniciaGrabacionRostros()
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.IniciaGrabacionRostros();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void guardaRostros(List<DataGridViewRow> dgv)
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.guardaRostros(dgv);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

        public void FinalizaGrabacionRostros()
        {
            try
            {
                clsDatosSDK oDatos = new clsDatosSDK();
                oDatos.FinalizaGrabacionRostros();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }

    }
}




