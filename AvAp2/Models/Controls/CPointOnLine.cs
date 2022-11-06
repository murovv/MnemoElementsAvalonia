using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Точка соединения")]
    public class CPointOnLine : BasicWithTextName
    {
        [Category("Свойства элемента мнемосхемы"), Description("Диаметр точки"), PropertyGridFilterAttribute, DisplayName("Диаметр точки"), Browsable(true)]
        public double PointDiameter
        {
            get => (double)GetValue(PointDiameterProperty);
            set
            {
                SetValue(PointDiameterProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<double> PointDiameterProperty = AvaloniaProperty.Register<CPointOnLine,double>("PointDiameter", 6.0);

        static CPointOnLine()
        {
            AffectsRender<CPointOnLine>(PointDiameterProperty);

        }

        public CPointOnLine() : base()
        {
            this.TextNameWidth = (double)50;
            this.MarginTextName = new Thickness(0, -20, 0, 0);
        }
        public override string ElementTypeFriendlyName
        {
            get => "Точка соединения";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(BrushContentColor, PenContentColor, new Point(15, 15), PointDiameter / 2, PointDiameter / 2);
        }
    }
}