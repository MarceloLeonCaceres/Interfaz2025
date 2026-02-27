using System;

namespace Domain
{
    public class RegistroBiometrico
    {
        public string Badge { get; }
        public DateTime FechaHora { get; }
        public int Tipo { get; }
        public string EntSal { get; }
        public string HueRos { get; }
        public string WorkCode { get; }

        public RegistroBiometrico(
            string badge,
            DateTime fechaHora,
            int tipo,
            string entSal,
            string hueRos,
            string workCode)
        {
            Badge = badge;
            FechaHora = fechaHora;
            Tipo = tipo;
            EntSal = entSal;
            HueRos = hueRos;
            WorkCode = workCode;
        }
    }
}
