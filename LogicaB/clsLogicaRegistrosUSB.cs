using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosB;
using Utilitarios;

namespace LogicaB
{

    public static class clsLogicaRegistrosUSB
    {
        public static string retornaUserId(string badgenumber)
        {
            
            return clsDatosRegistrosUSB.retornaUserId(badgenumber);
        }

        public static void InsertaLoteRegsUSB(string listado)
        {
            
            clsDatosRegistrosUSB.InsertaLoteRegsUSB(listado);
        }

        public static void EnceraRegistrosUSB()
        {
            clsDatosRegistrosUSB.EnceraRegistrosUSB();
        }

        public static void GuardaRegistrosUsbEnTemporal(List<Tuple<string, string, string, int, string, string, string>> lista, string sn)
        {                        
            int paso = 900;
            int sizeLote = paso;
            int envios = lista.Count / sizeLote;
            int resto = lista.Count >= sizeLote ? lista.Count % sizeLote : lista.Count;
            Tuple<string, string, string, int, string, string, string>[] array;
            List<Tuple<string, string, string, int, string, string, string>> tempList;
            try
            {
                for(int i = 0; i <= envios; i++)
                {
                    if (i == envios)
                    {                        
                        if( resto == 0)
                        {
                            break;
                        }
                        sizeLote = resto;
                    }
                    array = new Tuple<string, string, string, int, string, string, string>[sizeLote];
                    lista.CopyTo(i * paso, array, 0, sizeLote);
                    tempList = new List<Tuple<string, string, string, int, string, string, string>>();
                    tempList = array.ToList();
                    clsDatosRegistrosUSB.GuardaRegistrosUsbEnTemporal(tempList, sn);
                }                                
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        

        public static int IngresaNuevasMarcaciones()
        {
            try
            { 
                return clsDatosRegistrosUSB.IngresaNuevasMarcaciones();
            }
            catch (clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public static int CheckType_a_int(string c)
        {
            switch(c)
            {
                case "I": return 0;
                case "O": return 1;
                case "0": return 2;
                case "1": return 3;
                case "i": return 4;
                case "o": return 5;
            }
            return 0;
        }

        public static string int_a_CheckType(int i)
        {
            switch (i)
            {
                case 0: return "I";
                case 1: return "O";
                case 2: return "0";
                case 3: return "1";
                case 4: return "i";
                case 5: return "o";
            }
            return "I";
        }

    }
}
