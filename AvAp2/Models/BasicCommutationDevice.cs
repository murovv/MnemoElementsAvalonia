using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvAp2.Interfaces;

namespace AvAp2.Models
{
    public abstract class BasicCommutationDevice:BasicEquipment,IConnector
    {
        #region IConnector
        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя левого "), DisplayName("Видимость соединителя левого "), Browsable(true)]
        public bool IsConnectorExistLeft
        {
            get => (bool)GetValue(IsConnectorExistLeftProperty);
            set => SetValue(IsConnectorExistLeftProperty, value);
        }
        public static readonly StyledProperty<bool> IsConnectorExistLeftProperty = AvaloniaProperty.Register<CAutomaticSwitch, bool>(nameof(IsConnectorExistLeft));

        [Category("Свойства элемента мнемосхемы"), Description("Видимость соединителя правого "), DisplayName("Видимость соединителя правого "), Browsable(true)]
        public bool IsConnectorExistRight
        {
            get => (bool)GetValue(IsConnectorExistRightProperty);
            set => SetValue(IsConnectorExistRightProperty, value);
        }
        public static readonly StyledProperty<bool> IsConnectorExistRightProperty = AvaloniaProperty.Register<CAutomaticSwitch, bool>(nameof(IsConnectorExistRight));

        #endregion IConnector

        #region нормальный режим
        [Category("Свойства элемента мнемосхемы"), Description("Нормальное состояние КА в соответствии со схемой нормального режима"), DisplayName("Нормальное состояние КА"), Browsable(true)]
        public CommutationDeviceStates NormalState
        {
            get => (CommutationDeviceStates)GetValue(NormalStateProperty);
            set => SetValue(NormalStateProperty, value);
        }
        public static readonly StyledProperty<CommutationDeviceStates> NormalStateProperty = AvaloniaProperty.Register<CAutomaticSwitch, CommutationDeviceStates>(nameof(NormalState));

        [Category("Свойства элемента мнемосхемы"), Description("Отображать отклонения от нормального режима"), DisplayName("Нормальное состояние отображать "), Browsable(true)]
        public bool ShowNormalState
        {
            get => (bool)GetValue(ShowNormalStateProperty);
            set => SetValue(ShowNormalStateProperty, value);
        }
        public static readonly StyledProperty<bool> ShowNormalStateProperty = AvaloniaProperty.Register<CAutomaticSwitch, bool>(nameof(ShowNormalState));

        #endregion нормальный режим
        internal protected Pen PenRed;
        
        internal protected Pen PenNormalState;

        public BasicCommutationDevice() : base()
        {
            PenRed = new Pen(Brushes.Red, 3);
            PenRed.ToImmutable();
            PenNormalState = new Pen(Brushes.Yellow, 1);
            PenNormalState.ToImmutable();
        }
    }
}