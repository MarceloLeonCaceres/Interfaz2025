using ConexionDatos;
using System.Data;

namespace DatosB
{
    public static class clsDatosAdminCorreos
    {
        public static DataTable dtRetornaCorreos()
        {
            return ConexionDatos.ClsAccesoDatos.RetornaDataTable("Select id, Correo from da_Correos");
        }

        public static void AgregaCorreoNuevo(string sCorreo)
        {
            ClsAccesoDatos.EjecutaNoQuery("INSERT INTO da_Correos (Correo) VALUES ('" + sCorreo + "')");
        }

        public static void EliminaCorreo(int idCorreo)
        {
            ClsAccesoDatos.EjecutaNoQuery("DELETE FROM da_Correos WHERE id = " + idCorreo);
        }

        public static void EditaCorreo(int idCorreo, string sCorreo)
        {
            ClsAccesoDatos.EjecutaNoQuery("UPDATE da_Correos SET correo = '" + sCorreo + "' WHERE id = " + idCorreo);
        }

    }
}
