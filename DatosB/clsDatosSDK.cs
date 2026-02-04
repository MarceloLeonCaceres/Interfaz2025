using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ConexionDatos;
using DatosB.Models;
using Utilitarios;

namespace DatosB
{
    public class clsDatosSDK
    {
        public DataTable conteoUsersFP()
        {
            string consulta = @"select 'Userinfo' as Tabla, count(*) as Conteo from USERINFO
            union
            select 'Template', count(*) from TEMPLATE;";

            return ConexionDatos.ClsAccesoDatos.RetornaDataTable(consulta);

        }

        public void IniciaGrabacionHuellas()
        {
            string consulta = "TRUNCATE TABLE tmp_HUELLAS;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void GuardaHuellasEnTemporal(List<DataGridViewRow> dgv)
        {
            StringBuilder sbQueryHuellas = new StringBuilder();

            // 0     BadgeNumber
            // 1     Enable
            // 2     Nombre
            // 3     CardNo  Por ahora no estamos grabando
            // 4     Password / MVerifyPass
            // 5     FingerIndex
            // 6     Flag
            // 7     FP
            // 8     Privilegio

            foreach (DataGridViewRow fila in dgv)
            {
                sbQueryHuellas.AppendLine("INSERT INTO [tmp_HUELLAS] (Badge, sName, sPassword, iPrivilege, bEnabled, FingerId, Flag, Template, Template1) VALUES('" 
                    + fila.Cells[0].Value + "', '" + fila.Cells[2].Value + "', '" + fila.Cells[4].Value + "', " + fila.Cells[8].Value 
                    + ", '" + fila.Cells[1].Value + "', " + fila.Cells[5].Value + ", '" + fila.Cells[6].Value + "', '" + fila.Cells[7].Value + "', '" + fila.Cells[7].Value + "');");               
            }
            ClsAccesoDatos.EjecutaNoQuery(sbQueryHuellas.ToString());
        }
        public void ActualizaCreaDatosUsuarios(DataTable datosEmpleados)
        {
            StringBuilder sbQueryUsuarios = new StringBuilder();
            Utilitarios.ClsEncriptacion encripter = new Utilitarios.ClsEncriptacion();

            string sqlCreaTablaTemporal = @"CREATE TABLE #tmp_UsuariosBiometrico(
	            [pin] [nvarchar](24) NOT NULL,
	            [name] [nvarchar](60) NULL,
	            [mverifypass] [nvarchar](100) NULL,
	            [cardNumber] [nvarchar](20) NULL,
	            [privilege] [smallint] NULL,
	            [enprivilege] [smallint] NULL
            );" + "\n";

            for (int i = 0; i < datosEmpleados.Rows.Count; i++)
            {
                string nombre = string.IsNullOrWhiteSpace(datosEmpleados.Rows[i]["name"].ToString()) ? 
                    datosEmpleados.Rows[i]["pin"].ToString() :
                    datosEmpleados.Rows[i]["name"].ToString();
                string pwdEnReloj = encripter.Encripta(datosEmpleados.Rows[i]["password"].ToString());

                sbQueryUsuarios.AppendLine($@"INSERT INTO #tmp_UsuariosBiometrico 
                    ( pin, name, mverifypass, cardNumber, privilege, enprivilege ) VALUES (
                    '{datosEmpleados.Rows[i]["pin"].ToString()}', '{nombre}', '{pwdEnReloj}', 
                    '{datosEmpleados.Rows[i]["cardNumber"]}', {datosEmpleados.Rows[i]["privilege"]}, {datosEmpleados.Rows[i]["enprivilege"]} );");
            }


            string sqlMergeUserinfo = @"SET NOCOUNT ON;
MERGE USERINFO AS Target
    USING #tmp_UsuariosBiometrico AS Source on Target.Badgenumber = Source.pin
WHEN MATCHED THEN 
    UPDATE SET Target.mverifypass = Source.mverifypass, Target.cardNo = Source.cardNumber, Target.Privilege = Source.privilege, Target.emPrivilege = Source.enprivilege
WHEN NOT MATCHED THEN
    INSERT (Badgenumber, name, mVerifyPass, cardNo, EMPrivilege, privilege, DefaultDeptId)
    VALUES (Source.pin, Source.name, Source.mverifypass, Source.cardNumber, Source.enprivilege, Source.privilege,
                (select deptid from DEPARTMENTS where SUPDEPTID = 0) );";

            ClsAccesoDatos.EjecutaNoQuery(sqlCreaTablaTemporal + sbQueryUsuarios.ToString() + sqlMergeUserinfo);
        }

        public void FinalizaGrabacionHuellas()
        {

            string consulta;         

            // Elimina las huellas de la tabla Template, de los usuarios que tienen "nuevas" huellas ( no son necesariamente nuevas, pero son las que se descargaron recién)
            // porque serán reemplazados por las huellas recién descargadas
            consulta = @"DELETE FROM template
            WHERE USERID IN ( SELECT distinct U.USERID
	        FROM tmp_HUELLAS H inner join USERINFO U ON H.BADGE = U.BADGENUMBER );";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Inserta las huellas en la tabla TEMPLATE
            // Las Tablas TEMPLATE y HUELLAS tienen la estructura adecuada
            // están en la base "BaseSinDatos"
            consulta = @"INSERT INTO TEMPLATE (USERID, FINGERID, TEMPLATE, TEMPLATE1, Flag)
            SELECT U.USERID, H.FINGERID, H.TEMPLATE, H.TEMPLATE1, H.Flag
	        FROM tmp_HUELLAS H INNER JOIN USERINFO U ON H.BADGE = u.BADGENUMBER;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            ClsAccesoDatos.EjecutaNoQuery("TRUNCATE TABLE tmp_HUELLAS;");
        }

        public DataTable RetornaHuellas(string sBusqueda)
        {

            string consulta;
            consulta = @"SELECT u.BADGENUMBER, isnull(U.EMPRIVILEGE, 1) as [Enable], u.[NAME] as Nombre, isnull(CardNo, 0) as CardNo, u.[mverifypass] as Password,
            T.FINGERID as FingerIndex, T.Flag, T.TEMPLATE1 as FP,
            isnull(u.privilege, 0) as Privilegio, DEPTNAME AS Departamento
            FROM (USERINFO U INNER JOIN TEMPLATE T ON U.USERID = T.USERID)
				INNER JOIN DEPARTMENTS ON U.DEFAULTDEPTID = DEPARTMENTS.DEPTID
            WHERE U.[Name] LIKE '%" + sBusqueda + "%'";

            return ConexionDatos.ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public DataTable RetornaRostros(string sBusqueda)
        {

            string consulta;
            /*
             *  0:  Badgenumber
             *  1:  Enable
             *  2:  Nombre
             *  3:  Card
             *  4:  Password
             *  5:  Size
             *  6:  Template
             *  7:  Privilegio
             *  8:  Departamento
             * */
            consulta = @"SELECT u.BADGENUMBER, isnull(U.EMPRIVILEGE, 1) as [Enable], u.[NAME] as Nombre, isnull(CardNo, 0) as CardNo, u.[PASSWORD],
            T.Size, T.TEMPLATE1 as FC,
            isnull(u.privilege, 0) as Privilegio, DEPTNAME AS Departamento
            FROM (USERINFO U INNER JOIN FaceTemp T ON U.USERID = T.USERID)
				INNER JOIN DEPARTMENTS ON U.DEFAULTDEPTID = DEPARTMENTS.DEPTID
            WHERE U.[Name] LIKE '%" + sBusqueda + "%'";

            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public void GuardaUsuarios(List<DataGridViewRow> dgv_usuarios)
        {
            StringBuilder sbQueryBadgeNames = new StringBuilder();

            foreach (DataGridViewRow fila in dgv_usuarios)
                sbQueryBadgeNames.AppendLine("INSERT INTO [tmp_BADGES] (badgenumber, nombre) VALUES('" + fila.Cells["sEnrollNumber"].Value + "', '" + fila.Cells["sName"].Value + "');");

            ClsAccesoDatos.EjecutaNoQuery(sbQueryBadgeNames.ToString());
        }

        public void GuardaUsuariosMasivos(DataGridView dgv_usuarios)
        {
            StringBuilder sbQueryBadgeNames = new StringBuilder();
         
            foreach (DataGridViewRow fila in dgv_usuarios.Rows)
                sbQueryBadgeNames.AppendLine("INSERT INTO [tmp_BADGES] (badgenumber, nombre) VALUES('" + fila.Cells["sEnrollNumber"].Value + "', '" + fila.Cells["sName"].Value + "');");

            ClsAccesoDatos.EjecutaNoQuery(sbQueryBadgeNames.ToString());
        }

        public void InicializaTablasDescargaTemporales()
        {
            ClsAccesoDatos.EjecutaNoQuery($@"TRUNCATE TABLE tmp_MARCACIONES;
TRUNCATE TABLE tmp_BADGES;");
        }
        public void IniciaGrabacionMarcaciones(string sn)
        {
            ClsAccesoDatos.EjecutaNoQuery($"DELETE FROM tmp_MARCACIONES  WHERE sn = '{sn}' or sn is null or [User ID] = 0 or [User ID] = '0';" + "\n" + "DELETE FROM tmp_BADGES WHERE idDepto is null;");
        }

        public void GuardaSoloMarcacionesTemporales(List<DataGridViewRow> dgv_logs, string sn)
        {
            StringBuilder sbQueryMarcaciones = new StringBuilder();
            foreach (DataGridViewRow fila in dgv_logs)
                sbQueryMarcaciones.AppendLine(@"INSERT INTO [tmp_MARCACIONES] 
([User ID], [Verify Date], [Verify Type], [Verify State], workCode, sn) 
VALUES('" + fila.Cells["User ID"].Value + "', '" + fila.Cells["Verify Date"].Value + "', '" + fila.Cells["Verify Type"].Value + "', "
+ fila.Cells["Verify State"].Value + ", " + fila.Cells["WorkCode"].Value + $", '{sn}');");

            ClsAccesoDatos.EjecutaNoQuery(sbQueryMarcaciones.ToString());

            string consulta = $"UPDATE tmp_MARCACIONES SET SENSORID = (SELECT Top 1 MachineNumber FROM Machines WHERE sn = '{sn}') ;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

        }

        //public void GuardaSoloMarcacionesTemporales(DataGridView dgv, string sn, int inicio, int fin)
        //{
        //    StringBuilder sbQueryMarcaciones = new StringBuilder();
        //    for(int i = inicio; i <= fin; i++)
        //    {
        //        sbQueryMarcaciones.AppendLine($@"INSERT INTO [tmp_MARCACIONES] ([User ID], [Verify Date], [Verify Type], [Verify State], workCode) 
        //    VALUES('" + dgv.Rows[i].Cells["User ID"].Value + "', '" + dgv.Rows[i].Cells["Verify Date"].Value + "', '" + dgv.Rows[i].Cells["Verify Type"].Value 
        //    + "', " + dgv.Rows[i].Cells["Verify State"].Value + ", " + dgv.Rows[i].Cells["WorkCode"].Value + ");");
        //    }
        //    ClsAccesoDatos.EjecutaNoQuery(sbQueryMarcaciones.ToString());

        //    string consulta = "UPDATE tmp_MARCACIONES SET sn = '" + sn + "', SENSORID = (SELECT MachineNumber FROM Machines WHERE sn = '" + sn + "') WHERE sn is null;";
        //    ClsAccesoDatos.EjecutaNoQuery(consulta);

        //}
        public void GuardaMarcaciones(List<MarcacionRelojModel> dgv_logs, string sn)
        {
            StringBuilder sbInsertMarcaciones = new StringBuilder();

            foreach (var fila in dgv_logs)
            {
                sbInsertMarcaciones.AppendLine("INSERT INTO [tmp_MARCACIONES] ([User ID], [Verify Date], [Verify Type], [Verify State], workCode) VALUES('" + fila.UserID + "', '" + fila.VerifyDate.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + fila.VerifyType + "', " + fila.VerifyState + ", " + fila.WorkCode + ");" + "\n");
            }
            ClsAccesoDatos.EjecutaNoQuery(sbInsertMarcaciones.ToString());

            string setSnSensorid = "UPDATE tmp_MARCACIONES SET sn = '" + sn + "', SENSORID = (SELECT MachineNumber FROM Machines WHERE sn = '" + sn + "') WHERE sn is null;";
            ClsAccesoDatos.EjecutaNoQuery(setSnSensorid);

        }

        public void DepuraMarcacionesMasivas(string serialNumber)
        {
            string consulta;
            DataTable dt = new DataTable();
            string sDesde;
            string sHasta;

            // Elimina las marcaciones con badgenumber = 0
            consulta = @"DELETE FROM tmp_Marcaciones
                WHERE [User ID] = '0' or [User ID] = 0";
            ClsAccesoDatos.EjecutaNoQuery(consulta, 30);

            consulta = @"select dateadd(MINUTE, -1, min([Verify Date])) as Desde, dateadd(MINUTE, 1, max([Verify Date])) as Hasta 
            from tmp_MARCACIONES;";

            dt = ClsAccesoDatos.RetornaDataTable(consulta);
            sDesde = dt.Rows[0][0].ToString();
            sHasta = dt.Rows[0][1].ToString();

            // Elimina las marcaciones 'viejas' que ya estaban en la base de datos
            consulta = @"WITH MarcacionesViejas AS (
                SELECT U.BADGENUMBER, c.CHECKTIME
                FROM USERINFO U INNER JOIN CHECKINOUT C ON U.USERID = C.USERID 
                WHERE CHECKTIME BETWEEN '" + sDesde + "' AND '" + sHasta + @"')
            DELETE tmp_MARCACIONES
            FROM tmp_MARCACIONES inner join MarcacionesViejas 
        	ON tmp_MARCACIONES.[User ID] = MarcacionesViejas.badgenumber and tmp_MARCACIONES.[Verify Date] = MarcacionesViejas.Checktime;";
            ClsAccesoDatos.EjecutaNoQuery(consulta, 300);

        }

        public void EliminaMarcacionesDuplicadas(string sn)
        {
            string consulta;
            consulta = @"SELECT [User ID], [Verify Date], min([Verify Type]) as [Verify Type], 
                min([Verify State]) as [Verify State], 
		        min([WorkCode]) as [WorkCode], min([sn]) as [sn], min([SENSORID]) as [SENSORID]
                INTO #Dup
                  FROM [dbo].[tmp_Marcaciones]
                  group by [User ID], [Verify Date]
                  having count(*) > 1;

                  delete tmp_Marcaciones
                  from tmp_Marcaciones inner join #Dup 
	                on tmp_Marcaciones.[User ID] = #Dup.[User ID] and tmp_Marcaciones.[Verify Date] = #Dup.[Verify Date];

                INSERT INTO [dbo].[tmp_Marcaciones]
                ([User ID],[Verify Date],[Verify Type],[Verify State],[WorkCode],[sn],[SENSORID])
                select [User ID],[Verify Date],[Verify Type],[Verify State],[WorkCode],[sn],[SENSORID]
                from #Dup";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            consulta = $"select count(*) from tmp_Marcaciones where sn = '{sn}'";
            string numeroMarcaciones = ClsAccesoDatos.EjecutaEscalar(consulta);
            RegistraLogEventoBdd(0, sn, -1, "Ingreso Marcaciones", $"Por ingresar {numeroMarcaciones}");

        }

        public DataTable GrabaMarcacionesYUsuariosNuevosEnTablasDefinitivas(string sn)
        {

            string consulta; 
            DataTable dt;

            consulta = "IF OBJECT_ID(N'T_UsuariosUnicos', N'U') IS NOT NULL \n DROP TABLE T_UsuariosUnicos;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Toma los Usuarios Unicos con Marcaciones en #T_UsuariosUnicos
            consulta = $@"With UsuariosUnicosConMarcaciones AS (
                SELECT DISTINCT tmp_MARCACIONES.[User ID] 
                FROM tmp_MARCACIONES WHERE SN = '{sn}'
            )
            SELECT DISTINCT UsuariosUnicosConMarcaciones.[User ID], tmp_BADGES.nombre, tmp_BADGES.idDepto
            INTO T_UsuariosUnicos
            from UsuariosUnicosConMarcaciones LEFT JOIN tmp_BADGES ON UsuariosUnicosConMarcaciones.[User ID] = tmp_BADGES.BADGENUMBER;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Pone  badgenumber en nombre, cuando el empleado no tiene nombre
            consulta = @"update T_UsuariosUnicos set nombre = [User ID] 
            where nombre is null or nombre = '';";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Pone el 1er codigo disponible de departamento
            consulta = "UPDATE T_UsuariosUnicos set idDepto = (select deptid from DEPARTMENTS where SUPDEPTID = 0);";
            // @"DECLARE @depto integer;
            //Select @depto = DeptId from DEPARTMENTS where SUPDEPTID > 0;
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Para ingresar los nuevos usuarios con marcaciones
            consulta = @"INSERT INTO USERINFO (BADGENUMBER, [NAME], DEFAULTDEPTID)
            SELECT DISTINCT Nuevos.[User ID], Nuevos.nombre, idDepto
            from T_UsuariosUnicos Nuevos LEFT JOIN userinfo U ON Nuevos.[User ID] = U.BADGENUMBER
            WHERE U.USERID IS NULL;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);


            // Inserta las Marcaciones en la tabla CHECKINOUT
            // están en la base "BaseSinDatos"
            consulta = $@"WITH MarcacionesReloj AS (
            SELECT U.Userid, M.*
            FROM tmp_MARCACIONES M INNER JOIN USERINFO U ON M.[User ID] = U.Badgenumber where M.sn = '{sn}' 
            )" +
            @"INSERT INTO CHECKINOUT 
                    ( USERID,     CHECKTIME,        VERIFYCODE,       CHECKTYPE,        WorkCode,    sn,    SENSORID, Memoinfo)
            SELECT MR.USERID, MR.[Verify Date], MR.[VERIFY TYPE], 
                case when MR.[VERIFY STATE] > 9 then 'I' else T.checkType end, 
                MR.WORKCODE, MR.sn, mr.SENSORID, ''
            FROM MarcacionesReloj MR left join CheckTypes T on MR.[Verify State] = T.int;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            consulta = @"SELECT U.Badgenumber, U.[Name] as nombre, M.[Verify Date] as FechaHora
                FROM tmp_MARCACIONES M inner join USERINFO U on M.[User ID] = u.Badgenumber
                where M.sn = '" + sn + "';";
            dt = ConexionDatos.ClsAccesoDatos.RetornaDataTable(consulta);

            consulta = $@"DELETE FROM tmp_MARCACIONES WHERE sn='{sn}';
                          DELETE FROM tmp_BADGES where idDepto is null;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            return dt;
               
        }

        public void RegistraLogEventoBdd(int orden, string sn, 
            int idProceso, string sTarea, string sTareaDetallada)
        {
            try
            {

                ClsAccesoDatos.EjecutaNoQuery(@"INSERT INTO [da_DetalleDescarga] 
            ( [orden], [LogTime], [sn], [idProceso], [LogDescr], [LogDetailed])
            VALUES(" + orden + ", GETDATE(), '" + sn + "', " + idProceso + ", '" + sTarea + "', '" + sTareaDetallada + "');");
            }
            catch(clsDataBaseException dbEx)
            {
                throw dbEx;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CargaMasivaSqlBulkCopy(DataTable dt, string nombreTabla, string sn)
        {
            ClsAccesoDatos.CargaSqlBulkCopy(dt, nombreTabla, sn);
        }

        public int iniciaProcesoMasivo(int idAministrador)
        {
            List<string> lstConsultas = new List<string>();

            lstConsultas.Add(@"INSERT INTO da_ProcesosDescarga (Fecha, userId)
            VALUES (GETDATE(), " + idAministrador.ToString() + ");");

            string respuesta = ClsAccesoDatos.EjecutaEscalar("SELECT MAX(id) FROM da_ProcesosDescarga;", lstConsultas);
            return Int32.Parse(respuesta);
        }

        public DataTable DtReporteDescargas(DateTime fDesde, DateTime fHasta)
        {
            string queryDt;
            List<string> lstConsultas = new List<string>();

            lstConsultas.Add("IF OBJECT_ID(N'#tempDescargas', N'U') IS NOT NULL " + "DROP TABLE #tempDescargas;");

            lstConsultas.Add(@"SELECT idProceso, sn,
	            [1] as [Intento de Conexión], [2] AS [Conexión], [3] as [Lectura Marcaciones], [4] as [Intento Desconexión], [5] as [Desconexión], [6], [7] as [Grabación Marcaciones Nuevas], [8], [9] as [Error]
            INTO #tempDescargas
            FROM ( SELECT idProceso, sn, Convert(varchar(19), LogTime, 121) as LogTime, [orden]
		            FROM da_DetalleDescarga INNER JOIN da_ProcesosDescarga 
			            ON da_DetalleDescarga.idProceso = da_ProcesosDescarga.id 
			            where da_ProcesosDescarga.Fecha between '" + fDesde.ToShortDateString() + "' and '" + fHasta.ToShortDateString() + @"') AS TablaBase
            PIVOT 
            (
             min(LogTime) 
            FOR [orden] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9])
            ) AS PivotTable
            ");

            queryDt = @"SELECT Pro.id as numProceso, Convert(varchar(16), Pro.Fecha, 121) as Fecha, Machines.MachineAlias, Machines.[IP], [Intento de Conexión],
	            IIF(NOT temp.[Conexión] IS NULL, 'OK', '-') as [Conexión], 
	            IIF(NOT temp.[Lectura Marcaciones] IS NULL, 'OK', '-') as [Lectura Marcaciones], 
	            IIF(NOT temp.[Intento Desconexión] IS NULL, 'OK', '-') as [Intento Desconexión], 
	            IIF(NOT temp.[Desconexión] IS NULL, 'OK', '-') as [Desconexión], 
	            -- temp.[6], 
	            IIF(NOT temp.[Grabación Marcaciones Nuevas] IS NULL, 'OK', '-') as [Grabación Marcaciones Nuevas], 
	            -- temp.[8], 
	            temp.[Error] 
            FROM (#tempDescargas temp inner join da_ProcesosDescarga Pro on temp.idProceso = Pro.id)
		            INNER JOIN Machines ON temp.sn = Machines.sn
            ORDER BY idProceso, [Intento de Conexión]";
            return ClsAccesoDatos.RetornaDataTable(queryDt, lstConsultas);

        }

        public void IniciaGrabacionRostros()
        {
            string consulta = "TRUNCATE TABLE tmp_ROSTROS;";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void guardaRostros(List<DataGridViewRow> dgv)
        {
            string query = string.Empty;
            string consulta = string.Empty;

            // 0     BadgeNumber
            // 1     Enable
            // 2     Nombre
            // 3     Password
            // 4     Privilegio
            // 5     iLength
            // 6     FaceTemplate

            // Para ingresar las huellas del dgv a la tabla de huellas temporal
            foreach (DataGridViewRow fila in dgv)
                query = query + "INSERT INTO [tmp_ROSTROS] (Badge, sName, sPassword, iPrivilege, bEnabled, iLength, Template, Template1) " +
                    "VALUES('" + fila.Cells[0].Value + "', '" + fila.Cells[2].Value + "', '" + fila.Cells[3].Value + "', " + fila.Cells[4].Value + ", '" + fila.Cells[1].Value + "', " + fila.Cells[5].Value + ", '" + fila.Cells[6].Value + "', '" + fila.Cells[6].Value + "');" + "\n";

            ClsAccesoDatos.EjecutaNoQuery(query);
        }

        public void FinalizaGrabacionRostros()
        {

            string consulta;

            // Toma el primer depto disponible, (que en general va a ser el departamento raiz)
            // Selecciona los usuarios distintos de la tabla Huellas
            // e ingresa los usuarios nuevos a la tabla userinfo, con lo que se garantiza
            // que los usuarios nuevos tendrán su UserId
            // el dato bEnabled va a la columna EMPRIVILEGE
            consulta = @"DECLARE @depto integer;
            SELECT @depto = min(deptId) from DEPARTMENTS where DEPTID > 0;

            INSERT INTO USERINFO (BADGENUMBER, [NAME], DEFAULTDEPTID)
            select R.BADGE, isnull(nullif(R.sName, ''), R.BADGE), @depto
            FROM tmp_ROSTROS R left join Userinfo U ON R.badge = U.badgenumber
            where u.userid is null";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Elimina las huellas de la tabla Template, de los usuarios que tienen "nuevas" huellas ( no son necesariamente nuevas, pero son las que se descargaron recién)
            // porque serán reemplazados por las huellas recién descargadas
            consulta = @"DELETE FROM FaceTemp
            WHERE USERID IN ( SELECT distinct U.USERID
	        FROM tmp_ROSTROS R inner join USERINFO U ON R.BADGE = U.BADGENUMBER );";
            ClsAccesoDatos.EjecutaNoQuery(consulta);

            // Inserta las huellas en la tabla TEMPLATE
            // Las Tablas TEMPLATE y HUELLAS tienen la estructura adecuada
            // están en la base "BaseSinDatos"
            consulta = @"INSERT INTO FaceTemp (USERID, USERNO, SIZE, TEMPLATE, TEMPLATE1)
            SELECT U.USERID, R.BADGE, R.iLength, R.TEMPLATE, R.TEMPLATE1
	        FROM tmp_ROSTROS R INNER JOIN USERINFO U ON R.BADGE = u.BADGENUMBER;";
            ClsAccesoDatos.iEjecutaNoQuery(consulta, 60);

            ClsAccesoDatos.EjecutaNoQuery("TRUNCATE TABLE tmp_ROSTROS;");
        }

    }
}


