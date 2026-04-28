using ConexionDatos;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Utilitarios;

namespace DatosB
{
    public static class clsDatosRegistrosUSB
    {
        private static SqlCommand comando;

        public static string retornaUserId(string badgenumber)
        {
            string consulta;
            consulta = "SELECT userid FROM userinfo " + "WHERE badgenumber='" + badgenumber + "'";
            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static void InsertaLoteRegsUSB(string listado)
        {
            string consulta;
            SqlConnection cnn = new SqlConnection(ClsAccesoDatos.strConexion);
            comando = cnn.CreateCommand();
            cnn.Open();

            // Crea una tabla con marcaciones del archivo csv
            consulta = "BEGIN TRY " + "\n" + "TRUNCATE TABLE #RegistrosHP; " + "\n" + "END TRY " + "\n" + "BEGIN CATCH " + "\n" + "CREATE TABLE #RegistrosHP " + "\n" + "(BADGENUMBER varchar(24),  " + "\n" + "UserId int, " + "\n" + "[CHECKTYPE] varchar(1) DEFAULT 'I', " + "\n" + "[VERIFYCODE] int DEFAULT 4, " + "\n" + "[SENSORID] varchar(5) DEFAULT 3, " + "\n" + "[WorkCode] int DEFAULT 0, " + "\n" + "[sn] varchar(20) DEFAULT '1234567890123', " + "\n" + "CheckTime datetime); " + "\n" + "END CATCH; " + "\n";
            comando.CommandText = consulta + listado + "\n";
            comando.ExecuteNonQuery();

            // Elimina marcaciones duplicadas de la tabla creada con el archivo csv original
            consulta = "SELECT DISTINCT BADGENUMBER, checktime, count(*) AS CONTEO " + "\n" + "INTO #Duplicados " + "\n" + "FROM #RegistrosHP  " + "\n" + "GROUP BY BADGENUMBER, checktime " + "\n" + "HAVING COUNT(*) > 1;" + "\n" + "DELETE #RegistrosHP " + "\n" + "FROM #RegistrosHP JOIN #Duplicados ON #RegistrosHP.BADGENUMBER = #Duplicados.BADGENUMBER AND " + "\n" + "#RegistrosHP.CheckTime = #Duplicados.CheckTime;";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            consulta = "INSERT INTO #RegistrosHP (BadgeNumber, CheckTime, CheckType, VerifyCode, SensorId, WorkCode, sn) " + "\n" + "SELECT BadgeNumber, CheckTime, 'I', 4, 3, 0, '1234567890123' " + "\n" + "FROM #Duplicados;" + "\n" + "\n" + "DROP TABLE #Duplicados;";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            // Pone UserId con info de BadgeNumber
            consulta = "UPDATE #RegistrosHP SET USERID = USERINFO.USERID FROM USERINFO INNER JOIN #RegistrosHP ON USERINFO.BADGENUMBER = #RegistrosHP.BADGENUMBER;";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            // Crea ConsultasDapper Nuevos
            consulta = "DECLARE @Depto int = 1;" + "\n" + "SELECT @Depto = MIN(DEPTID) FROM DEPARTMENTS WHERE DEPTID > 0;" + "\n" + "INSERT INTO USERINFO (BADGENUMBER, NAME, DEFAULTDEPTID) " + "\n" + "SELECT DISTINCT BADGENUMBER, BADGENUMBER, @Depto " + "\n" + "FROM #RegistrosHP " + "\n" + "WHERE UserId IS NULL;";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            // Pone UserId con info de BadgeNumber
            consulta = "UPDATE #RegistrosHP SET USERID = USERINFO.USERID FROM USERINFO INNER JOIN #RegistrosHP ON USERINFO.BADGENUMBER = #RegistrosHP.BADGENUMBER;";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            // Fusiona (Merge) la tabla nueva de marcaciones con la tabla CHECKINOUT
            consulta = "SET NOCOUNT ON; " + "\n" + "MERGE CHECKINOUT AS Target " + "\n" + "USING #RegistrosHP as Source on (Target.UserId = Source.UserId AND Target.CheckTime = Source.CheckTime) " + "\n" + "WHEN MATCHED THEN " + "\n" + "UPDATE SET CheckType = 'I', [VERIFYCODE] = 4 " + "\n" + "WHEN NOT MATCHED THEN " + "\n" + "INSERT (UserId, CheckTime, CheckType, VerifyCode, SensorId, WorkCode, sn) " + "VALUES (Source.UserId, Source.CheckTime, Source.CheckType, Source.VerifyCode, Source.SensorId, Source.WorkCode, Source.sn);";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            consulta = "INSERT INTO SystemLog VALUES('-2', GETDATE(), 'Interfaz', 0, 'Inserta registros USB', '');";
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            cnn.Close();
        }

        public static void EnceraRegistrosUSB(string sn)
        {
            string consulta = $"Delete from tmp_Marcaciones where sn = '{sn}'";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }
        //public static void GuardaRegistrosUsbEnTemporal(List<Tuple<string, string, string, int, string, string, string>> listaTuplas, string sn)
        //{
        //    StringBuilder queryHuellas = new StringBuilder("");
        //    string consulta = string.Empty;

        //    queryHuellas.Append("INSERT INTO [tmp_MARCACIONES] \n");
        //    queryHuellas.Append("( [User ID], [Verify Date], SENSORID, [Verify State], [Verify Type], workCode, sn ) \n");
        //    queryHuellas.Append("VALUES \n");
        //    List<string> lstValues = new List<string>();
        //    foreach (var registro in listaTuplas)
        //    {
        //        lstValues.Add("('" + registro.Item1 + "', '" + registro.Item2 + " " + registro.Item3 + "', '0', '" + registro.Item5 + "', '" + registro.Item6 + "', '" + registro.Item7 + "', '" + sn + "')");
        //    }
        //    queryHuellas.Append(string.Join(",\n", lstValues));
        //    try
        //    {
        //        ClsAccesoDatos.EjecutaNoQuery("set dateformat ymd;\n" + queryHuellas.ToString());
        //    }
        //    catch (clsDataBaseException error)
        //    {
        //        throw new Utilitarios.clsDataBaseException(error.DataErrorDescription);
        //    }
        //    catch (Exception error)
        //    {
        //        throw error;
        //    }
        //}
        public static void GuardaRegistrosUsbEnTemporal(IEnumerable<RegistroBiometrico> registros, string sn)
        {
            try
            {
                using (var connection = new SqlConnection(ClsAccesoDatos.strConexion))
                {
                    connection.Open();

                    using (var bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = "tmp_MARCACIONES";
                        bulkCopy.BatchSize = 1000;
                        bulkCopy.BulkCopyTimeout = 60;

                        var table = CreateMarcacionesDataTable(registros, sn);
                        foreach (DataColumn col in table.Columns)
                        {
                            Debug.WriteLine(col.ColumnName);
                        }

                        bulkCopy.WriteToServer(table);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new clsLogicaException(ex.Message);
            }
        }
        private static DataTable CreateMarcacionesDataTable(IEnumerable<RegistroBiometrico> registros, string sn)
        {
            var table = new DataTable();

            table.Columns.Add("User ID", typeof(string));
            table.Columns.Add("Verify Date", typeof(DateTime));
            table.Columns.Add("Verify Type", typeof(int));
            table.Columns.Add("Verify State", typeof(int));
            table.Columns.Add("WorkCode", typeof(int));
            table.Columns.Add("sn", typeof(string));
            table.Columns.Add("SENSORID", typeof(string));

            foreach (var r in registros)
            {
                if (!int.TryParse(r.EntSal, out int entSal))
                    throw new FormatException($"El valor de EntSal '{r.EntSal}' no es un número válido para Ent/Sal {r.EntSal}.");

                if (!int.TryParse(r.WorkCode, out int workCode))
                    throw new FormatException($"El valor de WorkCode '{r.WorkCode}' no es un número válido para TrabajoCodigo {r.WorkCode}.");

                table.Rows.Add(
                    r.Badge,
                    r.FechaHora,
                    r.Tipo,
                    entSal,
                    workCode,
                    sn,
                    "0");
            }

            return table;
        }
        public static int IngresaNuevasMarcaciones(int maximoRelojes)
        {
            int n = -1;
            List<string> lstPasos = new List<string>();
            string consulta;
            try
            {
                // Borra marcaciones ya en CheckInOut
                consulta = @"delete Tmp
                from (CHECKINOUT C inner join USERINFO U ON C.USERID = U.USERID)
                inner join tmp_Marcaciones Tmp On Tmp.[User ID] = U.Badgenumber AND tmp.[Verify Date] = c.CHECKTIME;";
                ClsAccesoDatos.EjecutaNoQuery(consulta);

                consulta = @";WITH RangeCTE AS (    
                    SELECT MIN([Verify Date]) AS Desde, MAX([Verify Date]) AS Hasta    
                    FROM tmp_Marcaciones
                )
                SELECT 
                COUNT(*) AS TotalCheckins, MAX(SENSORCODE) AS NumeroYaIngresados
                FROM CHECKINOUT, RangeCTE
                WHERE CHECKTIME >= RangeCTE.Desde AND CHECKTIME <= RangeCTE.Hasta AND sn = '1234567890123';";
                var data = ClsAccesoDatos.RetornaDataTable(consulta);
                if (data.Rows.Count <= 0)
                    return -1;

                int enBddEnMismasFechas = (int)data.Rows[0][0];
                if (enBddEnMismasFechas < 0)
                    return -1;

                int numeroYaIngresados = 0;

                if (enBddEnMismasFechas > 0 && data.Rows[0][1] is int yaIngresados)
                {
                    numeroYaIngresados = yaIngresados;
                }

                if (numeroYaIngresados >= maximoRelojes)
                    return -2;

                // Ingresa Nuevos Usuarios
                consulta = @"DECLARE @Depto int = 1;
                SELECT @Depto = DEPTID FROM DEPARTMENTS WHERE SUPDEPTID = 0;
                WITH Nuevos AS(
                select distinct[User ID] from tmp_Marcaciones
                where [User ID] not in (select badgenumber from USERINFO )
                )
                INSERT INTO USERINFO (Badgenumber, [Name], DEFAULTDEPTID) SELECT [User ID], [User ID], @Depto FROM Nuevos";
                lstPasos.Add(consulta);

                // Crea la tabla Temporal de Equivalencias CheckTypes
                consulta = @"CREATE TABLE [dbo].[#CheckTypes](
	                [int] [int] NOT NULL,
	                [checkType] [varchar](1) NOT NULL
                ) ON [PRIMARY];
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (0, N'I');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (1, N'O');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (2, N'0');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (3, N'1');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (4, N'i');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (5, N'o');
                INSERT [dbo].[#CheckTypes] ([int], [checkType]) VALUES (255, N'I');";
                lstPasos.Add(consulta);

                // Envía marcaciones nuevas a CheckInOut
                consulta = $@"INSERT INTO [dbo].[CHECKINOUT]
                ([USERID], [CHECKTIME], [CHECKTYPE], [VERIFYCODE], [SENSORID], 
                    [WorkCode], [sn], Memoinfo, SENSORCODE)
                SELECT U.USERID, Tmp.[Verify Date], CT.[checkType], Tmp.[Verify Type], Tmp.SENSORID, 
                    Tmp.WorkCode, Tmp.sn, '', {(numeroYaIngresados + 1).ToString()}
                FROM (( tmp_Marcaciones Tmp inner join USERINFO U ON Tmp.[User ID] = U.Badgenumber) 
			    inner join [#CheckTypes] CT ON Tmp.[Verify State] = CT.[int])";

                n = ClsAccesoDatos.IntEjecutaEscalar(consulta, lstPasos);
                return n;
            }
            catch (clsDataBaseException error)
            {

                throw;
                //throw new Utilitarios.clsDataBaseException(error.DataErrorDescription +
                //    "\r\n" +
                //    error.InnerException?.Message?.ToString());
            }
            catch (Exception error)
            {
                throw;
            }
        }

    }
}