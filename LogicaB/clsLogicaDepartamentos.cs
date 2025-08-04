using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DatosB;
using System.Windows.Forms;
using Utilitarios;

namespace LogicaB
{
    public class clsLogicaDepartamentos
    {

        public string ListaDepartamentos = "";

        private DataTable RetornaDtDepartamentosArbol(int i, string codAdmin)
        {
            
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            try
            {
                return objDatos.RetornaDtDepartamentosArbol(i, codAdmin);
            }
            catch (clsDataBaseException ErrorBDD)
            {
                throw new clsLogicaException("Error BDD. " + "\n" + ErrorBDD.DataErrorDescription);
            }
            catch (clsLogicaException ErrorLogica)
            {
                throw new clsLogicaException("Error Logica. " + "\n" + ErrorLogica.logErrorDescription);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LlenaDepts(string codAdminLog, TreeView TreeViewDeptos, int TipoAdministrador)
        {
            try
            {
                TreeViewDeptos.Nodes.Clear();
                DataTable daTableArbol = new DataTable();
                DataTable dtAux = new DataTable();
                // Dim objLogica As New Logica.clsLogicaDepartamentos
                // daTableArbol = objLogica.RetornaDtDepartamentosArbol(0, String.Empty)
                daTableArbol = RetornaDtDepartamentosArbol(0, string.Empty);
                
                if (TipoAdministrador <= 0 || TipoAdministrador > 3)
                    TipoAdministrador = 3;

                switch (TipoAdministrador)
                {
                    case 3:
                        {
                            // Llena el treeView con todos los departamentos y subdepartamentos
                            LlenaTreeView(0, null/* TODO Change to default(_) if this is not a reference type */, daTableArbol, TreeViewDeptos);
                            break;
                        }

                    case 2:
                        {
                            // dtAux = New DataTable
                            // dtAux = objLogica.RetornaDtDepartamentosArbol(1, codAdminLog)
                            dtAux = RetornaDtDepartamentosArbol(1, codAdminLog);

                            TreeNode nuevoNodo = new TreeNode();
                            nuevoNodo.Text = (string)dtAux.Rows[0][4];
                            nuevoNodo.Tag = dtAux.Rows[0][2];
                            TreeViewDeptos.Nodes.Add(nuevoNodo);
                            // CrearNodosDelPadre(dtAux.Rows(0)(2), nuevoNodo)
                            // objLogica.LlenaTreeView(dtAux.Rows(0)(2), nuevoNodo, daTableArbol, TreeViewDeptos)
                            LlenaTreeView((int)dtAux.Rows[0][2], nuevoNodo, daTableArbol, TreeViewDeptos);
                            break;
                        }

                    case 1:
                        {
                            // dtAux = New DataTable
                            // dtAux = objLogica.RetornaDtDepartamentosArbol(2, codAdminLog)
                            dtAux = RetornaDtDepartamentosArbol(2, codAdminLog);

                            TreeNode nuevoNodo = new TreeNode();
                            nuevoNodo.Text = (string)dtAux.Rows[0][4];
                            nuevoNodo.Tag = dtAux.Rows[0][2];
                            TreeViewDeptos.Nodes.Add(nuevoNodo);
                            break;
                        }
                }

                TreeViewDeptos.ExpandAll();
            }
            catch (clsDataBaseException ErrorBDD)
            {
                throw new clsLogicaException("Error BDD. " + "\n" + ErrorBDD.DataErrorDescription);
            }
            catch (clsLogicaException ErrorLogica)
            {
                throw new clsLogicaException("Error Logica. " + "\n" + ErrorLogica.logErrorDescription);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LlenaTreeView(int indicePadre, TreeNode nodePadre, DataTable dataTableArbol, TreeView tvdepto)
        {
            DataView dataViewHijos;
            // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
            dataViewHijos = new DataView(dataTableArbol);
            dataViewHijos.RowFilter = dataTableArbol.Columns["CodPadre"].ColumnName + " = " + indicePadre.ToString();

            // Agregar al TreeView los nodos Hijos que se han obtenido en el DataView.
            foreach (DataRowView dataRowCurrent in dataViewHijos)
            {
                TreeNode nuevoNodo = new TreeNode();
                nuevoNodo.Text = dataRowCurrent["NomDepto"].ToString().Trim();
                nuevoNodo.Tag = dataRowCurrent["codnodo"].ToString().Trim();
                // si el parámetro nodoPadre es nulo es porque es la primera llamada, son los Nodos
                // del primer nivel que no dependen de otro nodo.
                if (nodePadre == null)
                    tvdepto.Nodes.Add(nuevoNodo);
                else
                    // se añade el nuevo nodo al nodo padre.
                    nodePadre.Nodes.Add(nuevoNodo);

                // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
                LlenaTreeView(Int32.Parse(dataRowCurrent["CodNodo"].ToString()), nuevoNodo, dataTableArbol, tvdepto);
            }
        }

        //public void RefrescaListaEmpleados(TreeNode nodo, bool incSubDeptos, int tipoAdmin, CheckedListBox checkListEmpleados, string busqueda)
        //{
        //    LimpiaDeptos();
        //    ActualizaListaDeptos(nodo);

        //    DataTable dtEmpleados = new DataTable();

        //    DatosB.clsDatosEmpleados objDatos = new DatosB.clsDatosEmpleados();
        //    clsUtilidades objUtil = new clsUtilidades();
        //    string condiciones;
        //    int intDepto = (int)nodo.Tag;

        //    if (tipoAdmin == -1)
        //        // Solo 1 empleado
        //        condiciones = "Userinfo.badgenumber=" + objUtil.codAdminLog;
        //    else if (incSubDeptos == false)
        //        // Los empleados de solo 1 departamento
        //        condiciones = "Userinfo.DefaultDeptId='" + intDepto.ToString() + "'";
        //    else if (ListaDepartamentos == null/* TODO Change to default(_) if this is not a reference type */ )
        //        // Devuelve todos los empleados excepto los despedidos temporalmente
        //        condiciones = " Userinfo.DefaultDeptId<>-1";
        //    else
        //        // Los empleados del departamento y de los subdepartamentos
        //        condiciones = RegresaListaDeptos();
        //    if (!string.IsNullOrWhiteSpace(busqueda))
        //        condiciones = "(" + condiciones + ") AND Userinfo.NAME like '%" + busqueda + "%'";

        //    dtEmpleados = RetornaDtEmpleados((int)nodo.Tag, incSubDeptos, 0, busqueda);

        //    // Borra lstEmpleado anterior
        //    checkListEmpleados.Items.Clear();
        //    checkListEmpleados.Refresh();

        //    for (int i = 0; i <= dtEmpleados.Rows.Count - 1; i++)
        //        checkListEmpleados.Items.Add(new clsAuxiliarLogica.ListaEmp(dtEmpleados(i)("Nombre"), dtEmpleados[i]["NumCA"], dtEmpleados(i)("UserId")));
        //}

        private void ActualizaListaDeptos(TreeNode NodoSeleccionado)
        {
            ListaDepartamentos = ListaDepartamentos + " or Userinfo.DefaultDeptId = " + NodoSeleccionado.Tag;
            foreach (TreeNode nodo in NodoSeleccionado.Nodes)
                ActualizaListaDeptos(nodo);
        }

        private void LimpiaDeptos()
        {
            ListaDepartamentos = string.Empty;
        }

        public string RegresaListaDeptos()
        {
            return ListaDepartamentos.Substring(3, ListaDepartamentos.Length - 3);
        }

        //private DataTable RetornaDtEmpleados(int intDepto, bool incSubDeptos, int tipoAdmin, string busqueda)
        //{
        //    DatosB.clsDatosEmpleados objDatos = new DatosB.clsDatosEmpleados();
        //    Utilidades.clsUtilidades objUtil = new Utilidades.clsUtilidades();
        //    string condiciones;

        //    if (tipoAdmin == -1)
        //        // Solo 1 empleado
        //        condiciones = "Userinfo.badgenumber=" + objUtil.codAdminLog;
        //    else if (incSubDeptos == false)
        //        // Los empleados de solo 1 departamento
        //        condiciones = "Userinfo.DefaultDeptId='" + intDepto.ToString() + "'";
        //    else if (ListaDepartamentos == null/* TODO Change to default(_) if this is not a reference type */ )
        //        // Devuelve todos los empleados excepto los despedidos temporalmente
        //        condiciones = " Userinfo.DefaultDeptId<>-1";
        //    else
        //        // Los empleados del departamento y de los subdepartamentos
        //        condiciones = RegresaListaDeptos();

        //    if (!string.IsNullOrWhiteSpace(busqueda))
        //        condiciones = "(" + condiciones + ") AND Userinfo.NAME like '%" + busqueda + "%'";

        //    return objDatos.RetornaEmpleados(condiciones).Tables(0);
        //}

        public void IngresaDepartamento(string nomDepto, int codPadre, string codAdminLog = "-1")
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            objDatos.IngresaDepartamento(nomDepto, codPadre, codAdminLog);
        }

        public int cuentaDeptosHijos(int codDepto)
        {
            DatosB.clsDatosDepartamentos oDatos = new DatosB.clsDatosDepartamentos();
            return oDatos.cuentaDeptosHijos(codDepto);
        }

        public void EliminaDepto(int codDepto, string codAdminLog = "-1")
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            objDatos.EliminaDepto(codDepto, codAdminLog);
        }

        public void ActualizaDepartamento(int codDepto, string nomDepto, string codAdminLog = "-1")
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            objDatos.ActualizaDepartamento(codDepto, nomDepto, codAdminLog);
        }

        public void TransfiereDepartamento(int codDepto, List<int> listEmps, string codAdminLog = "-1")
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            objDatos.TransfiereDepartamento(codDepto, listEmps, codAdminLog);
        }

        public DataTable RetornaLogo()
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            return objDatos.RetornaLogo();
        }

        public void updateLogoEmpresa(DataTable dtFoto, string tabla)
        {
            DatosB.clsDatosDepartamentos oDatos = new DatosB.clsDatosDepartamentos();
            oDatos.updateLogoEmpresa(dtFoto, tabla);
        }

        public void EliminaLogo()
        {
            DatosB.clsDatosDepartamentos objDatos = new DatosB.clsDatosDepartamentos();
            objDatos.EliminaLogo();
        }
    }

}
