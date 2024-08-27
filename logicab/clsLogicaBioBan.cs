using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosB;
using System.Windows.Forms;

namespace LogicaB
{
    public class clsLogicaBioBan
    {
        public void guardaHuellas(DataGridView dgv)
        {
            DatosB.clsDatosBioBan oDatos = new clsDatosBioBan();
            oDatos.guardaHuellas(dgv);

        }
    }
}
