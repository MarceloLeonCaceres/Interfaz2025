using LogicaB;
using System;
using System.Configuration;
using System.Management;
using System.Windows.Forms;

namespace AdminDispositivosBiometricos
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //// Inicio extraccion reloj para pruebas
            //System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("192.168.1.201");
            //ClsReloj datosRelojPrueba = new ClsReloj(1, "G3Blanco", 1, ipAddress, 4370, "AEH2185060039");
            //fControlReloj vReloj = new fControlReloj(datosRelojPrueba);
            //Application.Run(vReloj);
            //// Fin extraccion reloj para pruebas
            //bool hayModificacionesBdd = clsLogicaSeguridadInterfazRelojes.HayModificacionesEnBdd();

            //string msjPirateria = "Se han detectado violaciones a la integridad del sistema, y no puede seguir funcionando en estas condiciones.\n\n" + "Comuníquese con el proveedor para informar esta anomalía.";
            //if (hayModificacionesBdd)
            //{
            //    MessageBox.Show(msjPirateria, "Uso no autorizado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    clsLogicaSeguridadInterfazRelojes.RegistraModificacionesEnSistema();
            //    Application.ExitThread();
            //    Application.Exit();
            //    Environment.Exit(0);
            //}

            //bool hayModificacionesEnParametros = clsLogicaSeguridadInterfazRelojes.HayModificacionesEnParametros();
            //if (hayModificacionesEnParametros)
            //{
            //    MessageBox.Show(msjPirateria, "Uso no autorizado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    clsLogicaSeguridadInterfazRelojes.RegistraModificacionesEnSistema();
            //    Application.ExitThread();
            //    Application.Exit();
            //    Environment.Exit(0);
            //}

            //bool estaVigente =true;
            bool estaVigente = clsLogicaSeguridadInterfazRelojes.EstaVigente();
            if (!estaVigente)
            {
                string msjLicenciaCaducada = "La licencia de uso de este programa ha caducado.\n\n" + "Comuníquese con el proveedor para renovar la licencia de uso.";
                MessageBox.Show(msjLicenciaCaducada, "Licencia de uso expirada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
                Application.Exit();
                Environment.Exit(0);
            }

            string pcSecurity = ConfigurationManager.AppSettings["pcSecurity"];
            if (pcSecurity.ToLower() != "insecure")
            {
                bool es_Compu_Valida;
                try
                {
                    ManagementObject DiscoTerminal = new ManagementObject(@"Win32_PhysicalMedia='\\.\PHYSICALDRIVE0'");
                    string DiscoAUX = DiscoTerminal.Properties["SerialNumber"].Value.ToString().Trim();
                    es_Compu_Valida = clsLogicaSeguridadInterfazRelojes.Es_Terminal_Valida(DiscoAUX);
                }
                catch (Exception)
                {
                    es_Compu_Valida = true;
                }
                if (!es_Compu_Valida)
                {
                    string msjComputadoraNoRegistrada = "Este equipo no está registrado como un equipo válido para ejecutar el software ProperTime" + "\n\n" + "Comuníquese con el proveedor para adquirir una licencia adicional para esta computadora.";
                    MessageBox.Show(msjComputadoraNoRegistrada, "Computadora no registrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.ExitThread();
                    Application.Exit();
                    Environment.Exit(0);
                }
            }

            if (args.Length == 0)
            {
                Application.Run(new fPrincipal());
                // Application.Run(new fEmpleado());
            }
            else
            {
                fDescargaMasiva fDescarga = new fDescargaMasiva();
                clsLogicaDispositivos oLog = new clsLogicaDispositivos();
                fDescarga.listaRelojes = oLog.ListRelojes();
                fDescarga.modo = fDescargaMasiva.EnumModo.Automático;
                fDescarga.Show();
                fDescarga.DescargaMarcacionesRelojes(fDescarga.listaRelojes);

                Application.Exit();
                Environment.Exit(0);
            }

        }
    }
}
