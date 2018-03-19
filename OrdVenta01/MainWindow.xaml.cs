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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace OrdVenta01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
       
        CollectionViewSource listingDataView;
        public MainWindow()
        {
            InitializeComponent();
            listingDataView = (CollectionViewSource)(this.Resources["listingDataView"]);
            Console.WriteLine("Esta MainWindow");
            
        }
        //private void Window_Activated(object sender, EventArgs e)
        //{
        //    Window window = (Window)sender;
        //    window.Topmost = false;
        //}
    }
}
