using Avalonia;
using Avalonia.Media;

namespace AvAp2.Interfaces
{
    public interface IBasicWithTextName
    {
        string TextName { get; }
        
        //string TextNameToolTip { get; }
        bool TextNameISVisible { get; }
        Color TextNameColor { get; }
        double TextNameFontSize { get; }
        double TextNameWidth { get; }
        Thickness MarginTextName { get; }
        double AngleTextName { get; }
    }
}