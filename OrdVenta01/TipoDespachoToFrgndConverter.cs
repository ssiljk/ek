using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;

namespace OrdVenta01
{
    public class TipoDespachoToFrgndConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int td = (int)value;
            switch (td)
            {
                case 1:
                    return "White";
                case 2:
                    return "Black";
                case 3:
                    return "White";
                case 4:
                    return "White";
                case 5:
                    return "White";
                case 6:
                    return "Black";
                default:
                    return "Black";



            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
