namespace Domain
{
    public class UsuarioBiometrico
    {
        public string Pin { get; set; }
        public string Name { get; set; }
        public string MVerifyPass { get; set; }
        public string CardNumber { get; set; }
        public int Privilege { get; set; }
        public int EnPrivilege { get; set; }
    }
}
