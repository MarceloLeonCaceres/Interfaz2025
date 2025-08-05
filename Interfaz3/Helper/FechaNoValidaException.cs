using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDispositivosBiometricos.Helper
{
    [Serializable]
    public class FechaNoValidaException : Exception
    {
        public string DatosLeidos { get; }
        public FechaNoValidaException()
        {
        }
        public FechaNoValidaException(string message) 
        {
            DatosLeidos = message;
        }

        public FechaNoValidaException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}
