using DatosB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicaB
{
    public static class LogicaTarjetas
    {

        public static async Task<List<string>> GetTarjetasValidas()
        {
            var listaEncriptada = await DatosTarjetas.GetTarjetasValidas();

            if (listaEncriptada == null || listaEncriptada.Count == 0)
            {
                return new List<string>();
            }

            var AuxCripto = new Utilitarios.ClsEncriptacion();
            var listaDesencriptada = AuxCripto.DesencriptarListaBIO(listaEncriptada);

            return listaDesencriptada;
        }
    }
}
