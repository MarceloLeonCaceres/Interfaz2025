using System;
using System.Collections.Generic;
using System.Data;
using DatosB;
using Utilitarios;
using System.Net;

namespace LogicaB
{

    public class clsLogicaDispositivos
    {
        public DataTable dtRelojesValidos()
        {
            DatosB.clsDatosDispositivos oDatos = new DatosB.clsDatosDispositivos();
            List<string> LstSnValidos = new List<string>();
            LstSnValidos = ListaDesencriptadaDispositivos();
            if (LstSnValidos.Count > 0)
                return oDatos.dtRelojesValidos(LstSnValidos);
            else
            {
                return null;
            }
        }

        public List<string> ListaDesencriptadaDispositivos()
        {
            clsDatosDispositivos oDatos = new clsDatosDispositivos();
            ClsEncriptacion aux = new ClsEncriptacion();
            DataTable dtRelojesRegistrados = new DataTable();
            List<string> listaDispositivos = new List<string>();
            dtRelojesRegistrados = oDatos.dtDispositivos();

            foreach (DataRow filaReloj in dtRelojesRegistrados.Rows)
            {
                string desEncriptado = "";
                try
                {
                    desEncriptado = aux.DesencriptarBIO(filaReloj["Encriptado"].ToString());
                }
                catch (Exception)
                {
                    desEncriptado = aux.DesEncriptaFechaVigencia(filaReloj["Encriptado"].ToString());
                }
                finally
                {
                    listaDispositivos.Add("'" + desEncriptado + "'");
                }                
            }                

            return listaDispositivos;
        }
        public DataTable dtDispositivos()
        {
            DatosB.clsDatosDispositivos oDatos = new DatosB.clsDatosDispositivos();
            return oDatos.dtDispositivos();
        }

        public void InsertaDispositivos(string strSerial, int intUso)
        {
            ClsEncriptacion util = new ClsEncriptacion();
            DatosB.clsDatosDispositivos oDatos = new DatosB.clsDatosDispositivos();
            oDatos.InsertaDispositivos(util.EncriptarBIO(strSerial), intUso);
        }

        public void EliminaDispositivos(string strSerial)
        {
            ClsEncriptacion util = new ClsEncriptacion();
            DatosB.clsDatosDispositivos oDatos = new DatosB.clsDatosDispositivos();
            oDatos.EliminaDispositivos(util.EncriptarBIO(strSerial));
        }

        public void updateDispositivos(int intId, string strSerial, int intUso)
        {
            ClsEncriptacion util = new ClsEncriptacion();
            DatosB.clsDatosDispositivos oDatos = new DatosB.clsDatosDispositivos();
            oDatos.updateDispositivos(intId, util.EncriptarBIO(strSerial), intUso);
        }

        public DataTable dtRelojes()
        {
            try
            {
                clsDatosDispositivos oDatos = new clsDatosDispositivos();
                return oDatos.dtRelojes();
            }
            catch(clsDataBaseException errBdd)
            {
                throw new clsLogicaException(errBdd.DataErrorDescription);
            }
            catch (Exception err)
            {
                throw err;
            }
            
        }

        public List<ClsReloj> ListRelojes()
        {
            DataTable dtRelojesTemp = new DataTable();
            dtRelojesTemp = dtRelojes();
            List<ClsReloj> lstRelojes = new List<ClsReloj>();

            foreach (DataRow fila in dtRelojesTemp.Rows)
            {
                ClsReloj reloj = new ClsReloj();
                reloj.sNombreReloj = fila["Nombre"].ToString();
                reloj.sIP = IPAddress.Parse(fila["IP"].ToString());
                reloj.iPuerto = int.Parse(fila["Puerto"].ToString());
                reloj.iNumero = int.Parse(fila["NumeroDispositivo"].ToString());
                reloj.sSN = fila["NumeroSerie"].ToString();

                lstRelojes.Add(reloj);
            }
            return lstRelojes;
        }


        public void InsertarReloj(string sNombre, int iNumeroDispositivo, string sIP, int iPuerto, string sSN)
        {
            clsDatosDispositivos oDatos = new clsDatosDispositivos();
            oDatos.InsertarReloj(sNombre, iNumeroDispositivo, sIP, iPuerto, sSN);
        }

        public void ActualizarReloj(int id, string sNombre, int iNumeroDispositivo, IPAddress sIP, int iPuerto, string sSN)
        {
            clsDatosDispositivos oDatos = new clsDatosDispositivos();
            oDatos.ActualizarReloj(id, sNombre, iNumeroDispositivo, sIP, iPuerto, sSN);
        }

        public void EliminarReloj(int id)
        {
            clsDatosDispositivos oDatos = new clsDatosDispositivos();
            oDatos.EliminarReloj(id);
        }


    }



}
