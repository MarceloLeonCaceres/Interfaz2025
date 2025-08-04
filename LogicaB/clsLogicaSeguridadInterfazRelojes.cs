using DatosB;
using System;
using System.Collections.Generic;
using System.Data;
using Utilitarios;

namespace LogicaB
{
    public static class clsLogicaSeguridadInterfazRelojes
    {
        public static bool Es_Terminal_Valida(string disco)
        {
            Utilitarios.ClsEncriptacion AuxCripto = new ClsEncriptacion();
            string discoEncriptado = AuxCripto.Encripta(disco);
            int i = Convert.ToInt32(clsDatosSeguridadInterfazRelojes.RetornaConteoTerminal(discoEncriptado));
            return i > 0;
        }

        public static bool EstaVigente()
        {
            DataTable dt = clsDatosSeguridadInterfazRelojes.FechasVigencia();
            if (dt.Rows.Count == 0) return false;

            string vigenciaEncriptada = dt.Rows[0][0].ToString();
            Utilitarios.ClsEncriptacion AuxCripto = new ClsEncriptacion();
            string sVigenciaDesencriptada = "";
            DateTime fechaVigencia ;
            DateTime fechaActual ;
            try
            {
                sVigenciaDesencriptada = AuxCripto.Desencripta(vigenciaEncriptada);

            }
            catch (Exception ex)
            {
                sVigenciaDesencriptada = AuxCripto.DesEncriptaFechaVigencia(vigenciaEncriptada);
            }
            finally
            {
                fechaVigencia = Convert.ToDateTime(sVigenciaDesencriptada);
                fechaActual = Convert.ToDateTime(dt.Rows[0][1].ToString());
            }
            return fechaActual < fechaVigencia;
        }

        public static bool EstaVigente(DateTime fechaActual)
        {
            DataTable dt = clsDatosSeguridadInterfazRelojes.FechasVigencia();
            Utilitarios.ClsEncriptacion AuxCripto = new ClsEncriptacion();
            if (dt.Rows.Count == 0) return false;
            DateTime fechaVigencia = Convert.ToDateTime(AuxCripto.Desencripta(dt.Rows[0][0].ToString()));
            return fechaActual < fechaVigencia;
        }

        public static bool HayModificacionesEnBdd()
        {
            DataSet ds = clsDatosSeguridadInterfazRelojes.PropiedadesTablas();

            DataTable dtCheckinout = ds.Tables[0];
            DataRow colMemoinfo = dtCheckinout.Rows[5];
            if (colMemoinfo["COLUMN_NAME"].ToString() != "Memoinfo" ||
                colMemoinfo["IS_NULLABLE"].ToString() != "NO" ||
                !string.IsNullOrEmpty(colMemoinfo["COLUMN_DEFAULT"].ToString()))
            {
                return true;
            }

            DataTable dtMachines = ds.Tables[1];
            DataRow colProductType = dtMachines.Rows[21];
            if (colProductType["COLUMN_NAME"].ToString() != "ProductType" ||
                colProductType["IS_NULLABLE"].ToString() != "NO" ||
                !string.IsNullOrEmpty(colProductType["COLUMN_DEFAULT"].ToString()))
            {
                return true;
            }

            return false;
        }

        public static bool HayModificacionesEnParametros()
        {
            // Fecha -> numeroRelojes, Hora_Minuto -> True
            (DataTable dt, string conteo) = clsDatosSeguridadInterfazRelojes.GetParametrosDeSeguridad();
            if (dt.Rows.Count != 2)
            {
                return true;
            }

            List<string> parametros = new List<string>();
            Utilitarios.ClsEncriptacion AuxCripto = new ClsEncriptacion();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                parametros.Add(AuxCripto.Desencripta(dt.Rows[i][0].ToString()));
            }
            if (!parametros.Contains("True"))
            {
                return true;
            }
            if (parametros[0].ToString() != conteo)
            {
                return true;
            }
            return false;

        }

        public static void RegistraModificacionesEnSistema()
        {
            Utilitarios.ClsEncriptacion AuxCripto = new ClsEncriptacion();
            string falseEncriptado = AuxCripto.Encripta("False");
            clsDatosSeguridadInterfazRelojes.RegistraModificacionesEnSistema(falseEncriptado);
        }

    }
}
