using System;

namespace Domain
{
    public class MarcacionParser
    {
        private static readonly char[] Delimiters = { ' ', ',', '.', '\t' };

        public bool TryParse(string line, out RegistroBiometrico record)
        {
            record = null;

            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = line.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 7)
                return false;

            if (parts[0] == "0")
                return false;

            if (!DateTime.TryParse($"{parts[1]} {parts[2]}", out var fechaHora))
                return false;

            DateTime fechaHoraReducida = fechaHora.Date + new TimeSpan(fechaHora.Hour, fechaHora.Minute, 0);

            if (!int.TryParse(parts[3], out var tipo))
                return false;

            record = new RegistroBiometrico(
                parts[0],
                fechaHoraReducida,
                tipo,
                parts[4],
                parts[5],
                parts[6]);

            return true;
        }
    }
}
