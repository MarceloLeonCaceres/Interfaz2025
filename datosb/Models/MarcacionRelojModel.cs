using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosB.Models
{
    public class MarcacionRelojModel
    {
        public string UserID { get; set; }
        public DateTime VerifyDate { get; set; }
        public int VerifyType { get; set; }
        public int VerifyState { get; set; }
        public int WorkCode { get; set; }
        public string Sn { get; set; } = string.Empty;
    }
}
