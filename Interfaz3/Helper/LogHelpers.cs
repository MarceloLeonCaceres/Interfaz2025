using System;
using System.IO;
using System.Windows.Forms;

namespace SDK
{
    internal static class LogHelpers
    {

        public static void DescribeError(string sMensaje, string sNombreArchivo = "Log_Error")
        {
            // Write the string array to a new file named "WriteLines.txt".
            string Hoy = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            string detalle1 = "";
            string detalle2 = "SN = " + sNombreArchivo;
            string detalle3 = "";

            // Create a string array with the lines of text
            string[] lines = { "\n\n", "*****************************************", DateTime.Now.ToString(), "\n", sMensaje, "\n", detalle1, detalle2, detalle3 };

            // Set a variable to the Documents path.
            string docPath = Application.StartupPath;

            using (StreamWriter outputFile = File.AppendText(Path.Combine(docPath, "Log_Error", "Log_Error_" + Hoy + ".txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }

        public static void ReportaNovedad(string sMensaje, string sNombreArchivo = "Log_Event")
        {
            string hoy = DateTime.Now.ToString("yyyy-MM-dd");

            string[] lines =
            {
                "",
                "*****************************************",
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"),
                sMensaje
            };

            string basePath = Application.StartupPath;
            string logDir = Path.Combine(basePath, "Log_Error");

            // ✅ Ensure directory exists
            Directory.CreateDirectory(logDir);

            string filePath = Path.Combine(logDir, $"Log_Event_{hoy}.txt");

            using (var writer = File.AppendText(filePath))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

        }

    }
}