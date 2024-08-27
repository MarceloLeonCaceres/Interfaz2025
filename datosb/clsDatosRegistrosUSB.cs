using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ConexionDatos;
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

        public static void EnceraRegistrosUSB()
        {
            string consulta = "Delete from tmp_Marcaciones where sn ='1234567890123'";
            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }
        public static void GuardaRegistrosUsbEnTemporal(List<Tuple<string, string, string, int, string, string, string>> listaTuplas, string sn)
        {
            StringBuilder queryHuellas = new StringBuilder("");
            string consulta = string.Empty;

            queryHuellas.Append("INSERT INTO [tmp_MARCACIONES] \n");
            queryHuellas.Append("( [User ID], [Verify Date], SENSORID, [Verify State], [Verify Type], workCode, sn ) \n");
            queryHuellas.Append("VALUES \n");
            List<string> lstValues = new List<string>();
            foreach (var registro in listaTuplas)
            {
                lstValues.Add("('" + registro.Item1 + "', '" + registro.Item2 + " " + registro.Item3 + "', '0', '" + registro.Item5 + "', '" + registro.Item6 + "', '" + registro.Item7 + "', '" + sn + "')");                                
            }
            queryHuellas.Append(string.Join(",\n", lstValues));
            try
            {
                ClsAccesoDatos.EjecutaNoQuery("set dateformat ymd;\n" + queryHuellas.ToString());
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

        public static int IngresaNuevasMarcaciones()
        {
            int n = -1;
            List<string> lstPasos = new List<string>();
            string consulta;
            try
            {
                // Elimina duplicados en tmp_Marcaciones
                // Puede haber duplicados por las marcaciones USB, que se carga solo con HH:mm
                // 1 de 3. Se obtienen los duplicados
                consulta = @"SELECT DISTINCT [User ID], [Verify Date], [Verify Type], [Verify State], [WorkCode], sn, SENSORID 
		            INTO #duplicate_table
                  FROM tmp_Marcaciones
	              GROUP BY [User ID], [Verify Date], [Verify Type], [Verify State], [WorkCode], sn, SENSORID 
                  HAVING COUNT(*) > 1";
                lstPasos.Add(consulta);

                // 2 de 3. Se eliminan los 2 duplicados en tmp_Marcaciones
                consulta = @"DELETE original
                  FROM tmp_Marcaciones original INNER JOIN #duplicate_table Dup
	              ON original.[User ID] = Dup.[User ID] AND original.[Verify Date] = Dup.[Verify Date] AND original.[sn] = Dup.[sn]";
                lstPasos.Add(consulta);

                // 3 de 3. Se ingresan los registros únicos en tmp_Marcaciones
                consulta = @"INSERT INTO tmp_Marcaciones([User ID], [Verify Date], [Verify Type], [Verify State], [WorkCode], sn, SENSORID)
                    SELECT [User ID], [Verify Date], [Verify Type], [Verify State], [WorkCode], sn, SENSORID 
                    FROM #duplicate_table";
                lstPasos.Add(consulta);


                // Ingresa Nuevos ConsultasDapper
                consulta = @"DECLARE @Depto int = 1;
                SELECT @Depto = DEPTID FROM DEPARTMENTS WHERE SUPDEPTID = 0;
                WITH Nuevos AS(
                select distinct[User ID] from tmp_Marcaciones
                where [User ID] not in (select badgenumber from USERINFO )
                )
                INSERT INTO USERINFO (Badgenumber, [Name], DEFAULTDEPTID) SELECT [User ID], [User ID], @Depto FROM Nuevos";
                lstPasos.Add(consulta);

                // Borra marcaciones ya en CheckInOut
                consulta = @"delete Tmp
                from (CHECKINOUT C inner join USERINFO U ON C.USERID = U.USERID)
                inner join tmp_Marcaciones Tmp On Tmp.[User ID] = U.Badgenumber AND tmp.[Verify Date] = c.CHECKTIME;";
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
                consulta = @"INSERT INTO [dbo].[CHECKINOUT]
                ([USERID], [CHECKTIME], [CHECKTYPE], [VERIFYCODE], [SENSORID], [WorkCode], [sn], Memoinfo)
                SELECT U.USERID, Tmp.[Verify Date], CT.[checkType], Tmp.[Verify Type], Tmp.SENSORID, Tmp.WorkCode, Tmp.sn, ''
                FROM (( tmp_Marcaciones Tmp inner join USERINFO U ON Tmp.[User ID] = U.Badgenumber) 
			    inner join [#CheckTypes] CT ON Tmp.[Verify State] = CT.[int])";

                n = ClsAccesoDatos.IntEjecutaEscalar(consulta, lstPasos);
                
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
        
    }
}