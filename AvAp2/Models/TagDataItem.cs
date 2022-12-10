using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models
{
    /// <summary>
    /// Класс для отображения текущих данных 
    /// </summary>
    public class TagDataItem : INotifyPropertyChanged
    {
        static TagDataItem(){}
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            T oldValue = storage;
            storage = value;
            this.OnPropertyChanged(propertyName);
            //this.RiseValueChanged(propertyName, oldValue.ToString(), value.ToString());
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Событие возникает при изменении значения тега.
        /// </summary>
        public static event EventHandler ValueChanged;

        /// <summary>
        /// Аргумент события изменения значения тега.
        /// </summary>
        public class EventArgsValueChanged : EventArgs
        {
            #region EventArgsValueChanged
            public string PropertyName
            {
                get => propertyName;
            }
            private readonly string propertyName;

            public string OldValue
            {
                get => oldValue;
            }
            private readonly string oldValue;

            public string NewValue
            {
                get => newValue;
            }
            private readonly string newValue;
            public EventArgsValueChanged(string APropertyName, string AOldValue, string ANewValue)
            {
                propertyName = APropertyName;
                oldValue = AOldValue;
                newValue = ANewValue;
            }
            #endregion EventArgsValueChanged
        }

        /// <summary>
        /// Запускает событие при изменении значения тега.
        /// </summary>
        internal protected void RiseValueChanged(string APropertyName, string AOldValue, string ANewValue)
        {
            
            ValueChanged?.Invoke(this, new EventArgsValueChanged(APropertyName, AOldValue, ANewValue));
        }

        #region Свойства
        public object ModelSignal
        {
            get => modelSignal;
            set => SetProperty(ref modelSignal, value);
        }
        private object modelSignal;

        public DateTime TimeStamp
        {
            get => timeStamp;
            set => SetProperty(ref timeStamp, value);
        }
        private DateTime timeStamp;

        public DateTime TimeStampRecieveDate
        {
            get => timeStampRecieveDate;
            set => SetProperty(ref timeStampRecieveDate, value);
        }
        private DateTime timeStampRecieveDate;


        public TagValueQuality Quality
        {
            get => quality;
            set => SetProperty(ref quality, value);
        }
        private TagValueQuality quality;


        public string TagValueString
        {
            get => tagValueString;
            set
            {
                string tagValueStringOld = tagValueString;
                if (SetProperty(ref tagValueString, value))
                    this.RiseValueChanged(nameof(TagValueString), tagValueStringOld, value.ToString());
            }
        }
        private string tagValueString;

        public readonly StyledProperty<string> TagValueStringProperty =
            AvaloniaProperty.Register<BasicWithState, string>(nameof(BasicWithState));
        #endregion

        /// <summary>
        /// Значение тега для отображения
        /// </summary>
        /// <param name="AModelSignal">Сигнал</param>
        public TagDataItem(object AModelSignal)
        {
            ModelSignal = AModelSignal;
            TagValueString = string.Empty;
            Quality = TagValueQuality.Invalid;
            TimeStamp = DateTime.MinValue;
            
        }
    }
}
