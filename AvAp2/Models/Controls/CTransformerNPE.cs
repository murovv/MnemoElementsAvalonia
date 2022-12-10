using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Трансформатор заземления нейтрали")]
    public class CTransformerNPE : BasicEquipment, IConnector
    {
        
        #region IConnector
        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "), PropertyGridFilter, DisplayName("Видимость соединителя левого"), Browsable(true)]
        public bool IsConnectorExistLeft
        {
            get => (bool)GetValue(IsConnectorExistLeftProperty);
            set
            {
                SetValue(IsConnectorExistLeftProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsConnectorExistLeftProperty = AvaloniaProperty.Register<CTransformerNPE, bool>(nameof(IsConnectorExistLeft), true);

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "), PropertyGridFilter, DisplayName("Видимость соединителя правого"), Browsable(true)]
        public bool IsConnectorExistRight
        {
            get => (bool)GetValue(IsConnectorExistRightProperty);
            set
            {
                SetValue(IsConnectorExistRightProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<bool> IsConnectorExistRightProperty = AvaloniaProperty.Register<CTransformerNPE, bool>("IsConnectorExistRight", true);

        #endregion IConnector

        static CTransformerNPE()
        {
            AffectsRender<CTransformerNPE>(IsConnectorExistLeftProperty, IsConnectorExistRightProperty);
        }
        public CTransformerNPE() : base()
        {
            DataContext = this;
            DrawingIsSelected.InvalidateVisual();
            DrawingIsSelected.InvalidateVisual();
        }
        public override string ElementTypeFriendlyName
        {
            get => "Трансформатор заземления нейтрали";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        private static StreamGeometry TheGeometry()
        {
            StreamGeometry geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(15, 15), false);
                context.LineTo(new Point(24, 18));
                context.LineTo(new Point(32, 15));
            }
            return geometry;
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом класса напряжения
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    if (TagDataMainState.TagValueString.Equals("1"))// В работе если что-то привязано и там "1"
                        isActiveState = true;
            }


            var rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);

                var geometry1 = TheGeometry();
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry1);
                var geometry2 = TheGeometry();
                geometry2.Transform = new RotateTransform(120, 15, 15);
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry2);
                var geometry3 = TheGeometry();
                geometry3.Transform = new RotateTransform(240, 15, 15);
                drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometry3);

                if (IsConnectorExistLeft)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(-2, 15));
                if (IsConnectorExistRight)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(32, 15), new Point(45, 15));

                drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 18, 18);

                rotation.Dispose();
            }
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                DrawingContext.PushedState translate;
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 40, 40));
                if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
                    ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                rotate.Dispose();
            }

        }
        
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            var rotate = ctx.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            DrawingContext.PushedState translate;
            ctx.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 40, 40));
            if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
            rotate.Dispose();
        }
        }
    /*internal protected override void DrawIsSelected()
        {
            using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            {
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-5, -5, 40, 40));
                if (DrawingVisualText.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.ContentBounds;
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsSelected.Opacity = ControlISSelected ? .3 : 0;
        }
        internal protected override void DrawMouseOver()
        {
            using (var drawingContext = DrawingVisualIsMouseOver.RenderOpen())
            {
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-5, -5, 40, 40));
                if (DrawingVisualText.ContentBounds.Width > 0)
                {
                    Rect selectedRect  = DrawingVisualText.ContentBounds;
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsMouseOver.Opacity = 0;
        }*/
}