using ConexionDatos;
using System.Data;

namespace DatosB
{
    public static class clsDatosSeguridadInterfazRelojes
    {
        public static string RetornaConteoTerminal(string discoEncriptado)
        {
            string consulta;
            consulta = "SELECT COUNT(*) from sca_Terminales WHERE sca_SN = '" + discoEncriptado + "'";

            return ClsAccesoDatos.EjecutaEscalar(consulta);
        }

        public static DataTable FechasVigencia()
        {
            string consulta = @"Select PARAVALUE as FechaVigencia,  convert(varchar(10), getdate(), 103) as FechaActual 
                from ProperParam where PARANAME = 'vfCoded'";
            return ClsAccesoDatos.RetornaDataTable(consulta);
        }

        public static DataSet PropiedadesTablas()
        {
            string getPropertiesCheckinout = "Select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Checkinout'";
            string getPropertiesMachines = "Select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Machines'";

            DataTable dtCheckinout = ClsAccesoDatos.RetornaDataTable(getPropertiesCheckinout);
            DataTable dtMachines = ClsAccesoDatos.RetornaDataTable(getPropertiesMachines);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtCheckinout);
            ds.Tables.Add(dtMachines);

            return ds;
        }

        public static (DataTable, string) GetParametrosDeSeguridad()
        {
            // Fecha -> numeroRelojes, Hora_Minuto -> True
            string consulta = @"Select PARAVALUE as RegistroBdd 
                from ProperParam where PARANAME = 'Formato_Fecha' or PARANAME = 'Formato_HoraMinuto'";
            DataTable dtParametros = ClsAccesoDatos.RetornaDataTable(consulta);

            consulta = "Select Count(*) From Machines";
            string conteoRelojes = ClsAccesoDatos.EjecutaEscalar(consulta);
            return (dtParametros, conteoRelojes);
        }

        public static void RegistraModificacionesEnSistema(string falseEncriptado)
        {
            //  Hora_Minuto <- False
            string script = $@"Delete from ProperParam where Paraname = 'Formato_HoraMinuto';
                INSERT INTO ProperParam(PARANAME, PARATYPE, PARAVALUE)
                VALUES('Formato_HoraMinuto', 'String', '{falseEncriptado}')";
            ClsAccesoDatos.EjecutaNoQuery(script);

        }
    }
}