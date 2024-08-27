using System;
using System.Collections.Generic;
using System.Data;
using ConexionDatos;
using Utilitarios;

namespace DatosB
{
    public class clsDatosDispositivos
    {
        public DataTable dtDispositivos()
        {
            string consulta;

            consulta = "Select id, Serial as Encriptado, Uso, '1234567890123' as  Serie FROM Dispositivos;";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public DataTable dtRelojesValidos(List<string> lstSNValidos)
        {
            string consulta;

            consulta = @"select ID, MachineAlias as Nombre, IP, Port as Puerto, MachineNumber as NumeroDispositivo, sn as NumeroSerie
            from Machines 
            where sn in (" + string.Join(",", lstSNValidos) + ");";

            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public void InsertaDispositivos(string strSerialEncriptado, int intUso)
        {
            string consulta;

            consulta = "INSERT INTO dispositivos " + "values ('" + strSerialEncriptado + "', " + intUso + ")";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void EliminaDispositivos(string strSerialEncriptado)
        {
            string consulta;

            consulta = "DELETE FROM dispositivos " + "WHERE serial='" + strSerialEncriptado + "'";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void updateDispositivos(int intId, string strSerialEncriptado, int intUso)
        {
            string consulta;

            consulta = "UPDATE dispositivos " + "\n" + "SET Serial = '" + strSerialEncriptado + "', Uso = " + intUso + "\n" + "WHERE id='" + intId + "'";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public DataTable dtRelojes()
        {
            string consulta;

            consulta = @"select ID, MachineAlias as Nombre, IP, Port as Puerto, MachineNumber as NumeroDispositivo, sn as NumeroSerie
            from Machines;";
            try
            {
                return ClsAccesoDatos.RetornaDataTable(consulta);
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

        public void InsertarReloj(object sNombre, object iNumeroDispositivo, object sIP, object iPuerto, object sSN)
        {
            string consulta;

            consulta = @"INSERT INTO Machines (ConnectType, MachineAlias, MachineNumber, [IP], [Port], sn)
            VALUES (1, '" + sNombre + "', " + iNumeroDispositivo + ", '" + sIP + "', " + iPuerto + " , '" + sSN + "');";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void ActualizarReloj(object iId, object sNombre, object iNumeroDispositivo, object sIP, object iPuerto, object sSN)
        {
            string consulta;

            consulta = @"UPDATE Machines SET 
            MachineAlias = '" + sNombre + @"', 
            MachineNumber = " + iNumeroDispositivo + @",
            [IP] = '" + sIP + @"',
            [Port] = " + iPuerto + @" , 
            ConnectType = 1,
            sn = '" + sSN + @" '
            WHERE ID = " + iId + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }

        public void EliminarReloj(int iId)
        {
            string consulta;

            consulta = @"DELETE FROM Machines 
            WHERE ID = " + iId + ";";

            ClsAccesoDatos.EjecutaNoQuery(consulta);
        }
    }

}
