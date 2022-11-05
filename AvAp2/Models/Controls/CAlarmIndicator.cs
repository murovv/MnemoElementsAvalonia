using System;
using System.ComponentModel;
using System.Globalization;
using System.Reactive.PlatformServices;
using System.Reactive.Subjects;
using System.Threading;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    /// <summary>
    /// Базовый класс всех контролов схемы диагностики.
    /// </summary>
    [Description("Индикатор тревоги")]
    public class CAlarmIndicator : BasicAlarmIndicator, IBasicWithState
    {
        #region IBasicWithState
        [Category("Привязки данных"), Description("Идентификатор тега состояния элемента. Для текущих данных - ток или дискрет, для оборудования - наличие напряжения для цвета"), PropertyGridFilterAttribute, DisplayName("ID тега состояния"), Browsable(true)]
        public string TagIDMainState
        {
            get => (string)GetValue(TagIDMainStateProperty);
            set
            {
                SetValue(TagIDMainStateProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> TagIDMainStateProperty = AvaloniaProperty.Register<CAlarmIndicator, string>(nameof(TagIDMainState), "-1");

        [Browsable(false)]
        public TagDataItem TagDataMainState
        {
            get => (TagDataItem)GetValue(TagDataMainStateProperty);
            // set => SetValue(TagDataMainStateProperty, value);
            set
            {
                TagDataItem oldValue = (TagDataItem)GetValue(TagDataMainStateProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= TdiState_PropertyChanged;
                    if (value != null)
                        value.PropertyChanged += TdiState_PropertyChanged;
                    SetValue(TagDataMainStateProperty, value);
                }
            }
        }
        public static StyledProperty<TagDataItem> TagDataMainStateProperty = AvaloniaProperty.Register<CAlarmIndicator, TagDataItem>(nameof(TagDataMainState), null);

        protected virtual void TdiState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)))
            {
                if (TagDataMainState.TagValueString.Equals(EventTagValueActive.ToString()))
                    IsActive = true;
                else
                    IsActive = false;
            }
            //if (e.PropertyName.Equals(nameof(TagDataItem.Quality)))
            //    DrawBaseQuality();
        }
        #endregion IBasicWithState

      
        [Category("Свойства элемента мнемосхемы"), Description("Значение тега для активной тревоги."), PropertyGridFilterAttribute, DisplayName("Значение тега"), Browsable(true)]
        public decimal EventTagValueActive
        {
            get => (decimal)GetValue(EventTagValueActiveProperty);
            set => SetValue(EventTagValueActiveProperty, value);
        }
        public static StyledProperty<decimal> EventTagValueActiveProperty = AvaloniaProperty.Register<CAlarmIndicator, decimal>("EventTagValueActive", 1M);

        static CAlarmIndicator()
        {
            AffectsRender<CAlarmIndicator>(OpacityProperty, TagDataMainStateProperty, EventGroupIDProperty);
        }

        public CAlarmIndicator() : base()
        {

            BlinkAnimationController.BlinkOpacityProperty.Changed.Subscribe();
            this.TextName = "Индикатор тревоги";
            this.ControlISHitTestVisible = true;
        }

        public override string ElementTypeFriendlyName
        {
            get => "Индикатор тревоги";
        }

        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            bool isActiveState = false;// При настройке, пока ничего не привязано, рисуем цветом 
            if (TagDataMainState == null)
                isActiveState = true;
            else
            {
                if (TagDataMainState.TagValueString != null)
                    isActiveState = IsActive == true;
            }

            DrawingContext.PushedState rotate;
            using (rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                if (isActiveState)
                {
                    switch (EventGroupID) // Для активной в зависимости от уровня
                    {
                        case 1:
                            drawingContext.DrawRectangle(Brushes.Red, PenWhite1, new Rect(1, 1, 27, 27), 4,4);
                            break;
                        case 2:
                            drawingContext.DrawRectangle(Brushes.OrangeRed, PenWhite1, new Rect(1, 1, 27, 27), 4, 4);
                            break;
                        case 3:
                            drawingContext.DrawRectangle(Brushes.Blue, PenWhite1, new Rect(1, 1, 27, 27), 4, 4);
                            break;
                        case 4:
                            drawingContext.DrawRectangle(Brushes.Green, PenWhite1, new Rect(1, 1, 27, 27), 4, 4);
                            break;
                        default:
                            drawingContext.DrawRectangle(Brushes.LightGray, PenWhite1, new Rect(1, 1, 27, 27), 4, 4);
                            break;
                    }
                }
                else
                    drawingContext.DrawRectangle(Brushes.Gray, PenWhite1, new Rect(1, 1, 27, 27), 4, 4);
                drawingContext.DrawImage(ImageSourceTransparent, new Rect(1, 1, 27, 27));

                FormattedText ftTextName = new FormattedText(TextName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal),
                    TextNameFontSize, BrushTextNameColor);

                ftTextName.TextAlignment = TextAlignment.Left;
                drawingContext.DrawText(ftTextName, new Point(35, 0));
            }
        }


       
    }
}