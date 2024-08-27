using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConexionDatos;

namespace DatosB
{


    public class clsDatosDepartamentos
    {

        // ConexionDatos.ClsAccesoDatos ClsAccesoDatos = new ConexionDatos.ClsAccesoDatos();

        public clsDatosDepartamentos()
        {
        }

        public DataTable RetornaDtDepartamentosArbol(int tipoAdministrador, string codAdministrador)
        {
            string strConsulta = string.Empty;
            switch (tipoAdministrador)
            {
                case 0:
                    {
                        // Super Administrador
                        strConsulta = "SELECT DeptId as codNodo, DeptName as NomDepto, SupDeptId as codPadre " + "FROM DEPARTMENTS " + "ORDER BY SUPDEPTID, DEPTNAME;";
                        break;
                    }

                case 1:
                    {
                        // Administrador de Departamento
                        strConsulta = "SELECT userinfo.userid, userinfo.badgenumber, userinfo.defaultdeptid, departments.deptid, departments.deptname, userinfo.OTAdmin, userinfo.OTPrivAdmin, userinfo.OTPassword " + "FROM userinfo INNER JOIN departments ON departments.deptid = userinfo.defaultdeptid " + "WHERE userinfo.badgenumber='" + codAdministrador + "' " + "ORDER BY SUPDEPTID, DEPTNAME;";
                        break;
                    }

                case 2:
                    {
                        // Administrador de Local
                        strConsulta = "SELECT userinfo.userid, userinfo.badgenumber, userinfo.defaultdeptid, departments.deptid, departments.deptname, userinfo.OTAdmin, userinfo.OTPrivAdmin, userinfo.OTPassword " + "FROM userinfo INNER JOIN departments ON departments.deptid = userinfo.defaultdeptid " + "WHERE userinfo.badgenumber='" + codAdministrador + "' " + "ORDER BY SUPDEPTID, DEPTNAME;";
                        break;
                    }
            }

            DataTable dt = new DataTable();
            // // // clsConexionBdd conexion = new clsConexionBdd();
            dt = ClsAccesoDatos.RetornaDataTable(strConsulta);
            return dt;
        }

        public void IngresaDepartamento(string nomDepto, int codPadre, string codAdminLog = "-1")
        {
            string consulta;
            // clsConexionBdd objConexion = new clsConexionBdd();
            consulta = "INSERT INTO Departments (deptname, supdeptid) " + "VALUES ('" + nomDepto + "','" + codPadre + "')";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            ClsAccesoDatos.EjecutaNoQuery("INSERT INTO SystemLog VALUES('" + codAdminLog + "', GETDATE(), 'ProperTime', 0, 'Creación Departamentos', '" + nomDepto + "');");
        }

        public int cuentaDeptosHijos(int codDepto)
        {
            string consulta;
            consulta = "Select count(*) from departments " + "WHERE SupDeptId='" + codDepto + "'";

            // clsConexionBdd objConexion = new clsConexionBdd();
            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }


        public void EliminaDepto(int codDepto, string codAdminLog = "-1")
        {
            string consulta;
            // clsConexionBdd objConexion = new clsConexionBdd();

            consulta = ClsAccesoDatos.EjecutaEscalar("select DeptName from departments where deptid = '" + codDepto + "'").ToString();
            ClsAccesoDatos.EjecutaNoQuery("INSERT INTO SystemLog VALUES('" + codAdminLog + "', GETDATE(), 'ProperTime', 0, 'Elimina Departamentos', '" + consulta + "');");

            consulta = "UPDATE userinfo set defaultdeptid=(select min(deptid) from DEPARTMENTS) " + "\n" + "where defaultdeptid='" + codDepto + "'";
            consulta = consulta + "\n";
            consulta = consulta + "DELETE from departments where deptid='" + codDepto + "'";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void ActualizaDepartamento(int codDepto, string nomDepto, string codAdminLog = "-1")
        {
            string consulta;
            // clsConexionBdd objConexion = new clsConexionBdd();

            consulta = "UPDATE Departments set DeptName='" + nomDepto + "' where deptId='" + codDepto + "'";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            ClsAccesoDatos.EjecutaNoQuery("INSERT INTO SystemLog VALUES('" + codAdminLog + "', GETDATE(), 'ProperTime', 0, 'Cambia nombre Departamentos', '" + nomDepto + "');");
        }

        public void TransfiereDepartamento(int codDepto, List<int> listaEmps, string codAdminLog = "-1")
        {
            string consulta = string.Empty;
            List<string> aux = new List<string>();
            // clsConexionBdd objConexion = new clsConexionBdd();
            DataTable dt = new DataTable();

            foreach (int usuario in listaEmps)
            {
                consulta = consulta + "UPDATE Userinfo " + "SET DefaultDeptId = '" + codDepto + "' " + "WHERE userid=" + usuario + ";" + "\n";
                aux.Add(usuario.ToString());
            }
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            consulta = string.Join(",", aux);
                dt = ClsAccesoDatos.RetornaDataTable("Select NAME from userinfo " +
                    "where USERID in (" + consulta + ")");
            
            consulta = string.Empty;
            foreach (DataRow fila in dt.Rows)
                consulta = consulta + ", " + fila[0].ToString();

            ClsAccesoDatos.EjecutaNoQuery("INSERT INTO SystemLog VALUES('" + codAdminLog + "', GETDATE(), 'ProperTime', 0, 'Transfiere Empleados a Departamentos', '" + consulta + "');");
        }

        public DataTable RetornaLogo()
        {
            string consulta;
            consulta = "SELECT DEPTID, PHOTOD " + "FROM Departments " + "WHERE DeptId in (SELECT MIN(DeptId) from DEPARTMENTS WHERE DeptId > 0)";

            DataTable dt = new DataTable();
            // clsConexionBdd objConexion = new clsConexionBdd();

            dt = ClsAccesoDatos.RetornaDataTable(consulta);
            return dt;
        }


        public void updateLogoEmpresa(DataTable dtLogoEmpresa, string parametro)
        {
            SqlConnection cnn = new SqlConnection(ClsAccesoDatos.sConexion());
            SqlDataAdapter daAdapter = new SqlDataAdapter("Select DeptId, PHOTOd " + "FROM Departments;", cnn);
            SqlCommand comando = new SqlCommand();
            comando.Connection = cnn;
            daAdapter.SelectCommand = comando;

            comando.CommandText = "Select DeptId, PHOTOd " + "FROM Departments;";
            DataSet ds = new DataSet();

            SqlCommandBuilder comandBuilder = new SqlCommandBuilder(daAdapter);

            comandBuilder.QuotePrefix = "[";
            comandBuilder.QuoteSuffix = "]";

            daAdapter.Update(dtLogoEmpresa);
            dtLogoEmpresa.AcceptChanges();
        }

        public void EliminaLogo()
        {
            string consulta;
            consulta = "UPDATE Departments SET PHOTOd = NULL;";

            // clsConexionBdd objConexion = new clsConexionBdd();
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }
    }

}
