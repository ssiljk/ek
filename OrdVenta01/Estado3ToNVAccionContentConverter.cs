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
    class Estado3ToNVAccionContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String td = (String)value;
            switch (td.Trim())
            {
                case "Nueva":
                    return "Recibir";
                case "Recibida":
                    return "Completar";
                case "Lista":
                    return "Entregar";
                default:
                    return "default";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
