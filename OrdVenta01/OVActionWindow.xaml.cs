using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace OrdVenta01
{
    /// <summary>
    /// Interaction logic for OVActionWindow.xaml
    /// </summary>
    public partial class OVActionWindow : Window
    {
        private int nvnumero;
        private ObservableCollection<OrdenVentaItem> ordenVentaItems = new ObservableCollection<OrdenVentaItem>();
        public OVActionWindow(string id, ObservableCollection<OrdenVentaItem> ordenVentaItems)
        {             
            InitializeComponent();
            idLabel.Content = id;
            this.ordenVentaItems = ordenVentaItems;
            this.nvnumero = Convert.ToInt32(id);
            //RecibirOV.Visibility = BotonRecibirOVEstado3toVisibility(FindOrdenVentaItem(nvnumero, ordenVentaItems));
            //ListaOV.Visibility = BotonListaOVEstado3toVisibility(FindOrdenVentaItem(nvnumero, ordenVentaItems));
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
          //  Button btn = new Button();
          //  btn = (Button)sender;
          //  ovi = (OrdenVentaItem)btn.DataContext;
          ////  MIKO2016DataSet1.ek_nventaRow ek_NventaRow = mIKO2016DataSet1.ek_nventa.FindBynvNumero(ovi.NvNumero);
            Console.WriteLine("print click ={0}", nvnumero);
            WindowRpt win3 = new WindowRpt(nvnumero);
            win3.Show();
            this.Close();
        }

        private void RecibirOV_Click(object sender, RoutedEventArgs e)
        {
            OrdenVentaItem ovit = new OrdenVentaItem();
            Console.WriteLine("Recibe {0}", Convert.ToString(nvnumero));
            SetRecibida(nvnumero, ordenVentaItems);
            this.Close();
        }

        private void ListaOV_Click(object sender, RoutedEventArgs e)
        {
            OrdenVentaItem ovit = new OrdenVentaItem();
            Console.WriteLine("Lista {0}", Convert.ToString(nvnumero));
            SetLista(nvnumero, ordenVentaItems);
            this.Close();
        }



        private void EntregarOV_Click(object sender, RoutedEventArgs e)
        {
            OrdenVentaItem ovit = new OrdenVentaItem();
            Console.WriteLine("Entrega");
            ovit = FindOrdenVentaItem(nvnumero, ordenVentaItems);
            ordenVentaItems.Remove(ovit);
            this.Close();
        }

        private OrdenVentaItem FindOrdenVentaItem(int nvnumero, ObservableCollection<OrdenVentaItem> ordenVentaItems)
        {
            foreach (var itr in ordenVentaItems)
            {
                if (nvnumero == itr.NvNumero)
                {
                    Console.WriteLine("itr={0}", itr.ToString());
                    return (itr);
                }
            }
            return (null);
        }

        private void SetRecibida(int nvnumero, ObservableCollection<OrdenVentaItem> ordenVentaItems)
        {
            foreach (var itr in ordenVentaItems)
            {
                if (nvnumero == itr.NvNumero)
                {
                    itr.Estado3 = "Recibida";
                    itr.DateRecepcion = DateTime.Now;
                    return;            
                }
            }
        }

        private void SetLista(int nvnumero, ObservableCollection<OrdenVentaItem> ordenVentaItems)
        {
            foreach (var itr in ordenVentaItems)
            {
                if (nvnumero == itr.NvNumero)
                {
                    itr.Estado3 = "Lista";
                    itr.DateLista = DateTime.Now;
                    return;
                }
            }
        }

        private Visibility BotonRecibirOVEstado3toVisibility(OrdenVentaItem ordenVentaItem)
        {
            
            switch (Convert.ToString(ordenVentaItem.Estado3).Trim())
            {
                case "Nueva":
                    return System.Windows.Visibility.Visible;
                case "Recibida":
                    {
                        Console.WriteLine("Converter Recibida");
                        return System.Windows.Visibility.Collapsed;
                    }
                case "Lista":
                    return System.Windows.Visibility.Collapsed;
                default:
                    return System.Windows.Visibility.Collapsed;
            }
                
        }

        private Visibility BotonListaOVEstado3toVisibility(OrdenVentaItem ordenVentaItem)
        {

            switch (Convert.ToString(ordenVentaItem.Estado3).Trim())
            {
                case "Nueva":
                    return System.Windows.Visibility.Visible;
                case "Recibida":
                    {
                        Console.WriteLine("Converter Recibida");
                        return System.Windows.Visibility.Visible;
                    }
                case "Lista":
                    return System.Windows.Visibility.Collapsed;
                default:
                    return System.Windows.Visibility.Collapsed;
            }

        }

        private void VistaPrevia_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
