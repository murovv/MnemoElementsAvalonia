

using System;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace AvAp2.Converters
{
    public class ConverterMnemoTypeToIcon:IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                if (value is Type type)
                {
                    if(assets.Exists(new Uri($"avares://MnemoschemeEditor/Assets/{type.Name}.png"))){
                        Bitmap bitmap = new Bitmap(assets.Open(new Uri($"avares://MnemoschemeEditor/Assets/{type.Name}.png")));
                        return bitmap;
                    }
                    return new Bitmap(assets.Open(new Uri($"avares://MnemoschemeEditor/Assets/InvalidMnemo.png")));
                }
            }
            throw new Exception();
            
        }
        public object ConvertBack(object value, Type typeTarget, object param, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}