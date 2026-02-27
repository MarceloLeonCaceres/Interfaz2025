using System.Collections.Generic;

namespace Domain
{
    public class ImportResult
    {
        public IReadOnlyList<RegistroBiometrico> Valid { get; }
        public IReadOnlyList<string> InvalidLines { get; }

        public ImportResult(
            IReadOnlyList<RegistroBiometrico> valid,
            IReadOnlyList<string> invalid)
        {
            Valid = valid;
            InvalidLines = invalid;
        }
    }
}
