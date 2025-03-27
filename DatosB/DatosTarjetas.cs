using System.Collections.Generic;
using System;
using ConexionDatos;
using System.Threading.Tasks;

namespace DatosB
{
    public static class DatosTarjetas
    {
        public static async Task<List<string>> GetTarjetasValidas()
        {
            string sql = "Select Serie FROM Cards;";
            var accesoDatos = new SqlDataAccess();

            var listado = await accesoDatos.ReadDataAsync<string, dynamic>(sql, null);
            return listado;
        }
    }
}
