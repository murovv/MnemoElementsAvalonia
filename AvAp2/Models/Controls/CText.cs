using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Текстовое поле")]
    public class CText : BasicWithTextName
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        public CText() : base()
        {
            this.MarginTextName = new Thickness(0);
            this.TextName = "Текст";
        }
        public override string ElementTypeFriendlyName
        {
            get => "Текстовое поле";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }


        protected override void DrawIsSelected()
        {
            
            if (DrawingVisualText.Bounds.Width > 0 && ControlISSelected)
            {
                DrawingIsSelected.Geometry = new RectangleGeometry(DrawingVisualText.Bounds);
            }
            else
            {
                DrawingIsSelected.Geometry = new GeometryGroup();
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingIsSelectedTransform = new RotateTransform(AngleTextName, DrawingVisualText.Bounds.Center.X, DrawingVisualText.Bounds.Center.Y).Value;
            DrawingIsSelectedWrapper.InvalidateVisual();
        }
        protected override void DrawMouseOver()
        {
            if (DrawingVisualText.Bounds.Height > 0)
            {
                DrawingMouseOver.Geometry = new RectangleGeometry(DrawingVisualText.Bounds);
                // DrawingMouseOver.Geometry.Transform = new RotateTransform(AngleTextName);
            }

            DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
            DrawingMouseOverWrapper.Source = new DrawingImage(DrawingMouseOver);
            DrawingMouseOverWrapper.RenderTransform = new RotateTransform(AngleTextName);
        }

        
    }
}