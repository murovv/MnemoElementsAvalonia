using System.ComponentModel;
using Avalonia;
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
                //DrawingIsSelected.Geometry.Transform = new RotateTransform(AngleTextName,15, 15);
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingIsSelectedWrapper.Source = new DrawingImage(DrawingIsSelected);
            DrawingIsSelectedWrapper.RenderTransform = new RotateTransform(AngleTextName);
            
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