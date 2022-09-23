using System.ComponentModel;
using Avalonia;

namespace AvAp2.Models
{
    public abstract class BasicWithState:BasicWithColor
    {
        public BasicWithState() : base()
        {
            AffectsRender<BasicWithState>(TagDataMainStateProperty);
            AffectsRender<BasicWithState>(TdiStateStringProperty);
            AffectsRender<BasicWithState>(TagIDMainStateProperty);
        }

        public string TdiStateString
        {
            get => GetValue(TdiStateStringProperty);
            set => SetValue(TdiStateStringProperty, value);
        }
        public static readonly StyledProperty<string> TdiStateStringProperty = AvaloniaProperty.Register<BasicWithState, string>(nameof(TdiStateString));


        [Category("Привязки данных"), Description("Идентификатор тега состояния элемента. Для текущих данных - ток или дискрет, для оборудования - наличие напряжения для цвета"), DisplayName("ID тега состояния"), Browsable(true)]
        public string TagIDMainState
        {
            get => (string)GetValue(TagIDMainStateProperty);
            set => SetValue(TagIDMainStateProperty, value);
        }
        public static readonly StyledProperty<string> TagIDMainStateProperty = AvaloniaProperty.Register<BasicWithState, string>(nameof(TagIDMainState));

        
        public TagDataItem TagDataMainState
        {
            get => (TagDataItem)GetValue(TagDataMainStateProperty);
            set
            {
                TagDataItem oldValue = GetValue(TagDataMainStateProperty);
                if (oldValue != value)
                {
                    if (oldValue != null)
                        oldValue.PropertyChanged -= Value_PropertyChanged;
                    if (value != null)
                        value.PropertyChanged += Value_PropertyChanged;
                }
                SetValue(TagDataMainStateProperty, value);
            }
        }

        public static StyledProperty<TagDataItem> TagDataMainStateProperty = AvaloniaProperty.Register<BasicWithState, TagDataItem>(nameof(TagDataMainState));


        private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                if (e.PropertyName.Equals(nameof(TagDataItem.TagValueString)))
                    TdiStateString = ((TagDataItem)sender).TagValueString;
                //if (e.PropertyName.Equals(nameof(TagDataItem.Quality)))
                //    Render();
            }
        }
        
    }
}