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
            // Write the string array to a new file named "WriteLines.txt".
            string Hoy = DateTime.Now.ToString("yyyy-MM-dd");
            string detalle1 = "";
            string detalle3 = "";

            // Create a string array with the lines of text
            string[] lines = { "\n", "*****************************************", DateTime.Now.ToString(), sMensaje, detalle1 };

            // Set a variable to the Documents path.
            string docPath = Application.StartupPath;

            using (StreamWriter outputFile = File.AppendText(Path.Combine(docPath, "Log_Error", "Log_Event_" + Hoy + ".txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }
    }
}