using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models
{
    public abstract class CAbstractTransformer:BasicEquipment
    {
        private double _translationX;
        private double _translationY;
        public double TranslationX { 
            get => _translationX;
            protected set
            {
                _translationX = value;
                DrawingMouseOverWrapper.RenderTransform =
                    new MatrixTransform(
                        new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(_translationX, _translationY)
                            .Value));
                DrawingIsSelectedWrapper.RenderTransform = new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(_translationX, _translationY)
                        .Value));
                
            } }
        public double TranslationY { get => _translationY;
            protected set
            {
                _translationY = value;
                DrawingMouseOverWrapper.RenderTransform =
                    new MatrixTransform(
                        new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(_translationX, _translationY)
                            .Value));
                DrawingIsSelectedWrapper.RenderTransform = new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(_translationX, _translationY)
                        .Value));
                
            }  }

        public CAbstractTransformer()
        {
            DrawingMouseOverWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY)
                        .Value));
            DrawingIsSelectedWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY)
                        .Value));
        }
        
       
    }
}