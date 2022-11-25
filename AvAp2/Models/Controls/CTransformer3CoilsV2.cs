using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using IProjectModel;

namespace AvAp2.Models
{
    [Description("Трехобмоточный трансформатор Версия 2")]
    public class CTransformer3CoilsV2 : CTransformer3CoilsV1
    {
        static CTransformer3CoilsV2()
        {
            AffectsRender<CTransformer3CoilsV2>(CoilsConnectionType3Property, CoilLeftExitIsExist3Property, CoilRightExitIsExist3Property, CoilTopExitIsExist3Property, CoilBottomExitIsExist3Property);
        }
        public CTransformer3CoilsV2() : base()
        {
        }
        public override string ElementTypeFriendlyName
        {
            get => "Трехобмоточный трансформатор";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
        #region isActiveState

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

            #endregion isActiveState

            DrawingContext.PushedState rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);

                switch (CoilsConnectionType)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorThin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorThin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    if (CoilLeftExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
                    if (CoilTopExitIsExist && !AutoIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));
                    if (AutoIsExist)
                    {
                        var geometryAuto = AutoLargeGeometry();
                       
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorAutoVoltage : PenContentColorAlternate, geometryAuto);
                    }
                    if (CoilRightExitIsExist)
                        drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 20, 20);

                    if (IsRegulator)
                    {
                        var geometryArrow = ArrowGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, geometryArrow);
                    }
                }
                else
                {
                    //if (CoilLeftExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist)
                    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColor : PenContentColorAlternate, new Point(15, 15), 12, 12);
                }
                //====================================================================================================================================
                // Вторичная обмотка (слева)
                DrawingContext.PushedState translate;
                if (IsPower)
                    translate = drawingContext.PushPostTransform(new TranslateTransform(-18, 30).Value);
                else
                    translate = drawingContext.PushPostTransform(new TranslateTransform(-11, 19).Value);
                switch (CoilsConnectionType2)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2Thin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    if (CoilLeftExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-27, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));                    
                    //if (CoilRightExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(8, 34), new Point(2, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 20, 20);

                }
                else
                {
                    //if (CoilLeftExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 15), 12, 12);
                }

                //====================================================================================================================================
                // Третья обмотка (справа)
                translate.Dispose();
                DrawingContext.PushedState translate1;
                if (IsPower)
                   translate1 = drawingContext.PushPostTransform(new TranslateTransform(18, 30).Value);
                else
                   translate1 = drawingContext.PushPostTransform(new TranslateTransform(11, 19).Value);
                switch (CoilsConnectionType3)
                {
                    case CoilsConnectionTypes.StarConnection:
                        #region StarConnection
                        var geometry1 = StarRayGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry1);
                        var geometry2 = StarRayGeometry();
                        geometry2.Transform = new RotateTransform(120, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry2);
                        var geometry3 = StarRayGeometry();
                        geometry3.Transform = new RotateTransform(240, 15, 15);
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometry3);
                        #endregion StarConnection
                        break;
                    case CoilsConnectionTypes.DeltaConnection:
                        #region DeltaConnection
                        var geometryDelta = DeltaGeometry();
                        
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryDelta);
                        #endregion DeltaConnection
                        break;
                    case CoilsConnectionTypes.VConnection:
                        #region VConnection
                        var geometryV = VGeometry();
                    
                        drawingContext.DrawGeometry(Brushes.Transparent, isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, geometryV);
                        #endregion VConnection
                        break;
                    case CoilsConnectionTypes.OnePhaseConnection:
                        #region OnePhaseConnection
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3Thin : PenContentColorThinAlternate, new Point(15, 24), new Point(15, 4));
                        #endregion OnePhaseConnection
                        break;
                    default:
                        break;
                }

                if (IsPower)
                {
                    //if (CoilLeftExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-16, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(13, -30), new Point(13, -4));
                    if (CoilRightExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(34, 15), new Point(57, 15));
                    if (CoilBottomExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(22, 34), new Point(28, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(14, 15), 20, 20);

                }
                else
                {
                    //if (CoilLeftExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-15, 15), new Point(4, 15));
                    //if (CoilTopExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, -15), new Point(15, 4));
                    //if (CoilRightExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(26, 15), new Point(45, 15));
                    //if (CoilBottomExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, 26), new Point(15, 45));
                    drawingContext.DrawEllipse(Brushes.Transparent, isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(15, 15), 12, 12);
                }
                translate1.Dispose();
                rotation.Dispose();
                
        }
        protected override void DrawIsSelected()
        {
            Geometry geometry1 = new RectangleGeometry();
            var transform = new RotateTransform(Angle, 15, 15).Value;
            DrawingIsSelected = new GeometryDrawing();
            GeometryGroup geometry = new GeometryGroup();
            if (ControlISSelected)
            {
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    TranslationY = -7;
                    TranslationX = -25;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 79, 75));
                }
                else
                {
                    TranslationY = 0;
                    TranslationX = -12;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 54, 49));
                }

                geometry1.Transform = new MatrixTransform(transform);

                geometry.Children.Add(geometry1);
                if (DrawingVisualText.Bounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.Bounds;
                    geometry.Children.Add(new RectangleGeometry(selectedRect));
                }

                DrawingIsSelected.Geometry = geometry;
            }

            DrawingIsSelected.Brush = BrushIsSelected;
            DrawingIsSelected.Pen = PenIsSelected;
            DrawingIsSelectedWrapper.Source = new DrawingImage(DrawingIsSelected);
            DrawingIsSelectedWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY)
                        .Value));
        }
        
        protected override void DrawMouseOver()
        {
            Geometry geometry1 = new RectangleGeometry();
            var transform = new RotateTransform(Angle, 15, 15).Value;
            DrawingMouseOver = new GeometryDrawing();
            GeometryGroup geometry = new GeometryGroup();
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    TranslationY = -7;
                    TranslationX = -25;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 79, 75));
                }
                else
                {
                    TranslationY = 0;
                    TranslationX = -12;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 54, 49));
                }

                geometry1.Transform = new MatrixTransform(transform);

                geometry.Children.Add(geometry1);
                if (DrawingVisualText.Bounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.Bounds;
                    geometry.Children.Add(new RectangleGeometry(selectedRect));
                }

                DrawingMouseOver.Geometry = geometry;

                DrawingMouseOver.Brush = BrushMouseOver;
            DrawingMouseOver.Pen = PenMouseOver;
            DrawingMouseOverWrapper.Source = new DrawingImage(DrawingMouseOver);
            DrawingMouseOverWrapper.RenderTransform =
                new MatrixTransform(
                    new RotateTransform(Angle, 15, 15).Value.Prepend(new TranslateTransform(TranslationX, TranslationY)
                        .Value));
        }
        }

        /*internal protected override void DrawIsSelected()
        {
            using (var drawingContext = DrawingVisualIsSelected.RenderOpen())
            {
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-25, -7, 79, 75));
                else
                    drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-12, 0, 54, 49));
                drawingContext.Pop(); 
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
                drawingContext.PushTransform(new RotateTransform(Angle, 15, 15));//Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-25, -7, 79, 75));
                else
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-12, 0, 54, 49));
                drawingContext.Pop();
                if (DrawingVisualText.ContentBounds.Width > 0)
                {
                    Rect selectedRect = DrawingVisualText.ContentBounds;
                    drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, selectedRect);
                }
                drawingContext.Close();
            }
            DrawingVisualIsMouseOver.Opacity = 0;
        }*/
}