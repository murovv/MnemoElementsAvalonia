using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Interfaces;


namespace AvAp2.Models
{
    [Description("Индикатор связи")]
    public class CPointLink : BasicWithState, IConnection
    {
        #region IConnection
        [Category("Свойства элемента мнемосхемы"), Description("Значение привязанного тега состояния связи (м.б. 0-нет, 1-есть, 2-ошибка; м.б. 1-Down, 2-Up, 3-Test)"), PropertyGridFilterAttribute, DisplayName("Значение состояние - на связи"), Browsable(true)]
        public string StringStateIsConnected
        {
            get => (string)GetValue(StringStateIsConnectedProperty);
            set
            {
                SetValue(StringStateIsConnectedProperty, value);
                //RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> StringStateIsConnectedProperty = AvaloniaProperty.Register<CPointLink,string>(nameof(StringStateIsConnected), "1");

        #endregion IConnection

        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        public CPointLink() : base()
        {
            AffectsRender<CPointLink>(StringStateIsConnectedProperty);
            this.ControlISHitTestVisible = true;
        }
      
        public override string ElementTypeFriendlyName
        {
            get => "Индикатор связи";
        }
       
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
#warning Не лучший вариант, но по 104 Сашина библиотека выдаёт строки "0" и "1"
            IBrush brush = Brushes.LightGray;
            if (TagDataMainState == null)//Пока ничего не привязано рисуем зелёным
                brush = BrushContentColor;
            else// В рантайме появляется объект - источник привязки - начинаем анализировать
            {
                if (TagDataMainState.TagValueString != null)
                {
                    if (TagDataMainState.TagValueString.Equals(StringStateIsConnected))// В работе если что-то привязано и там связь есть
                        brush = BrushContentColor;
                    else
                        brush = BrushContentColorAlternate;
                }
            }
            DrawingContext.PushedState rotation;
            using (rotation = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value))
            {
                Pen penBorder = new Pen(Brushes.AntiqueWhite, 1);
                drawingContext.DrawRectangle(brush, penBorder, new Rect(11, 12, 8, 6));
            }
        }
    }
}