using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Текстовое поле")]
    public class CText : BasicWithTextName
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilter, DisplayName("Сетка"), Browsable(false)]
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


        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (DrawingVisualText.Bounds.Width > 0 && ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(AngleTextName, DrawingVisualText.Bounds.Center.X, DrawingVisualText.Bounds.Center.Y).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                transform.Dispose();
            }
        }
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            if (DrawingVisualText.Bounds.Width > 0)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(AngleTextName, DrawingVisualText.Bounds.Center.X, DrawingVisualText.Bounds.Center.Y).Value);
                ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
                transform.Dispose();
            }
        }

        
    }
}