using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Windows.Threading;
using System.Media;

namespace OrdVenta01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        private OrdVenta01.MIKO2016DataSet mIKO2016DataSet;
        private OrdVenta01.MIKO2016DataSet1 mIKO2016DataSet1;
        private OrdVenta01.MIKO2016DataSet2 mIKO2016DataSet2;
        private OrdVenta01.MIKO2016DataSetTableAdapters.nw_nventaTableAdapter mIKO2016DataSetnw_nventaTableAdapter;
        //private System.Windows.Data.CollectionViewSource nw_nventaViewSource;
        private OrdVenta01.MIKO2016DataSet1TableAdapters.ek_nventaTableAdapter mIKO2016DataSet1ek_nventaTableAdapter;
        private OrdVenta01.MIKO2016DataSet2TableAdapters.ek_TraeInfoClienteTableAdapter mIKO2016DataSet2ek_TraeInfoClienteTableAdapter;
        //private System.Windows.Data.CollectionViewSource nw_nventa1ViewSource;
       
        private ObservableCollection<OrdenVentaItem> ordenVentaItems = new ObservableCollection<OrdenVentaItem>();
       
        private string mensajeNvSinCanal;
        private bool esVentas;

        void AppStartup(object sender, StartupEventArgs args)
        {
            string v;
            DataRow[] currentRows; //< remover
            mIKO2016DataSet = ((OrdVenta01.MIKO2016DataSet)(this.FindResource("mIKO2016DataSet")));
            mIKO2016DataSet1 = ((OrdVenta01.MIKO2016DataSet1)(this.FindResource("mIKO2016DataSet1")));
            mIKO2016DataSet2 = ((OrdVenta01.MIKO2016DataSet2)(this.FindResource("mIKO2016DataSet2")));
            Console.WriteLine("AppStartup");

            v = OrdVenta01.Properties.Settings.Default.Ventas.ToString();
            if ((v.Equals("Y", StringComparison.InvariantCultureIgnoreCase)))
            {
                esVentas = true;
            }
            else
            {
                esVentas = false;
            }
          
            AseguraCanalOk();
           

            CargaEKfromDB();

            CargaInicialColeccionDesdeEw();

            MergeColeccionConEk();

            DispatcherTimerMethod();



            //-----------------------------------print datatable content
            #region
            currentRows = mIKO2016DataSet.nw_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            Console.WriteLine("Dataset ew luego de eliminar");
            if (currentRows.Length < 1)
                Console.WriteLine("No Current Rows Found");
            else
            {
                foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
                    Console.Write("\t{0}", column.ColumnName);

                //Console.WriteLine("\tRowState");

                //
                foreach (DataRow row in currentRows)
                {
                    foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
                        Console.Write("\t{0}", row[column]);

                    Console.WriteLine("\t" + row.RowState);
                }
            }
            #endregion
            //  ----------------------------------fin print datatable content
            //  LoadOrdenVentaData();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Console.WriteLine("After windowshow");

            //AddProductWindow addProductWindow = new AddProductWindow();
            //addProductWindow.Show();
        }

        public ObservableCollection<OrdenVentaItem> OrdenVentaItems
        {
            get { return this.ordenVentaItems; }
            set { this.ordenVentaItems = value; }
        }

        //private void LoadOrdenVentaData()
        //{

        //    //DataRow row1;

        //    mIKO2016DataSetnw_nventaTableAdapter = new OrdVenta01.MIKO2016DataSetTableAdapters.nw_nventaTableAdapter();
        //    mIKO2016DataSetnw_nventaTableAdapter.Fill(mIKO2016DataSet.nw_nventa);
        //    DataRow[] currentRows = mIKO2016DataSet.nw_nventa.Select(
        //    null, null, DataViewRowState.CurrentRows);
        //    if (currentRows.Length < 1)
        //        Console.WriteLine("No Current Rows Found");
        //    else
        //    {
        //        foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
        //            Console.Write("\t{0}", column.ColumnName);

        //        //Console.WriteLine("\tRowState");

        //        //
        //        foreach (DataRow row in currentRows)
        //        {
        //            foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
        //                Console.Write("\t{0}", row[column]);

        //            Console.WriteLine("\t" + row.RowState);
        //        }
        //        //for (int i = 0; i < currentRows.Length; i++)
        //        //{
        //        //    in
        //        //}
        //        //Console.WriteLine("\t" + currentRows[0]["NVNumero"]);
        //        //Console.WriteLine("\t" + currentRows[0]["NVFem"]);
        //        //Console.WriteLine("\t" + currentRows[0]["Estado"]);
        //        //Console.WriteLine("\t" + Convert.ChangeType(currentRows[0]["Estado"], typeof(Estado)));

        //        //Item item = new Item((int)currentRows[i]["NVNumero"], (DateTime)currentRows[i]["nvFem"], (String)currentRows[i]["Estado"]);
        //        //this.items.Add(item);
        //        //}


        //    }

        //    //OrdenVentaItem ord10 = new OrdenVentaItem(15213, new DateTime(2018, 2, 14, 8, 34, 45), EstadoOrden.Nueva, String.ClienteRetira, "Observacion z", "Ventas3");
        //    //this.OrdenVentaItems.Add(ord1);


        //}
        public void AseguraCanalOk()
        {
            // carga los registros de ew y verifica que todos tengan el campo nvCanalNV asignado llamando a CargaEWSoftlandFromDB
            // los que tengan NULL en la BD van a salir en un messagebox y espera la respuesta del usuario
            // el loop se va a repetir hasta que todos los registrostengan el campo asignado
            //
            Boolean dataOk = false;
            do
            {
                CargaEWSoftlandFromDb();
                if (CheckCanalVentaOk())
                {
                    dataOk = true;
                }
                else
                {
                    if (esVentas)
                    {
                        mensajeNvSinCanal = "Coloque el Canal De Venta a las siguientes Notas de Venta: " + mensajeNvSinCanal;
                        Console.WriteLine("Notas con peo{0}", mensajeNvSinCanal);
                        MessageBoxResult result = MessageBox.Show(mensajeNvSinCanal);
                    }
                    dataOk = false;
                }

            } while (!dataOk);

        }

        public void CargaEWSoftlandFromDb()
        {
            DataRow[] currentRows;
            //
            // Carga desde la DB los registros del dia actual de la tabla nw_nventa usando Fill
            //
            mIKO2016DataSetnw_nventaTableAdapter = new OrdVenta01.MIKO2016DataSetTableAdapters.nw_nventaTableAdapter();
            mIKO2016DataSetnw_nventaTableAdapter.Fill(mIKO2016DataSet.nw_nventa);
            mIKO2016DataSet.nw_nventa.AcceptChanges();
            //-----------------------------------print datatable content
            #region
            currentRows = mIKO2016DataSet.nw_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            Console.WriteLine("Dataset ew");
            if (currentRows.Length < 1)
                Console.WriteLine("No Current Rows Found");
            else
            {
                foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
                    Console.Write("\t{0}", column.ColumnName);

                //Console.WriteLine("\tRowState");

                //
                foreach (DataRow row in currentRows)
                {
                    foreach (DataColumn column in mIKO2016DataSet.nw_nventa.Columns)
                        Console.Write("\t{0}", row[column]);

                    Console.WriteLine("\t" + row.RowState);
                }
            }
            #endregion
            //----------------------------------fin print datatable content
        }

        public void CargaEKfromDB()
        {
            DataRow[] currentRows;
            //
            // Carga desde la DB los registros del dia actual de la tabla ek_nventa usando FillBy1
            // FillBy1 carga los registros del dia actual que no tengan Estado3=Entregado
            //
            mIKO2016DataSet1ek_nventaTableAdapter = new OrdVenta01.MIKO2016DataSet1TableAdapters.ek_nventaTableAdapter();
            mIKO2016DataSet1ek_nventaTableAdapter.Fill(mIKO2016DataSet1.ek_nventa);
            //-----------------------------------print datatable content
            #region
            currentRows = mIKO2016DataSet1.ek_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            Console.WriteLine("Dataset ek");
            if (currentRows.Length < 1)
                Console.WriteLine("No Current Rows Found");
            else
            {
                foreach (DataColumn column in mIKO2016DataSet1.ek_nventa.Columns)
                    Console.Write("\t{0}", column.ColumnName);

                //Console.WriteLine("\tRowState");

                //
                foreach (DataRow row in currentRows)
                {
                    foreach (DataColumn column in mIKO2016DataSet1.ek_nventa.Columns)
                        Console.Write("\t{0}", row[column]);

                    Console.WriteLine("\t" + row.RowState);
                }
            }
            #endregion
            //----------------------------------fin print datatable content
            mIKO2016DataSet2ek_TraeInfoClienteTableAdapter = new OrdVenta01.MIKO2016DataSet2TableAdapters.ek_TraeInfoClienteTableAdapter();

        }


        private Boolean CheckCanalVentaOk()
        {
            // va por cada registro leido de ew y pregunta por el nvCanalNV
            // por cada uno que no este asignado agrega su NVnumero a un string
            // regresa false si hay por lo menos un nvCanalNV == null
            // regresa true si todos los registros leidos tienen  nvCanalNv == true

            mensajeNvSinCanal= " ";
            Boolean canalVentaOk = true;
            //DataRow drCurrent;
            //DataRow[] cliCurrent;
            DataRow[] currentRows = mIKO2016DataSet.nw_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            foreach (DataRow row in currentRows)
            {
                OrdenVentaItem ovItem = new OrdenVentaItem();
                ovItem.NvNumero = (int)row["NVNumero"];
                if(row["nvCanalNV"]==DBNull.Value)
                {
                    mensajeNvSinCanal += " " + Convert.ToString(ovItem.NvNumero);
                    canalVentaOk = false;
                }     
            }
            return (canalVentaOk);
        }

       

        private void CargaInicialColeccionDesdeEw()
        {
            // Selecciona los registros del datatable de ew al arreglo de Datarow
            // Asigna el datarow al objeto ovitem
            // Agrega el ovitem a la coleccion
            // Asigna al drcurrent el contenido de ovitem que va a ir a la BD EK
            //
            DataRow drCurrent;
            DataRow[] cliCurrent;
            DataRow[] currentRows = mIKO2016DataSet.nw_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            foreach (DataRow row in currentRows)
            {
                OrdenVentaItem ovItem = new OrdenVentaItem();
             
                ovItem.NvNumero = (int)row["NVNumero"];
                ovItem.DateCreacion = (DateTime)row["FechaHoraCreacion"];
                ovItem.DateRecepcion = Convert.ToDateTime("2001-01-01 01:00:00.000");
                ovItem.DateLista = Convert.ToDateTime("2001-01-01 01:00:00.000");
                ovItem.DateEntrega = Convert.ToDateTime("2001-01-01 01:00:00.000");

                ovItem.Estado1 = Convert.ToString(row["nvCanalNV"]);
                ovItem.Estado3 = "Nueva";
                ovItem.Estado2 = SetEstado2(ovItem);
                ovItem.Estado4 = "estado4";
                ovItem.Observ1 = "observ1";
                ovItem.Observ2 = "observ2";
                ovItem.Observ3 = "observ3";
                ovItem.Observ4 = "observ4";
                ovItem.DateAux = Convert.ToDateTime("2001-01-01 01:00:00.000");
                ovItem.CodCliente = (String)row["CodAux"];

                // Se Asigna el Nombre del Cliente

                Console.WriteLine("CodCliente=({0})", ovItem.CodCliente);
                mIKO2016DataSet2ek_TraeInfoClienteTableAdapter.Fill(mIKO2016DataSet2.ek_TraeInfoCliente, ovItem.CodCliente.Trim());
                cliCurrent = mIKO2016DataSet2.ek_TraeInfoCliente.Select(null, null, DataViewRowState.CurrentRows);

                foreach (DataRow row2 in cliCurrent)
                {
                    ovItem.Observ1 = (String)row2["NomAux"];
                    Console.WriteLine("row2");
                }


                // se agrega un nuevo objeto a la coleccion
                this.OrdenVentaItems.Add(ovItem);

                // se obtiene un nuevo registro para la datatable ek
                drCurrent = mIKO2016DataSet1.ek_nventa.NewRow();

                // se modifican los campos del registro de la datatable ek

                drCurrent["nvNumero"] = ovItem.NvNumero;
                drCurrent["dateCreacion"] = ovItem.DateCreacion;
                drCurrent["dateRecepcion"] = ovItem.DateRecepcion;
                drCurrent["dateLista"] = ovItem.DateLista;
                drCurrent["dateEntrega"] = ovItem.DateEntrega;
                drCurrent["estado1"] = ovItem.Estado1;
                drCurrent["estado2"] = ovItem.Estado2;
                drCurrent["estado3"] = ovItem.Estado3;
                drCurrent["estado4"] = ovItem.Estado4;
                drCurrent["observ1"] = ovItem.Observ1.Substring(0, Math.Min(50, ovItem.Observ1.Length)); // maximo 50 Caracteres
                drCurrent["observ2"] = ovItem.Observ2;
                drCurrent["observ3"] = ovItem.Observ3;
                drCurrent["observ4"] = ovItem.Observ4;
                drCurrent["dateAux"] = ovItem.DateAux;
                drCurrent["CodCliente"] = ovItem.CodCliente;

                // se agrega el nuevo registro a la datatable ek
                try
                {
                    mIKO2016DataSet1.ek_nventa.Rows.Add(drCurrent);
                }
                catch (System.Exception ex)
                {

                    Console.WriteLine("Reg DB{0}\t",ovItem.NvNumero );
                    Console.WriteLine("Exception {0}", ex);
                }
                //  update de la BD con el tableadapter de ek

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
                Console.WriteLine("ek Updated\t");
            }
            foreach(var itr in OrdenVentaItems)
            {
                Console.WriteLine("OrdVIt={0}", Convert.ToString(itr.NvNumero));
            }
            //----------------------------------fin print datatable content
        }

        private void MergeColeccionConEk()
        {
            List<OrdenVentaItem> oviList = new List<OrdenVentaItem>();
            DataRow[] currentRows = mIKO2016DataSet1.ek_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            
            foreach (DataRow row in currentRows)
            {
                foreach (var itr in OrdenVentaItems)
                {
                    if (Convert.ToInt32(row["nvNumero"]) == itr.NvNumero) 
                    {
                        if (Convert.ToString(row["Estado3"]).Trim() == "Entregada")
                        {
                            // Crea lista con los items provenientes de ek que tienen Estado3 entregado
                            // y por lo tanto deben ser eliminados de la coleccion
                            oviList.Add(itr);
                            Console.WriteLine("Remover{0}", Convert.ToString(itr.NvNumero));
                            
                        }
                        else
                        {
                            itr.DateCreacion = (DateTime)row["DateCreacion"];
                            itr.DateRecepcion = (DateTime)row["DateRecepcion"];
                            itr.DateLista = (DateTime)row["DateLista"];
                            itr.DateEntrega = (DateTime)row["DateLista"];
                            itr.Estado1 = (String)row["Estado1"];
                            itr.Estado2 = (int)row["Estado2"];
                            itr.Estado3 = Convert.ToString(row["Estado3"]);
                            itr.Estado4 = Convert.ToString(row["Estado4"]);
                            itr.Observ1 = Convert.ToString(row["Observ1"]);
                            itr.Observ2 = Convert.ToString(row["Observ2"]);
                            itr.Observ3 = Convert.ToString(row["Observ3"]);
                            itr.Observ4 = Convert.ToString(row["Observ4"]);
                            itr.DateAux = (DateTime)(row["DateAux"]);
                            itr.CodCliente = Convert.ToString(row["CodCliente"]);
                        }
                    }
                }
            }
            foreach(var itr in oviList)
            {
                Console.WriteLine("Removing{0}\t", Convert.ToString(itr.NvNumero));
                OrdenVentaItems.Remove(itr);
            
            }

            //=====================
            Console.WriteLine("OrdVenta\t");
            foreach (var itr in OrdenVentaItems)
            {
                Console.WriteLine("NVnumero={0}\t", Convert.ToString(itr.NvNumero));
            }
            //=====================
        }

        private void AddToOrdCollection()
        {
           

        }

        private void idText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!esVentas)
            {
                TextBlock tb = new TextBlock();
                tb = (TextBlock)sender;
                Console.WriteLine("item idtext click ={0}", tb.Text);    
                //OVActionWindow oVAction = new OVActionWindow(tb.Text, ordenVentaItems);
                //oVAction.ShowDialog();
                //Console.WriteLine("print click ={0}", nvnumero);

                WindowRpt win3 = new WindowRpt(Convert.ToInt32(tb.Text));    // Se llama la ventana de windowreport y alli se hace el preview del crystal report
                win3.Show();

            }

        }

        public void DispatcherTimerMethod()
        {
            //  DispatcherTimer setup
           
            DispatcherTimer dispTimer = new DispatcherTimer();
            dispTimer.Interval = TimeSpan.FromSeconds(5);
            dispTimer.Tick += dispTimer_Tick;
            dispTimer.Start();
            
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            
            Console.WriteLine("Timer={0}",DateTime.Now.ToLongTimeString());
            CargaRecurrenteFromDB();
            RemoveNotasEntregadas();
            //SystemSounds.Beep.Play();
            //SystemSounds.Asterisk.Play();
            //SystemSounds.Exclamation.Play();
            if (HayCodigoRojo())
            {
                SystemSounds.Hand.Play();
            }
        }

        public Boolean HayCodigoRojo()
        {
            foreach (var itr in ordenVentaItems)
            {
                if (Convert.ToInt32(itr.Estado2) == 1)
                {
                    return (true);
                }
            }
            return (false);
        }

        public void CargaRecurrenteFromDB()
        {
            DataRow drCurrent;
            DataRow[] currentRows;
            DataRow[] cliCurrent;
            int i = 0;
            
            AseguraCanalOk();
            

            //
            // Carga desde la DB los registros del dia actual de la tabla nw_nventa usando Fill
            //
            //mIKO2016DataSetnw_nventaTableAdapter = new OrdVenta01.MIKO2016DataSetTableAdapters.nw_nventaTableAdapter();
            //mIKO2016DataSetnw_nventaTableAdapter.Fill(mIKO2016DataSet.nw_nventa);
            //mIKO2016DataSet.nw_nventa.AcceptChanges();
            //-
            currentRows = mIKO2016DataSet.nw_nventa.Select(
            null, null, DataViewRowState.CurrentRows);
            Console.WriteLine("Dataset ew");
            if (currentRows.Length < 1)
                Console.WriteLine("No Current Rows Found");
            else
            {
                foreach (DataRow row in currentRows)   // Hay registros en ew
                {
                    if (ordenVentaItems.Count() > 0)  // Hay elementos en ordenVentaItems
                    {
                        if (Convert.ToInt32(row["nvNumero"]) > ordenVentaItems.Last().NvNumero)
                        {
                            OrdenVentaItem ovItem = new OrdenVentaItem();

                            ovItem.NvNumero = (int)row["NVNumero"];
                            ovItem.DateCreacion = (DateTime)row["FechaHoraCreacion"];
                            ovItem.DateRecepcion = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.DateLista = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.DateEntrega = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.Estado1 = Convert.ToString(row["nvCanalNV"]);
                            //ovItem.Estado1 = "CliEsp";
                            ovItem.Estado3 = "Nueva";
                            ovItem.Estado2 = SetEstado2(ovItem);
                            ovItem.Estado4 = "estado4 ";
                            ovItem.Observ1 = "observ1";
                            ovItem.Observ2 = "observ2";
                            ovItem.Observ3 = "observ3";
                            ovItem.Observ4 = "observ4";
                            ovItem.DateAux = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.CodCliente = (String)row["CodAux"];

                            // Se Asigna el Nombre del Cliente

                            Console.WriteLine("CodCliente=({0})", ovItem.CodCliente);
                            mIKO2016DataSet2ek_TraeInfoClienteTableAdapter.Fill(mIKO2016DataSet2.ek_TraeInfoCliente, ovItem.CodCliente.Trim());
                            cliCurrent = mIKO2016DataSet2.ek_TraeInfoCliente.Select(null, null, DataViewRowState.CurrentRows);

                            foreach (DataRow row2 in cliCurrent)
                            {
                                ovItem.Observ1 = (String)row2["NomAux"];
                                Console.WriteLine("row2");
                            }
                            // se agrega un nuevo objeto a la coleccion
                            this.OrdenVentaItems.Add(ovItem);

                            // se obtiene un nuevo registro para la datatable ek
                            drCurrent = mIKO2016DataSet1.ek_nventa.NewRow();

                            // se modifican los campos del registro de la datatable ek

                            drCurrent["nvNumero"] = ovItem.NvNumero;
                            drCurrent["dateCreacion"] = ovItem.DateCreacion;
                            drCurrent["dateRecepcion"] = ovItem.DateRecepcion;
                            drCurrent["dateLista"] = ovItem.DateLista;
                            drCurrent["dateEntrega"] = ovItem.DateEntrega;
                            drCurrent["estado1"] = ovItem.Estado1;
                            drCurrent["estado2"] = ovItem.Estado2;
                            drCurrent["estado3"] = ovItem.Estado3;
                            drCurrent["estado4"] = ovItem.Estado4;
                            drCurrent["observ1"] = ovItem.Observ1.Substring(0, Math.Min(50, ovItem.Observ1.Length)); // maximo 50 Caracteres
                            drCurrent["observ2"] = ovItem.Observ2;
                            drCurrent["observ3"] = ovItem.Observ3;
                            drCurrent["observ4"] = ovItem.Observ4;
                            drCurrent["dateAux"] = ovItem.DateAux;
                            drCurrent["CodCliente"] = ovItem.CodCliente;

                            //  Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                            if (drCurrent != null)
                                try
                                {
                                    //se agrega el nuevo registro a la datatable ek

                                    mIKO2016DataSet1.ek_nventa.Rows.Add(drCurrent);

                                    //  update de la BD con el tableadapter de ek

                                    SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                                    mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
                                    Console.WriteLine("ek-recurrent Updated\t");
                                }
                                catch (System.Exception ex)
                                {

                                    //Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                                    Console.WriteLine("Exception {0}", ex);
                                }
                        }
                    }
                    else // No hay elementos en ordenVentaItems pero si hay en ew
                    {
                        DataRow[] rowsEk = mIKO2016DataSet1.ek_nventa.Select(
                        null, null, DataViewRowState.CurrentRows);
                        foreach (DataRow row3 in rowsEk)
                            i = i++;

                        if (rowsEk.Length > 0) // si hay registros de hoy en ek
                        {
                            if (Convert.ToInt32(row["nvNumero"]) > (Convert.ToInt32(rowsEk[rowsEk.Length-1]["NVNumero"])))
                            //  El registro de ew es mayor que el ultimo de ek
                            // Hay que agregar el registro a ordenVentaItems
                            // e insertarlo en ek
                            {
                                OrdenVentaItem ovItem = new OrdenVentaItem();

                                ovItem.NvNumero = (int)row["NVNumero"];
                                ovItem.DateCreacion = (DateTime)row["FechaHoraCreacion"];
                                ovItem.DateRecepcion = Convert.ToDateTime("2001-01-01 01:00:00.000");
                                ovItem.DateLista = Convert.ToDateTime("2001-01-01 01:00:00.000");
                                ovItem.DateEntrega = Convert.ToDateTime("2001-01-01 01:00:00.000");
                                ovItem.Estado1 = Convert.ToString(row["nvCanalNV"]);
                                //ovItem.Estado1 = "CliEsp";
                                ovItem.Estado3 = "Nueva";
                                ovItem.Estado2 = SetEstado2(ovItem);
                                ovItem.Estado4 = "estado4 ";
                                ovItem.Observ1 = "observ1";
                                ovItem.Observ2 = "observ2";
                                ovItem.Observ3 = "observ3";
                                ovItem.Observ4 = "observ4";
                                ovItem.DateAux = Convert.ToDateTime("2001-01-01 01:00:00.000");
                                ovItem.CodCliente = (String)row["CodAux"];

                                // Se Asigna el Nombre del Cliente

                                Console.WriteLine("CodCliente=({0})", ovItem.CodCliente);
                                mIKO2016DataSet2ek_TraeInfoClienteTableAdapter.Fill(mIKO2016DataSet2.ek_TraeInfoCliente, ovItem.CodCliente.Trim());
                                cliCurrent = mIKO2016DataSet2.ek_TraeInfoCliente.Select(null, null, DataViewRowState.CurrentRows);

                                foreach (DataRow row2 in cliCurrent)
                                {
                                    ovItem.Observ1 = (String)row2["NomAux"];
                                    Console.WriteLine("row2");
                                }
                                // se agrega un nuevo objeto a la coleccion
                                this.OrdenVentaItems.Add(ovItem);

                                // se obtiene un nuevo registro para la datatable ek
                                drCurrent = mIKO2016DataSet1.ek_nventa.NewRow();

                                // se modifican los campos del registro de la datatable ek

                                drCurrent["nvNumero"] = ovItem.NvNumero;
                                drCurrent["dateCreacion"] = ovItem.DateCreacion;
                                drCurrent["dateRecepcion"] = ovItem.DateRecepcion;
                                drCurrent["dateLista"] = ovItem.DateLista;
                                drCurrent["dateEntrega"] = ovItem.DateEntrega;
                                drCurrent["estado1"] = ovItem.Estado1;
                                drCurrent["estado2"] = ovItem.Estado2;
                                drCurrent["estado3"] = ovItem.Estado3;
                                drCurrent["estado4"] = ovItem.Estado4;
                                drCurrent["observ1"] = ovItem.Observ1.Substring(0, Math.Min(50, ovItem.Observ1.Length)); // maximo 50 Caracteres
                                drCurrent["observ2"] = ovItem.Observ2;
                                drCurrent["observ3"] = ovItem.Observ3;
                                drCurrent["observ4"] = ovItem.Observ4;
                                drCurrent["dateAux"] = ovItem.DateAux;
                                drCurrent["CodCliente"] = ovItem.CodCliente;

                                //  Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                                if (drCurrent != null)
                                    try
                                    {
                                        //se agrega el nuevo registro a la datatable ek

                                        mIKO2016DataSet1.ek_nventa.Rows.Add(drCurrent);

                                        //  update de la BD con el tableadapter de ek

                                        SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                                        mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
                                        Console.WriteLine("ek-recurrent Updated\t");
                                    }
                                    catch (System.Exception ex)
                                    {

                                        //Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                                        Console.WriteLine("Exception {0}", ex);
                                    }

                            }  //Si el registro de ew no es mayor que el ultimo de ek entonces no hacemos nada, que pase al siguiente}
                        } 
                        else
                        {      // Si no hay registros de hoy en ek se inserta el registro en ordenVentaItems y en ek
                            OrdenVentaItem ovItem = new OrdenVentaItem();

                            ovItem.NvNumero = (int)row["NVNumero"];
                            ovItem.DateCreacion = (DateTime)row["FechaHoraCreacion"];
                            ovItem.DateRecepcion = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.DateLista = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.DateEntrega = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.Estado1 = Convert.ToString(row["nvCanalNV"]);
                            //ovItem.Estado1 = "CliEsp";
                            ovItem.Estado3 = "Nueva";
                            ovItem.Estado2 = SetEstado2(ovItem);
                            ovItem.Estado4 = "estado4 ";
                            ovItem.Observ1 = "observ1";
                            ovItem.Observ2 = "observ2";
                            ovItem.Observ3 = "observ3";
                            ovItem.Observ4 = "observ4";
                            ovItem.DateAux = Convert.ToDateTime("2001-01-01 01:00:00.000");
                            ovItem.CodCliente = (String)row["CodAux"];

                            // Se Asigna el Nombre del Cliente

                            Console.WriteLine("CodCliente=({0})", ovItem.CodCliente);
                            mIKO2016DataSet2ek_TraeInfoClienteTableAdapter.Fill(mIKO2016DataSet2.ek_TraeInfoCliente, ovItem.CodCliente.Trim());
                            cliCurrent = mIKO2016DataSet2.ek_TraeInfoCliente.Select(null, null, DataViewRowState.CurrentRows);

                            foreach (DataRow row2 in cliCurrent)
                            {
                                ovItem.Observ1 = (String)row2["NomAux"];
                                Console.WriteLine("row2");
                            }
                            // se agrega un nuevo objeto a la coleccion
                            this.OrdenVentaItems.Add(ovItem);

                            // se obtiene un nuevo registro para la datatable ek
                            drCurrent = mIKO2016DataSet1.ek_nventa.NewRow();

                            // se modifican los campos del registro de la datatable ek

                            drCurrent["nvNumero"] = ovItem.NvNumero;
                            drCurrent["dateCreacion"] = ovItem.DateCreacion;
                            drCurrent["dateRecepcion"] = ovItem.DateRecepcion;
                            drCurrent["dateLista"] = ovItem.DateLista;
                            drCurrent["dateEntrega"] = ovItem.DateEntrega;
                            drCurrent["estado1"] = ovItem.Estado1;
                            drCurrent["estado2"] = ovItem.Estado2;
                            drCurrent["estado3"] = ovItem.Estado3;
                            drCurrent["estado4"] = ovItem.Estado4;
                            drCurrent["observ1"] = ovItem.Observ1.Substring(0, Math.Min(50, ovItem.Observ1.Length)); // maximo 50 Caracteres
                            drCurrent["observ2"] = ovItem.Observ2;
                            drCurrent["observ3"] = ovItem.Observ3;
                            drCurrent["observ4"] = ovItem.Observ4;
                            drCurrent["dateAux"] = ovItem.DateAux;
                            drCurrent["CodCliente"] = ovItem.CodCliente;

                            //  Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                            if (drCurrent != null)
                                try
                                {
                                    //se agrega el nuevo registro a la datatable ek

                                    mIKO2016DataSet1.ek_nventa.Rows.Add(drCurrent);

                                    //  update de la BD con el tableadapter de ek

                                    SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                                    mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
                                    Console.WriteLine("ek-recurrent Updated\t");
                                }
                                catch (System.Exception ex)
                                {

                                    //Console.WriteLine("Reg DB{0}\t", ovItem.NvNumero);
                                    Console.WriteLine("Exception {0}", ex);
                                }

                        }


                    }

                    //  Console.WriteLine("Last={0}", ordenVentaItems.Last().NvNumero);
                }
            }
            
            
        }

        //private void ButtonEstado3_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Console.WriteLine("WW");
        //    Console.WriteLine("Estado3Button");
        //}
        //internal OrdenVentaItem findOrdenVentaItem(int nvnumero)
        //{
        //    foreach (var itr in ordenVentaItems)
        //    {
        //        if (nvnumero == itr.NvNumero)
        //        {
        //            Console.WriteLine("itr={0}", itr.ToString());
        //            return (itr);
        //        }
        //    }
        //    return (null);
        //}

        private int SetEstado2(OrdenVentaItem ovi)
        //
        // Estado2 es utilizado para determinar el background color
        //  de acuerdo con el siguiente criterio
        //   Estado1  nvCanalNV  Estado2        Estado3     Background
        //   CliEsp   001          1              Nueva       Rojo
        //   CliRet   002          2              Nueva       Amarillo
        //   Despacho 003          3              Nueva       Verde
        //                         4              Recibida    Azul
        //                         5              Lista       Gris
        //                         6              Entregada   Blanco
        //  *La nota de venta Entregada desaparece del display Cada 3 Min

        {
           // double resto = 0;
           // resto = Convert.ToInt32(ovi.NvNumero) % 2.3;
            int estado = 0;
            switch (Convert.ToString(ovi.Estado3).Trim())
            {     
                case "Nueva":
                   switch (Convert.ToString(ovi.Estado1).Trim())
                    {
                        case "001":
                            estado = 1;
                            break;
                        case "002":
                            estado = 2;
                            break;
                        case "003":
                            estado = 3;
                            break;
                    }
                    break;
                    //if ((resto >= 0) && (resto < 0.5))
                    //{
                    //    estado = 1;
                    //}
                    //else
                    //{
                    //    if ((resto >= 0.5) && (resto<=0.8))
                    //        estado = 2;
                    //    else
                    //        estado = 3;
                    //}

                    //break;
                case "Recibida":
                    estado = 4;
                    break;
                case "Lista":
                    estado = 5;
                    break;
            }
            return (estado);
        }

        private void NVAccion_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        { //ya no se usa borrar
           
            OrdenVentaItem ovi = new OrdenVentaItem();

           
            Button btn = new Button();
            btn = (Button)sender;
            ovi = (OrdenVentaItem)btn.DataContext;
            MIKO2016DataSet1.ek_nventaRow ek_NventaRow = mIKO2016DataSet1.ek_nventa.FindBynvNumero(ovi.NvNumero);
            switch (Convert.ToString(btn.Content).Trim())
            {
                case "Recibir":
                    {
                        foreach (var itr in ordenVentaItems)
                        {
                            if (ovi.NvNumero == itr.NvNumero)
                            {
                                itr.Estado3 = "Recibida";
                                itr.Estado2 = 4;
                                itr.DateRecepcion = DateTime.Now;
                                ek_NventaRow.estado3 = "Recibida";
                                ek_NventaRow.estado2 = 4;
                                ek_NventaRow.dateRecepcion = (DateTime)itr.DateRecepcion;
                                break;
                            }
                        }
                    }
                    break;
                case "Completar":
                    {
                        foreach (var itr in ordenVentaItems)
                        {
                            if (ovi.NvNumero == itr.NvNumero)
                            {
                                itr.Estado3 = "Lista";
                                itr.Estado2 = 5;
                                itr.DateLista = DateTime.Now;
                                ek_NventaRow.estado3 = "Lista";
                                ek_NventaRow.estado2 = 5;
                                ek_NventaRow.dateLista = (DateTime)itr.DateLista;
                                break;
                            }
                        }
                    }
                    break;
                case "Entregar":
                    {
                        foreach (var itr in ordenVentaItems)
                        {
                            if (ovi.NvNumero == itr.NvNumero)
                            {
                                itr.Estado3 = "Entregada";
                                itr.Estado2 = 6;
                                itr.DateEntrega = DateTime.Now;
                                ek_NventaRow.estado3 = "Entregada";
                                ek_NventaRow.estado2 = 6;
                                ek_NventaRow.dateEntrega = (DateTime)itr.DateEntrega;
                                break;
                            }
                        }
                        OrdenVentaItems.Remove(ovi);
                    }
                    break;

            }
            
                        
            Console.WriteLine("Accion\t");
            Console.WriteLine("ov{0}", ovi.NvNumero);    
            
            SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
            mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
        }

        private void Recibido_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!esVentas)
            {
                OrdenVentaItem ovi = new OrdenVentaItem();


                TextBlock tbk = new TextBlock();
                tbk = (TextBlock)sender;
                ovi = (OrdenVentaItem)tbk.DataContext;
                MIKO2016DataSet1.ek_nventaRow ek_NventaRow = mIKO2016DataSet1.ek_nventa.FindBynvNumero(ovi.NvNumero);
                if (ovi.Estado3.Trim() == "Nueva")
                {
                    foreach (var itr in ordenVentaItems)
                    {
                        if (ovi.NvNumero == itr.NvNumero)
                        {
                            itr.Estado3 = "Recibida";
                            itr.Estado2 = 4;
                            itr.DateRecepcion = DateTime.Now;
                            ek_NventaRow.estado3 = "Recibida";
                            ek_NventaRow.estado2 = 4;
                            ek_NventaRow.dateRecepcion = (DateTime)itr.DateRecepcion;
                            break;
                        }
                    }
                }


                Console.WriteLine("Accion\t");
                Console.WriteLine("ov{0}", ovi.NvNumero);
                PrintReportWhenReceived pr = new PrintReportWhenReceived();
                pr.PrintReport(ovi.NvNumero);
                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
            }
        }

        private void Listo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!esVentas)
            {
                OrdenVentaItem ovi = new OrdenVentaItem();


                TextBlock tbk = new TextBlock();
                tbk = (TextBlock)sender;
                ovi = (OrdenVentaItem)tbk.DataContext;
                MIKO2016DataSet1.ek_nventaRow ek_NventaRow = mIKO2016DataSet1.ek_nventa.FindBynvNumero(ovi.NvNumero);
                if (ovi.Estado3.Trim() == "Recibida")
                {
                    foreach (var itr in ordenVentaItems)
                    {
                        if (ovi.NvNumero == itr.NvNumero)
                        {
                            itr.Estado3 = "Lista";
                            itr.Estado2 = 5;
                            itr.DateLista = DateTime.Now;
                            ek_NventaRow.estado3 = "Lista";
                            ek_NventaRow.estado2 = 5;
                            ek_NventaRow.dateLista = (DateTime)itr.DateLista;
                            break;
                        }
                    }
                }


                Console.WriteLine("Accion\t");
                Console.WriteLine("ov{0}", ovi.NvNumero);

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
            }
        }

        private void Entregado_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!esVentas)
            {
                OrdenVentaItem ovi = new OrdenVentaItem();


                TextBlock tbk = new TextBlock();
                tbk = (TextBlock)sender;
                ovi = (OrdenVentaItem)tbk.DataContext;
                MIKO2016DataSet1.ek_nventaRow ek_NventaRow = mIKO2016DataSet1.ek_nventa.FindBynvNumero(ovi.NvNumero);
                if (ovi.Estado3.Trim() == "Lista")
                {
                    foreach (var itr in ordenVentaItems)
                    {
                        if (ovi.NvNumero == itr.NvNumero)
                        {
                            itr.Estado3 = "Entregada";
                            itr.Estado2 = 6;
                            itr.DateEntrega = DateTime.Now;
                            ek_NventaRow.estado3 = "Entregada";
                            ek_NventaRow.estado2 = 6;
                            ek_NventaRow.dateEntrega = (DateTime)itr.DateEntrega;
                            break;
                        }
                    }
                }


                Console.WriteLine("Accion\t");
                Console.WriteLine("ov{0}", ovi.NvNumero);

                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder();
                mIKO2016DataSet1ek_nventaTableAdapter.Update(mIKO2016DataSet1.ek_nventa);
            }
        }

        private void RemoveNotasEntregadas()
        {
            int i = 0;
            OrdenVentaItem[] ovi = new OrdenVentaItem[5];
            foreach (var itr in ordenVentaItems)
            {
                if ((itr.Estado3.Trim() == "Entregada") && i<5)
                {
                    ovi[i] = itr;
                    i++;
                }
            }
            if (i > 0)
            {
                for (i = 0; i < ovi.Length; i++)
                {
                    OrdenVentaItems.Remove(ovi[i]);
                }
            }
           

        }
        //=============================================

        //==============================================================
    }
    

}
