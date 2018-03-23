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
    class TipoDespachoToImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int td = (int)value;
            switch (td)
            {
                case 1:
                    return "images/check-mark-64.png";
                case 2:
                    return "images/checkmark.png";
                case 3:
                    return "images/check-mark-64.png";
                case 4:
                    return "images/check-mark-64.png";
                case 5:
                    return "images/check-mark-64.png";
                case 6:
                    return "images/checkmark.png";
                default:
                    return "images/checkmark.png";



            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
