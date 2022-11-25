using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using IProjectModel;

namespace AvAp2.Models
{
    [Description("Трехобмоточный трансформатор Версия 1 - Левый")]
    public class CTransformer3CoilsV1Left : CTransformer3CoilsV1
    {
        
        public CTransformer3CoilsV1Left() : base()
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
            
            var rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
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
                    //if (CoilLeftExitIsExist)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColor : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
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
                // Вторичная обмотка
                DrawingContext.PushedState translate;
                if (IsPower)
                    translate = drawingContext.PushPostTransform(new TranslateTransform(0, 30).Value);
                else
                    translate = drawingContext.PushPostTransform(new TranslateTransform(0, 19).Value);
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
                    //if (CoilLeftExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(-15, 15), new Point(-4, 15));
                    //if (CoilTopExitIsExist2)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, -15), new Point(15, -4));                    
                    if (CoilRightExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist2)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage2 : PenContentColorAlternate, new Point(15, 34), new Point(15, 45));
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
                // Третья обмотка
                translate.Dispose();
                DrawingContext.PushedState translate1;
                if (IsPower)
                    translate1 = drawingContext.PushPostTransform(new TranslateTransform(-28, 15).Value);
                else
                    translate1 = drawingContext.PushPostTransform(new TranslateTransform(-17, 9.5).Value);
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
                    if (CoilLeftExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(-16, 15), new Point(-6, 15));
                    if (CoilTopExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(13, -30), new Point(13, -4));
                    //if (CoilRightExitIsExist3)
                    //    drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(34, 15), new Point(45, 15));
                    if (CoilBottomExitIsExist3)
                        drawingContext.DrawLine(isActiveState ? PenContentColorVoltage3 : PenContentColorAlternate, new Point(13, 34), new Point(13, 60));
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
                rotate.Dispose();
        }
        protected override void DrawIsSelected()
        {
            Geometry geometry1 = new RectangleGeometry();
            geometry1.Transform = new RotateTransform(Angle, 15, 15);
            GeometryGroup geometry = new GeometryGroup();
            DrawingIsSelected = new GeometryDrawing();
            if (ControlISSelected)
            {
                //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
                if (IsPower)
                {
                    TranslationX = -37;
                    TranslationY = -7;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 75, 75));
                }
                else
                {
                    TranslationX = -17;
                    TranslationY = 0;
                    geometry1 = new RectangleGeometry(new Rect(0, 0, 49, 49));
                }


                geometry.Children.Add(geometry1);
                if (DrawingVisualText != null && DrawingVisualText.Bounds.Width > 0)
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
            geometry1.Transform = new RotateTransform(Angle, 15, 15);
            GeometryGroup geometry = new GeometryGroup();
            //Вращение не вокруг центра, а вокруг верхнего вывода: 15, -15
            if (IsPower)
            {
                TranslationX = -37;
                TranslationY = -7;
                geometry1 = new RectangleGeometry(new Rect(0, 0, 75, 75));
            }
            else
            {
                TranslationX = -17;
                TranslationY = -0;
                geometry1 = new RectangleGeometry(new Rect(0, 0, 49, 49));
            }


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
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-37, -7, 75, 75));
            else
                drawingContext.DrawRectangle(BrushIsSelected, PenIsSelected, new Rect(-17, 0, 49, 49));
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
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-37, -7, 75, 75));
            else
                drawingContext.DrawRectangle(BrushMouseOver, PenMouseOver, new Rect(-17, 0, 49, 49));
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