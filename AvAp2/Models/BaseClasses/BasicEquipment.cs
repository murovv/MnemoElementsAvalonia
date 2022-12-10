using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using AvAp2.Converters;
using AvAp2.Interfaces;
using IProjectModel;

namespace AvAp2.Models.BaseClasses
{
    public abstract class BasicEquipment : BasicWithState, IBasicEquipment, IVideo
    {
        #region  У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента"), PropertyGridFilter, DisplayName("Цвет содержимого"), Browsable(false)]
        public override Color ContentColor// У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан
        {
            get => (Color)GetValue(ContentColorProperty);
            set
            {
                SetValue(ContentColorProperty, value);
                RiseMnemoNeedSave();
            }
        }
        [Category("Свойства элемента мнемосхемы"), Description("Цвет содержимого элемента альтернативный"), PropertyGridFilter, DisplayName("Цвет содержимого альтернативный"), Browsable(false)]
        public override Color ContentColorAlternate
        {
            get => (Color)GetValue(ContentColorAlternateProperty);
            set
            {
                SetValue(ContentColorAlternateProperty, value);
                RiseMnemoNeedSave();
            }
        }
        #endregion  У прямоугольника можно просто поменять цвет, а у линии только через класс напряжения, просто цвет спрятан

        #region IVideo
        [Category("Свойства элемента мнемосхемы"), Description("Логин видеонаблюдения"), PropertyGridFilter, DisplayName("Видеонаблюдение логин"), Browsable(true)]
        public string VideoLogin
        {
            get => (string)GetValue(VideoLoginProperty);
            set
            {
                SetValue(VideoLoginProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> VideoLoginProperty = AvaloniaProperty.Register<BasicEquipment,string>(nameof(VideoLogin), "admin");

        [Category("Свойства элемента мнемосхемы"), Description("Пароль видеонаблюдения"), PropertyGridFilter, DisplayName("Видеонаблюдение пароль"), Browsable(true)]
        public string VideoPassword
        {
            get => (string)GetValue(VideoPasswordProperty);
            set
            {
                SetValue(VideoPasswordProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> VideoPasswordProperty = AvaloniaProperty.Register<BasicEquipment,string>(nameof(VideoPassword), "");

        [Category("Свойства элемента мнемосхемы"), Description("Канал видеонаблюдения"), PropertyGridFilter, DisplayName("Видеонаблюдение канал"), Browsable(true)]
        public string VideoChannelID
        {
            get => (string)GetValue(VideoChannelIDProperty);
            set
            {
                SetValue(VideoChannelIDProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> VideoChannelIDProperty = AvaloniaProperty.Register<BasicEquipment,string>(nameof(VideoChannelID), "");

        [Category("Свойства элемента мнемосхемы"), Description("Положение камеры видеонаблюдения"), PropertyGridFilter, DisplayName("Видеонаблюдение положение"), Browsable(true)]
        public string VideoChannelPTZ
        {
            get => (string)GetValue(VideoChannelPTZProperty);
            set
            {
                SetValue(VideoChannelPTZProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> VideoChannelPTZProperty = AvaloniaProperty.Register<BasicEquipment,string>("VideoChannelPTZ", "");

        #endregion IVideo



        [Category("Свойства элемента мнемосхемы"), Description("Класс напряжения элемента"), PropertyGridFilter, DisplayName("Напряжение"), Browsable(true)]
        public VoltageClasses VoltageEnum
        {
            get => (VoltageClasses)GetValue(VoltageEnumProperty);
            set
            {
                SetValue(VoltageEnumProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<VoltageClasses> VoltageEnumProperty = AvaloniaProperty.Register<BasicEquipment,VoltageClasses>(nameof(VoltageEnum),VoltageClasses.kV110);
        private static void OnVoltageChanged(AvaloniaPropertyChangedEventArgs<VoltageClasses> obj)
        {
            #region Смена цвета при изменении класса напряжения
            ((obj.Sender as BasicWithColor)!).ContentColor = VoltageEnumColors.VoltageColors[obj.NewValue.Value];
            #region switch
            //switch ((VoltageClasses)e.NewValue)
            //{
            //    case VoltageClasses.kV1150:
            //        b.ContentColor = Color.FromArgb(255, 205, 138, 255);
            //        break;
            //    case VoltageClasses.kV750:
            //        b.ContentColor = Color.FromArgb(255, 0, 0, 200);
            //        break;
            //    case VoltageClasses.kV500:
            //        b.ContentColor = Color.FromArgb(255, 165, 15, 10);
            //        break;
            //    case VoltageClasses.kV400:
            //        b.ContentColor = Color.FromArgb(255, 240, 150, 30);
            //        break;
            //    case VoltageClasses.kV330:
            //        b.ContentColor = Color.FromArgb(255, 0, 140, 0);
            //        break;
            //    case VoltageClasses.kV220:
            //        b.ContentColor = Color.FromArgb(255, 200, 200, 0);
            //        break;
            //    case VoltageClasses.kV150:
            //        b.ContentColor = Color.FromArgb(255, 170, 150, 0);
            //        break;
            //    case VoltageClasses.kV110:
            //        b.ContentColor = Color.FromArgb(255, 0, 180, 200);
            //        break;
            //    case VoltageClasses.kV35:
            //        b.ContentColor = Color.FromArgb(255, 130, 100, 50);
            //        break;
            //    case VoltageClasses.kV10:
            //        b.ContentColor = Color.FromArgb(255, 100, 0, 100);
            //        break;
            //    case VoltageClasses.kV6:
            //        b.ContentColor = Color.FromArgb(255, 200, 150, 100);
            //        break;
            //    case VoltageClasses.kV04:
            //        b.ContentColor = Color.FromArgb(255, 190, 190, 190);
            //        break;
            //    case VoltageClasses.kVGenerator:
            //        b.ContentColor = Color.FromArgb(255, 230, 70, 230);
            //        break;
            //    case VoltageClasses.kVRepair:
            //        b.ContentColor = Color.FromArgb(255, 205, 255, 155);
            //        break;
            //    default://VoltageClasses.kVEmpty
            //        b.ContentColor = Color.FromArgb(255, 255, 255, 255);
            //        break;
            //} 
            #endregion
            #endregion
        }

        static BasicEquipment()
        {
            AffectsRender<BasicEquipment>(VoltageEnumProperty);
            VoltageEnumProperty.Changed.Subscribe(OnVoltageChanged);
        }
        public BasicEquipment() : base()
        {
            ContentColor = VoltageEnumColors.VoltageColors[VoltageClasses.kV110];// Для состояния под напряжением
            ContentColorAlternate = VoltageEnumColors.VoltageColors[VoltageClasses.kVEmpty];// Для состояния без напряжения
        }
    }
}