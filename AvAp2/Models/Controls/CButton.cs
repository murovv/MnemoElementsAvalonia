using System.ComponentModel;
using Avalonia;
using Avalonia.Media;

namespace AvAp2.Models
{
    [Description("Кнопка выполнения команды")]
    public class CButton : BasicWithColor
    {
        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilterAttribute, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Идентификатор команды"), PropertyGridFilterAttribute, DisplayName("Команда"), Browsable(true)]
        public string CommandID
        {
            get => (string)GetValue(CommandIDProperty);
            set
            {
                SetValue(CommandIDProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> CommandIDProperty = AvaloniaProperty.Register<CButton, string>(nameof(CommandID), "-1");

        [Category("Свойства элемента мнемосхемы"), Description("Параметр команды (двухпозиционой)"), PropertyGridFilterAttribute, DisplayName("Команда параметр"), Browsable(true)]
        public byte CommandParameter
        {
            get => (byte)GetValue(CommandParameterProperty);
            set
            {
                SetValue(CommandParameterProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<byte> CommandParameterProperty = AvaloniaProperty.Register<CButton, byte>("CommandParameter", (byte)1);


        public CButton() : base()
        {
            this.TextName = "Команда";
            this.ControlISHitTestVisible = true;
            this.ContentColor = Colors.Black;
            this.ContentColorAlternate = Colors.WhiteSmoke;
            this.TextNameWidth = 240;
            this.MarginTextName = new Thickness(10, 0, 0, 0);
        }

        public override string ElementTypeFriendlyName
        {
            get => "Команда";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }


        public override void Render(DrawingContext drawingContext)
        {
            //drawingContext.DrawEllipse(BrushContentColor, PenContentColorThinAlternate, new Point(15, 15), 14, 14);
            drawingContext.DrawRectangle(BrushContentColor, PenContentColorThinAlternate, new Rect(1, 1, 27, 27), 5, 5);
            drawingContext.DrawRectangle(BrushContentColor, PenContentColorThinAlternate, new Rect(11, 11, 7, 7), 2, 2);
        }

    }
}