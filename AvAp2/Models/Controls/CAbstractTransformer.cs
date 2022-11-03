using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class CAbstractTransformer:BasicEquipment
    {
        public double TranslationX { get;protected set; }
        public double TranslationY { get; protected set; }
        public override Image DrawingMouseOverWrapper
        {
            get => new Image
            {
                Source = new DrawingImage(DrawingMouseOver),
                RenderTransform =
                    new MatrixTransform(
                        new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY).Value))
            };
        }
        
        public override Image DrawingIsSelectedWrapper
        {
            get => new Image
            {
                Source = new DrawingImage(DrawingIsSelected),
                RenderTransform =
                    new MatrixTransform(
                        new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY).Value))
            };
        }
    }
}