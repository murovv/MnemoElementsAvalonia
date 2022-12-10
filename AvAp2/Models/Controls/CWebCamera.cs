using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;

namespace AvAp2.Models.Controls
{
    [Description("Камера видеонаблюдения")]
    public class CWebCamera : BasicWithTextName, IVideo
    {
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
        public static StyledProperty<string> VideoLoginProperty = AvaloniaProperty.Register<CWebCamera, string>(nameof(VideoLogin), "admin");

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
        public static StyledProperty<string> VideoPasswordProperty = AvaloniaProperty.Register<CWebCamera, string>(nameof(VideoPassword), "");

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
        public static StyledProperty<string> VideoChannelIDProperty = AvaloniaProperty.Register<CWebCamera, string>(nameof(VideoChannelID), "");

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
        public static StyledProperty<string> VideoChannelPTZProperty = AvaloniaProperty.Register<CWebCamera, string>(nameof(VideoChannelPTZ),"");

        #endregion IVideo
        
        public CWebCamera() : base()
        {
            this.ControlISHitTestVisible = true;
        }
        public override string ElementTypeFriendlyName
        {
            get => "Камера видеонаблюдения";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public override void Render(DrawingContext drawingContext)
        {
            var assests = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Uri uri = new Uri($@"avares://{name}/Assets/video.png");
            try
            {
                
                Bitmap img = new Bitmap(assests.Open(uri));
                drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
                drawingContext.DrawImage(img, new Rect(1, 1, 27, 27));
            }
            catch (Exception) { }// Если по какой-либо причине не удалось, установим по умолчанию
        } 
    }
    
}