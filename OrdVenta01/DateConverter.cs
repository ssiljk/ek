using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Threading.Tasks;

namespace OrdVenta01
{
    [ValueConversion(typeof(DateTime), typeof(System.String))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            // return date.ToShortDateString();
            //return date.ToLongDateString();
            if (date.Year == 2001)
            {
                return ("");
            }
            else
            {
                return date.ToLongTimeString();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return value;
        }
    }
}
