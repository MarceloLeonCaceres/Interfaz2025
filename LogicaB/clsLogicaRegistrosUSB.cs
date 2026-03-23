using DatosB;
using Domain;
using System;
using System.Collections.Generic;
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

        public static int IngresaMarcaciones(IEnumerable<RegistroBiometrico> lista, string sn)
        {
            int numeroMaximoRelojesUSB = 1;
            try
            {
                clsDatosRegistrosUSB.EnceraRegistrosUSB(sn);

                clsDatosRegistrosUSB.GuardaRegistrosUsbEnTemporal(lista, sn);

                int n = clsDatosRegistrosUSB.IngresaNuevasMarcaciones(numeroMaximoRelojesUSB);
                return n;
            }
            catch (clsDataBaseException errBdd)
            {
                //throw new clsLogicaException(errBdd.InnerException?.Message?.ToString() + "\r\n" +
                //    errBdd.DataErrorDescription?.ToString());
                throw;
            }
            catch (Exception err)
            {
                throw;
            }
        }
        public static int CheckType_a_int(string c)
        {
            switch (c)
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
