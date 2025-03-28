using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Utilitarios;
using ConexionDatos;
using DatosB.Models;

namespace DatosB
{
    public static class clsDatosEmpleados
    {
        public static DataTable RetornaBadgeNombre()
        {
            string consulta;
            consulta = @"SELECT BADGENUMBER as Badge, NAME as Nombre FROM USERINFO";

            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public static DataTable RetornaIdBadgeNombreSsn()
        {
            string consulta;
            consulta = @"SELECT Userid, BADGENUMBER as Codigo, [NAME] as Nombre, SSN as [Codigo Alterno] FROM USERINFO";

            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public static DataTable RetornaIdBadgeNombreSsn(string sBusqueda)
        {
            string consulta;
            consulta = @"SELECT Userid, BADGENUMBER as Codigo, [NAME] as Nombre, SSN as [Codigo Alterno] FROM USERINFO
                WHERE [Name] like '%" + sBusqueda + "%';";

            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public static string RetornaMaxCodigo()
        {
            string consulta;
            consulta = @"SELECT MAX ( cast(badgenumber as bigint) ) FROM USERINFO";

            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static void IngresaNuevoEmpleado(string badgeNumber, string nombre)
        {
            string consulta;
            consulta = @"INSERT INTO USERINFO (Badgenumber, [Name])
                VALUES ( '" + badgeNumber + "', '" + nombre + "')";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public static void EliminaUsuario(int id)
        {
            string consulta;
            consulta = @"DELETE FROM USERINFO WHERE USERID = " + id.ToString() + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public static void EliminaHuellas(int id)
        {
            string consulta;
            consulta = @"DELETE FROM TEMPLATE WHERE USERID = " + id.ToString() + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public static void EliminaRostro(int id)
        {
            string consulta;
            consulta = @"DELETE FROM FaceTemp WHERE USERID = " + id.ToString() + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public static void EliminaMarcaciones(int id)
        {
            string consulta;
            consulta = @"DELETE FROM CheckInOut WHERE USERID = " + id.ToString() + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public static string ExisteOtroBadgenumber(int id, string badge)
        {
            string consulta;
            consulta = @"SELECT COUNT(*) FROM USERINFO 
                WHERE USERID <> " + id.ToString() + " AND BADGENUMBER = '" + badge + "'";

            return ClsAccesoDatos.EjecutaEscalar(consulta);            
        }


        public static string ExisteOtroNombre(int id, string nombre)
        {
            string consulta;
            consulta = @"SELECT COUNT(*) FROM USERINFO 
                WHERE USERID <> " + id.ToString() + " AND [Name] = '" + nombre + "'";

            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static string ExisteOtroSsn(int id, string Ssn)
        {
            string consulta;
            consulta = @"SELECT COUNT(*) FROM USERINFO 
                WHERE USERID <> " + id.ToString() + " AND Ssn = '" + Ssn + "'";

            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static bool EditaUsuario(int id, string lBadgenumber, string sNombre, string sSsn)
        {
            string consulta;
            consulta = @"UPDATE USERINFO
                SET Badgenumber = '"+ lBadgenumber + "', Name = '"+ sNombre + "', SSN = '"+ sSsn + "' " +
                "WHERE USERID = " + id.ToString();

            try
            {
                ClsAccesoDatos.EjecutaNoQuery(consulta);
                return true;
            }
            catch (Exception error)
            {
                return false;
            }
        }

        public static void EnceraRegEmpleadosCsv()
        {
            string consulta = "Delete from tmp_BADGES WHERE idDepto = -1";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }
        public static void guardaEmpleadosCsv(List<Tuple<string, string, string>> lista)
        {
            StringBuilder queryHuellas = new StringBuilder("");
            string consulta = string.Empty;

            foreach (var registro in lista)
            {
                queryHuellas.Append("INSERT INTO [tmp_BADGES] ");
                queryHuellas.Append("( [Badgenumber], [Nombre], idDepto, [alterno]) ");
                consulta = "VALUES('" + registro.Item1 + "', '" + registro.Item2 + "', -1, '" + registro.Item3 + "');" + "\n";
                queryHuellas.Append(consulta);
            }
            try
            {
                ClsAccesoDatos.EjecutaNoQuery(queryHuellas.ToString());
            }
            catch (clsDataBaseException error)
            {
                throw new Utilitarios.clsDataBaseException(error.DataErrorDescription);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public static string CuentaDuplicadosBadge()
        {
            string consulta = @"select count(*) from tmp_BADGES
                group by badgenumber
                having count(badgenumber) > 1";
            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static string CuentaDuplicadosNombre()
        {
            string consulta = @"select count(*) from tmp_BADGES
                group by nombre
                having count(nombre) > 1";
            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static string CuentaDuplicadosAlterno()
        {
            string consulta = @"select count(*) from tmp_BADGES where not alterno is null and alterno <> ''
                group by alterno
                having count(alterno) > 1";
            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static int IngresaEmpleadosCsv()
        {
            int n = -4;

            try
            {
                // Ingresa Nuevos ConsultasDapper
                string consulta = @"DECLARE @Depto int = 1;
                SELECT @Depto = MIN(DEPTID) FROM DEPARTMENTS WHERE DEPTID > 0;
                INSERT INTO USERINFO(Badgenumber, [Name], SSN, DEFAULTDEPTID) 
                    SELECT tmp.badgenumber, tmp.nombre, tmp.alterno, @Depto
                    FROM tmp_BADGES tmp left join USERINFO U on tmp.badgenumber = U.Badgenumber where u.Badgenumber is null";

                n = ClsAccesoDatos.iEjecutaNoQuery(consulta);

            }
            catch (clsDataBaseException error)
            {
                throw new Utilitarios.clsDataBaseException(error.DataErrorDescription);
            }
            catch (Exception error)
            {
                throw error;
            }

            return n;
        }

        public static DataTable GetEmpleadosSinHuellaNiRostro()
        {
            string query = @"SELECT U.Badgenumber, U.[NAME], U.cardNo, U.emPrivilege, U.mverifyPass
                FROM (USERINFO U left join TEMPLATE FP ON U.USERID = FP.USERID)
	                left join FaceTemp ON U.USERID = FaceTemp.UserID
                WHERE FP.USERID is null AND FaceTemp.UserID is null AND DEFAULTDEPTID > 0";
            return ClsAccesoDatos.RetornaDataTable(query);
        }

        public static DataTable GetTodosEmpleadosParaEnviarAReloj()
        {
            string query = @"SELECT Badgenumber, [NAME], cardNo,  
                case when cardNo > 0 then '*****' else '' end as TarjetaProximidad, 
		        mverifyPass, case when Len(mverifyPass) > 0 then '*****' else '' end as [Contrasenia], 
                privilege, emPrivilege
                FROM USERINFO
                WHERE DEFAULTDEPTID > 0";
            return ClsAccesoDatos.RetornaDataTable(query);
        }
    }
}
