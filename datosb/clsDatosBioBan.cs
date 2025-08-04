using ConexionDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DatosB
{
    public class clsDatosBioBan
    {

        // ConexionDatos.ClsAccesoDatos oDatos = new ClsAccesoDatos();

        public DataTable dtUsuarioRelojS(string sBusqueda, string CodUniCompu)
        {
            ActualizaTablaPrincipal(CodUniCompu);
            ActualizaEmpleadosRelojesSecundarios(CodUniCompu);

            string consulta = "SELECT u.USERID, U.BADGENUMBER AS CodigoEmp, u.[NAME] AS NomEmpleadoR, u.DEFAULTDEPTID As idDepto, " +
                "MACHINES.ID as idReloj, MACHINES.MachineAlias as RelojSecundario, U_Sec.ID, case U_Sec.Admin when 1 then 'Si' else '' end as AdminSec " +
                "from (USERINFO U inner join UsersMachines U_Sec on U.USERID = U_Sec.USERID) " +
                "inner join MACHINES on U_Sec.DEVICEID = MACHINES.ID " +
                "where DEFAULTDEPTID > 0 ";
            if (sBusqueda != "")
            {
                consulta += "AND [NAME] like '%" + sBusqueda + "%'";
            }
            consulta += "\n" + "Order by NomEmpleadoR, CodigoEmp";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public void ActualizaEmpleadosRelojesSecundarios(string CodUniCompu)
        {
            // Elimina los ConsultasDapper-RelojesSecundarios porque se trata de Empleado-RelojPrincipal
            string consulta = "delete U_M " +
                "FROM UsersMachines U_M inner join TablaPrincipal" + CodUniCompu + " TP ON U_M.USERID = TP.USERID and U_M.DEVICEID = TP.idReloj;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }


        public void ActualizaTablaPrincipal(string CodUniCompu)
        {
            string consulta = "IF OBJECT_ID(N'TablaPrincipal" + CodUniCompu + "', N'U') IS NOT NULL " +
                "DROP TABLE TablaPrincipal" + CodUniCompu + ";";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            consulta = "CREATE TABLE TablaPrincipal" + CodUniCompu + " (" +
                "UserId int not null, " +
                "Badgenumber varchar(10) not null, " +
                "Nombre varchar(60), " +
                "idDepto int, " +
                "idReloj int, " +
                "MachineAlias varchar(80), " +
                "Motivo varchar(12), " +
                "EMPRIVILEGE int, " +
                "NumHuellas int);";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Con Reloj Principal
            consulta = "insert into TablaPrincipal" + CodUniCompu + " (USERID, BADGENUMBER, Nombre, idDepto, idReloj, MachineAlias, Motivo, EMPRIVILEGE) " +
                "Select U.USERID, U.Badgenumber, U.[Name] as Nombre, U.defaultDeptId, U.AccGroup as idReloj, " +
                "M.MachineAlias, 'P' AS Motivo, EMPRIVILEGE " +
                "FROM USERINFO U inner join MACHINES M ON u.AccGroup = M.ID; \n";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Sin Reloj Principal pero Con Departamento Asignado " +
            consulta = "insert into TablaPrincipal" + CodUniCompu + " (USERID, BADGENUMBER, Nombre, idDepto, idReloj, MachineAlias, Motivo, EMPRIVILEGE) " +
                "Select U.USERID, U.Badgenumber, U.[Name] as Nombre, U.defaultDeptId, M.ID, M.MachineAlias, 'D', EMPRIVILEGE " +
                "FROM (USERINFO U inner join[Dept-Machines] D_M ON U.DEFAULTDEPTID = D_M.DeptID) \n" +
                    "inner join MACHINES M on D_M.IdMachines = M.ID " +
                "WHERE U.USERID NOT IN (SELECT USERID FROM TablaPrincipal" + CodUniCompu + "); \n";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Sin Reloj Principal y Sin Departamento Asignado " +
            consulta = "insert into TablaPrincipal" + CodUniCompu + " (USERID, BADGENUMBER, Nombre, idDepto, Motivo) " +
                "Select U.USERID, U.Badgenumber, U.[Name] as Nombre, U.defaultDeptId, '' " +
                "FROM USERINFO U " +
                "WHERE U.USERID NOT IN (SELECT USERID FROM TablaPrincipal" + CodUniCompu + "); \n" +
                // Conteo de Huellas +
                "With ConteoHuellas AS( " +
                "select USERID, COUNT(*) AS NumHuellas from TEMPLATE GROUP BY USERID) " +
                "update TP " +
                "set TP.NumHuellas = ConteoHuellas.NumHUellas " +
                "from TablaPrincipal" + CodUniCompu + " TP inner join ConteoHuellas on TP.UserId = ConteoHuellas.Userid;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

        }

        public DataTable DtDepartamentosRelojes()
        {
            string consulta = "WITH Empleados_x_Depto as ( \n" +
                    "SELECT DEFAULTDEPTID, COUNT(*) AS ConteoDep_Users " +
                    "FROM USERINFO " +
                    "GROUP BY DEFAULTDEPTID) \n" +
                "SELECT Departments.DEPTID, CONCAT(DeptName, '  (', ConteoDep_Users, ')') as DeptName, SupDeptId, Machines.Id, Machines.MachineAlias, Machines.[IP], ConteoDep_Users \n" +
                    "FROM ((DEPARTMENTS LEFT JOIN[Dept-Machines] D_M ON DEPARTMENTS.DeptId = D_M.DeptId) " +
                    "LEFT JOIN MACHINES ON D_M.IdMachines = MACHINES.Id ) \n" +
                    "LEFT JOIN Empleados_x_Depto ON DEPARTMENTS.DEPTID = Empleados_x_Depto.DEFAULTDEPTID \n" +
                    "ORDER BY DEPTNAME, MachineAlias; ";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public DataTable dtRelojes()
        {
            string consulta = "Select ID, MachineAlias, IP, Port as Puerto, MachineNumber, sn FROM MACHINES ORDER BY MachineAlias;";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public int AsignaDepto_Reloj(int iDepto, int iReloj)
        {
            string consulta = "delete from [Dept-Machines] where DeptID = " + iDepto.ToString() + "; \n";
            consulta += "insert into [Dept-Machines] (DeptID, IdMachines) values(" + iDepto.ToString() + ", " + iReloj.ToString() + ");";
            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public int AsignaEmpleado_Depto(List<int> lstUsuarios, int iDepto)
        {
            string consulta = "UPDATE USERINFO SET ACCGroup = NULL, DefaultDeptId = " + iDepto.ToString() +
                " where UserID in (" + string.Join(",", lstUsuarios) + ");";
            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public int AsignaEmpleado_RelojPrincipal(List<int> lstUsuarios, int iReloj)
        {
            string consulta = "UPDATE USERINFO SET AccGroup = " + iReloj.ToString() +
                " where UserID in (" + string.Join(",", lstUsuarios) + ");";
            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public int EliminaEmpleadoRelojSecundario(List<int> lstEmpleadosRelojesSecundarios)
        {
            string consulta = "Delete from UsersMachines WHERE ID in ( " + string.Join(", ", lstEmpleadosRelojesSecundarios) + ") \n";

            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public int PoneQuitaAdminRelojPrincipal(List<int> lstUsuarios, int pone1_quita0)
        {
            string consulta = "Update USERINFO SET EMPRIVILEGE = " + pone1_quita0.ToString() + " WHERE USERID IN (" + String.Join(", ", lstUsuarios) + ") \n";

            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public int PoneQuitaAdminRelojSecundario(List<int> lstEmpleadosRelojesSecundarios, int pone1_quita0)
        {

            string consulta = "Update [UsersMachines] SET Admin = " + pone1_quita0.ToString() + " WHERE ID IN (" + String.Join(", ", lstEmpleadosRelojesSecundarios) + ") \n";
            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }


        public void TruncaTablas(List<string> lstTablas)
        {
            string consulta = "";
            foreach (string nomTabla in lstTablas)
            {
                consulta += "Truncate Table " + nomTabla + "; \n";
            }
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void CargaSqlBulkCopy(DataTable dt, string nombreTabla)
        {
            using (SqlConnection con = new SqlConnection(ClsAccesoDatos.sConexion()))
            {
                try
                {
                    con.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null))
                    {
                        bulkCopy.DestinationTableName = nombreTabla;
                        bulkCopy.WriteToServer(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int MergeTablas(string tOrigen, List<string> colsOrigen, string tDestino, List<string> colsDestino)
        {

            if (colsOrigen.Count != colsDestino.Count) return -1;
            List<string> lstVariables = new List<string>();
            string consulta = "";
            for (int i = 0; i < colsDestino.Count; i++)
            {
                lstVariables.Add("TARGET." + colsDestino[i].ToString() + " = SOURCE." + colsOrigen[i].ToString());
            }
            consulta = "MERGE " + tDestino + " AS TARGET " +
                "USING " + tOrigen + " AS SOURCE \n" +
                   "ON " + string.Join(" And ", lstVariables) + "\n" +
                // --[WHEN MATCHED THEN
                // -- < accion cuando coinciden > ]   No hace nada, ya está en la base de datos
                "WHEN NOT MATCHED BY TARGET THEN " +
                    "INSERT(USERID, DEVICEID) VALUES(SOURCE.USERID, SOURCE.DEVICEID);";
            // --[WHEN NOT MATCHED BY SOURCE THEN
            // -- < accion cuando no coinciden por origen > ]; Tampoco hace nada, no se debe ingresar"

            return ClsAccesoDatos.iEjecutaNoQuery(consulta);
        }

        public DataTable HuellasParaReloj(int idReloj)
        {
            // Usuarios secundarios del reloj
            string consulta = "SELECT u.BADGENUMBER, 0 as [Enable], u.[NAME] as Nombre, isnull(CardNo, 0) as CardNo, u.[PASSWORD], " +
                "T.FINGERID as FingerIndex, T.Flag, T.sca_FPrint as FP, " +
                "isnull(u.EmPrivilege, 0) as Privilegio \n" +
                "FROM (USERINFO U INNER JOIN TEMPLATE T ON U.USERID = T.USERID) " +
                     "INNER JOIN UsersMachines on U.USERID = UsersMachines.USERID \n" +
                "WHERE DEVICEID = " + idReloj.ToString() + "\n" +
                "Union \n" +
                // Usuarios Principales del reloj
                "SELECT u.BADGENUMBER, 0 as [Enable], u.[NAME] as Nombre, isnull(CardNo, 0) as CardNo, u.[PASSWORD], T.FINGERID as FingerIndex, T.Flag, T.sca_FPrint as FP, isnull(u.EmPrivilege, 0) as Privilegio \n" +
                "FROM(USERINFO U INNER JOIN TEMPLATE T ON U.USERID = T.USERID) \n" +
                "WHERE U.AccGroup = " + idReloj.ToString() + ";";
            return ClsAccesoDatos.RetornaDataTable(consulta);

        }

        public DataTable DtEmpleadosPrincipalesDelReloj(int idReloj, string codCompu)
        {
            ActualizaTablaPrincipal(codCompu);
            string consulta = "SELECT Badgenumber, UserId, Nombre, idDepto, idReloj, MachineAlias, Motivo, EMPrivilege " +
                "FROM TablaPrincipal" + codCompu + " " +
                "WHERE idReloj = " + idReloj.ToString() + ";";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public DataTable dtEmpleadosDepartamentos()
        {
            string consulta = "select U.BADGENUMBER, U.NAME, CASE U.DEFAULTDEPTID WHEN -1 THEN 'Inactivo' ELSE DEPTNAME END AS DEPARTMENT " +
                "from USERINFO u LEFT JOIN DEPARTMENTS D ON U.DEFAULTDEPTID = D.DEPTID";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public void guardaHuellas(DataGridView dgv)
        {
            string queryHuellas = String.Empty;
            string consulta = String.Empty;
            ClsAccesoDatos.EjecutaNoQuery("TRUNCATE TABLE HUELLAS;");

            // 0     BadgeNumber
            // 1     Enable
            // 2     Nombre
            // 3     CardNo  Por ahora no estamos grabando
            // 4     Password
            // 5     FingerIndex
            // 6     Flag
            // 7     FP
            // 8     Privilegio

            // Para ingresar las huellas del dgv a la tabla de huellas temporal
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                queryHuellas = queryHuellas +
                    "INSERT INTO [HUELLAS] (Badge, sName, sPassword, iPrivilege, bEnabled, FingerId, Flag, Template, Template1) VALUES('" +
                    fila.Cells[0].Value + "', '" +
                    fila.Cells[2].Value + "', '" +
                    fila.Cells[4].Value + "', " +
                    fila.Cells[8].Value + ", '" +
                    fila.Cells[1].Value + "', " +
                    fila.Cells[5].Value + ", '" +
                    fila.Cells[6].Value + "', '" +
                    fila.Cells[7].Value + "', '" +
                    fila.Cells[7].Value + "');" + "\n";
            }

            ClsAccesoDatos.EjecutaNoQuery(queryHuellas);

        }

        public void FinalizaGrabacionHuellas(int idReloj)
        {
            string consulta;
            int filasAfectadas;
            // Toma el primer depto disponible, (que en general va a ser el departamento raiz)
            // Selecciona los usuarios distintos de la tabla Huellas
            // e ingresa los usuarios nuevos a la tabla userinfo, con lo que se garantiza
            // que los usuarios nuevos tendrán su UserId
            // el dato bEnabled va a la columna EMPRIVILEGE
            consulta = "DECLARE @depto integer; \n" +
                "if (select count(*) from [Dept-Machines] where idMachines = " + idReloj.ToString() + " ) > 0 \n" +
                    "SELECT @depto = min(deptId) from [Dept-Machines] where idMachines = " + idReloj.ToString() + "; \n" +
                "else \n" +
                    "SELECT @depto = min(deptId) from DEPARTMENTS where DEPTID > 0; \n" +
                 "with UsuariosReloj as ( " +
                "SELECT badge, min(isnull(nullif(sName, ''), BADGE)) as Nombre, min(sPassword) as sPassword, min(iPrivilege) as iPrivilege, min(bEnabled) as bEnabled " +
            "FROM huellas " +
            "GROUP BY BADGE " +
                ") " +
                "INSERT INTO USERINFO(BADGENUMBER, [NAME], DEFAULTDEPTID, [PASSWORD], privilege, EMPRIVILEGE, AccGroup) " +
                "select R.BADGE, R.Nombre, @depto, R.sPassword, R.iPrivilege, R.bEnabled, '" + idReloj.ToString() + "' " +
                "FROM UsuariosReloj R left join Userinfo U ON R.badge = U.badgenumber " +
                "where u.userid is null";
            filasAfectadas = ClsAccesoDatos.iEjecutaNoQuery(consulta);

            // Elimina las huellas de la tabla Template, de los usuarios que tienen "nuevas" huellas 
            // ( no son necesariamente nuevas, pero son las que se descargaron recién)
            // porque serán reemplazados por las huellas recién descargadas
            consulta = "DELETE FROM template " +
                "WHERE USERID IN (SELECT distinct U.USERID " +
                "FROM huellas H inner join USERINFO U ON H.BADGE = U.BADGENUMBER); ";
            filasAfectadas = ClsAccesoDatos.iEjecutaNoQuery(consulta);

            // Inserta las huellas en la tabla TEMPLATE
            // Las Tablas TEMPLATE y HUELLAS tienen la estructura adecuada
            // están en la base "BaseSinDatos y en la Base10 iGeo"
            consulta = "INSERT INTO TEMPLATE (USERID, FINGERID, TEMPLATE, sca_FPrint, Flag) " +
                "SELECT U.USERID, H.FINGERID, H.TEMPLATE, H.TEMPLATE1, H.Flag " +
                "FROM Huellas H INNER JOIN USERINFO U ON H.BADGE = u.BADGENUMBER; ";
            filasAfectadas = ClsAccesoDatos.iEjecutaNoQuery(consulta);

            ClsAccesoDatos.EjecutaNoQuery("TRUNCATE TABLE HUELLAS;");

        }

    }
}
