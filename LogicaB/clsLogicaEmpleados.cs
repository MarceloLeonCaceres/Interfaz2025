using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using ConexionDatos;
using DatosB;
using Utilitarios;

namespace LogicaB
{
    public static class clsLogicaEmpleados
    {
        public static DataTable RetornaBadgeNombre()
        {
            try
            {
                return clsDatosEmpleados.RetornaBadgeNombre();
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

        public static DataTable RetornaIdBadgeNombreSsn(string sBusqueda)
        {
            try
            {
                if(sBusqueda == "")
                {
                    return clsDatosEmpleados.RetornaIdBadgeNombreSsn();
                }
                else
                {
                    return clsDatosEmpleados.RetornaIdBadgeNombreSsn(sBusqueda);
                }
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
        public static DataTable GetEmpleadosSinHuellaNiRostro()
        {
            try
            {
                return clsDatosEmpleados.GetEmpleadosSinHuellaNiRostro();
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
        public static DataTable GetTodosEmpleadosParaEnviarAReloj()
        {
            try
            {
                return clsDatosEmpleados.GetTodosEmpleadosParaEnviarAReloj();
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
        
        public static void AgregaNuevoEmpleado()
        {
            string sMaxCodigo = clsDatosEmpleados.RetornaMaxCodigo();
            long lMaxCodigo = long.Parse(sMaxCodigo);
            clsDatosEmpleados.IngresaNuevoEmpleado((lMaxCodigo + 1).ToString(), "Empleado " + (lMaxCodigo + 1).ToString());
        }

        public static void EliminaUsuario(int id)
        {
            clsDatosEmpleados.EliminaUsuario(id);
            clsDatosEmpleados.EliminaHuellas(id);
            clsDatosEmpleados.EliminaRostro(id);
            clsDatosEmpleados.EliminaMarcaciones(id);
        }

        public static int EditaUsuario(int idUsuario, string lBadgenumber,  string sNombre, string sSsn)
        {
            int n = 0;
            n = Int32.Parse(clsDatosEmpleados.ExisteOtroBadgenumber(idUsuario, lBadgenumber.ToString()));
            if (n >= 1)
                return -1;
            n = Int32.Parse(clsDatosEmpleados.ExisteOtroNombre(idUsuario, sNombre));
            if (n >=1 )
                return -2;
            if (!string.IsNullOrWhiteSpace(sSsn))
            {
                n = Int32.Parse(clsDatosEmpleados.ExisteOtroSsn(idUsuario, sSsn));
                if (n >= 1)
                    return -3;
            }

            if (clsDatosEmpleados.EditaUsuario(idUsuario, lBadgenumber, sNombre, sSsn))
                return 0;

            return -4;
        }

        
        public static void EnceraRegEmpleadosCsv()
        {
            try
            {
                clsDatosEmpleados.EnceraRegEmpleadosCsv();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public static void guardaEmpleadosCsv(List<Tuple<string, string, string>> lista)
        {
            try
            {
                clsDatosEmpleados.guardaEmpleadosCsv(lista);
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public static int IngresaEmpleadosCsv()
        {
            int n = 0;
            try
            {
                n = Int32.Parse(clsDatosEmpleados.CuentaDuplicadosBadge());
                if (n > 0)
                    return -1;
                n = Int32.Parse(clsDatosEmpleados.CuentaDuplicadosNombre());
                if (n > 0)
                    return -2;
                n = Int32.Parse(clsDatosEmpleados.CuentaDuplicadosAlterno());
                if (n > 0)
                    return -3;
                return clsDatosEmpleados.IngresaEmpleadosCsv();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
