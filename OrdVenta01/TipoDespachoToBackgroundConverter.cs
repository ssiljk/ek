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
    public class TipoDespachoToBackgroundConverter : IValueConverter
    {
        public Brush Caso1Brush
        {
            get; set;
        }

        public Brush Caso2Brush
        {
            get; set;
        }
        public Brush Caso3Brush
        {
            get; set;
        }
        public Brush Caso4Brush
        {
            get; set;
        }
        public Brush Caso5Brush
        {
            get; set;
        }
        public Brush Caso6Brush
        {
            get; set;
        }
        public Brush DefaultBrush
        {
            get; set;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int td = (int) value;
            switch (td)
            {
                case 1:
                    return Caso1Brush;
                case 2:
                    return Caso2Brush;
                case 3:
                    return Caso3Brush;
                case 4:
                    return Caso4Brush;
                case 5:
                    return Caso5Brush;
                default:
                    return DefaultBrush;

            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
