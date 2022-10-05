using System;
using System.Collections.Generic;
using Avalonia.Data.Converters;
using Avalonia.Media;
using IProjectModel;

namespace AvAp2.Converters
{
    public static class VoltageEnumColors
    {
        public static Dictionary<VoltageClasses, Color> VoltageColors = new Dictionary<VoltageClasses, Color>()
        {
            { VoltageClasses.kV1150, Color.FromArgb(255, 205, 138, 255) },
            { VoltageClasses.kV750, Color.FromArgb(255, 0, 0, 200) },
            { VoltageClasses.kV500, Color.FromArgb(255, 165, 15, 10) },
            { VoltageClasses.kV400, Color.FromArgb(255, 240, 150, 30) },
            { VoltageClasses.kV330, Color.FromArgb(255, 0, 140, 0) },
            { VoltageClasses.kV220, Color.FromArgb(255, 200, 200, 0) },
            { VoltageClasses.kV150, Color.FromArgb(255, 170, 150, 0) },
            { VoltageClasses.kV110, Color.FromArgb(255, 0, 180, 200) },
            { VoltageClasses.kV35, Color.FromArgb(255, 130, 100, 50) },
            { VoltageClasses.kV10, Color.FromArgb(255, 100, 0, 100) },
            { VoltageClasses.kV6, Color.FromArgb(255, 200, 150, 100) },
            { VoltageClasses.kV04, Color.FromArgb(255, 190, 190, 190) },
            { VoltageClasses.kVGenerator, Color.FromArgb(255, 230, 70, 230) },
            { VoltageClasses.kVRepair, Color.FromArgb(255, 205, 255, 155) },
            { VoltageClasses.kVEmpty, Colors.White }
        };

        public static Color GetVoltageEnumColor(VoltageClasses AVoltage)
        {
            return VoltageColors[AVoltage];
        }
    }
    
    public class ConverterVoltageEnumToBrush : IValueConverter
    {
        #region Конвертер номера напряжения в цвет 
        public object Convert(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is VoltageClasses v)
                {
                    return new SolidColorBrush( VoltageEnumColors.VoltageColors[v]);
                    #region switch
                    //switch (value)
                    //{
                    //    case VoltageClasses.kV1150:
                    //        return new SolidColorBrush(Color.FromArgb(255, 205, 138, 255));
                    //    case VoltageClasses.kV750:
                    //        return new SolidColorBrush(Color.FromArgb(255, 0, 0, 200));
                    //    case VoltageClasses.kV500:
                    //        return new SolidColorBrush(Color.FromArgb(255, 165, 15, 10));
                    //    case VoltageClasses.kV400:
                    //        return new SolidColorBrush(Color.FromArgb(255, 240, 150, 30));
                    //    case VoltageClasses.kV330:
                    //        return new SolidColorBrush(Color.FromArgb(255, 0, 140, 0));
                    //    case VoltageClasses.kV220:
                    //        return new SolidColorBrush(Color.FromArgb(255, 200, 200, 0));
                    //    case VoltageClasses.kV150:
                    //        return new SolidColorBrush(Color.FromArgb(255, 170, 150, 0));
                    //    case VoltageClasses.kV110:
                    //        return new SolidColorBrush(Color.FromArgb(255, 0, 180, 200));
                    //    case VoltageClasses.kV35:
                    //        return new SolidColorBrush(Color.FromArgb(255, 130, 100, 50));
                    //    case VoltageClasses.kV10:
                    //        return new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
                    //    case VoltageClasses.kV6:
                    //        return new SolidColorBrush(Color.FromArgb(255, 200, 150, 100));
                    //    case VoltageClasses.kV04:
                    //        return new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
                    //    case VoltageClasses.kVGenerator:
                    //        return new SolidColorBrush(Color.FromArgb(255, 230, 70, 230));
                    //    case VoltageClasses.kVRepair:
                    //        return new SolidColorBrush(Color.FromArgb(255, 205, 255, 155));
                    //} 
                    #endregion switch
                }
            }
            return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }
        public object ConvertBack(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion Конвертер номера напряжения в цвет
    }
}