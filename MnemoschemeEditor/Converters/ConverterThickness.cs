using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace MnemoschemeEditor.Converters
{
    public class ConverterThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return thickness.ToString();
            }
            else
            {
                throw new DataValidationException(value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                try
                {
                    var thickness = Thickness.Parse(s);
                    return thickness;
                }
                catch (Exception e)
                {
                    return new Thickness();
                }
            }

            return new Thickness();
        }
    }
}