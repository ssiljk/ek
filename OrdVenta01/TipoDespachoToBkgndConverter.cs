using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;
//using System.Windows.Media;
namespace OrdVenta01
{
    public class TipoDespachoToBkgndConverter : IValueConverter
    {
                
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int td = (int)value;
                switch (td)
                {
                    case 1:
                        return "Red";
                    case 2:
                    return "Yellow";
                    case 3:
                        return "Green";
                    case 4:
                        return "Blue";
                    case 5:
                        return "Gray";
                    case 6:
                        return "White";
                default:
                    return "White";

                    

                }
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
    }

}
