using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AIMarbles.Core.Converter
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string enumValue = value.ToString() ?? "";
            string parameterValue = parameter.ToString() ?? "" ;

            return enumValue.Equals(parameterValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(bool)value)
                return Binding.DoNothing; // Or return null if you prefer

            return Enum.Parse(targetType, parameter.ToString() ?? "");
        }
    }
}
