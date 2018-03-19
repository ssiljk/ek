using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Drawing;
using System.Windows.Media;

namespace OrdVenta01
{
    class Estado1ToCanalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String td = (String)value;
            switch (td.Trim())
            {
                case "001":
                    return "Cliente Esperando";
                case "002":
                    return "Retira mas tarde";
                case "003":
                    return "Despacho";
                default:
                    return "Cliente Esperando";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
