using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvAp2.Interfaces;
using AvAp2.Models.BaseClasses;
using IProjectModel;

namespace AvAp2.Models.Controls
{
    [Description("Гиперссылка")]
    public class CHyperLink : BasicWithTextName, IUserRights
    {
        #region IUserRights

        [Category("Навигация"), Description("Необходимый уровень доступа для перехода или запуска"), PropertyGridFilter, DisplayName("Права"), Browsable(true)]
        public UserInterfaceRights LinkRights
        {
            get => (UserInterfaceRights)GetValue(LinkRightsProperty);
            set
            {
                SetValue(LinkRightsProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<UserInterfaceRights> LinkRightsProperty = AvaloniaProperty.Register<CHyperLink, UserInterfaceRights>(nameof(LinkRights), UserInterfaceRights.Base);// { AffectsRender = true });

        #endregion IUserRights

        [Category("Свойства элемента мнемосхемы"), Description("Элемент перемещается по 30-сетке"), PropertyGridFilter, DisplayName("Сетка"), Browsable(false)]
        public override bool ControlIs30Step
        {
            get => false;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Адрес удалённого WEB-узла или путь на диске к файлу. Путь м.б. как абсолютным (C:\\FolderName\\FileName.pdf), так и относительным, где ..\\ - один шаг вверх от АРМ.exe (например: ..\\..\\FolderName\\FileName.pdf)"), PropertyGridFilter, DisplayName("Ссылка, документ или файл."), Browsable(true)]
        public string HyperLinkURL
        {
            get => (string)GetValue(HyperLinkURLProperty);
            set
            {
                SetValue(HyperLinkURLProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> HyperLinkURLProperty = AvaloniaProperty.Register<CHyperLink, string>(nameof(HyperLinkURL), "http://address.com");

        [Category("Свойства элемента мнемосхемы"), Description("Имя файла изображения, файл должен лежать в папке Assets"), PropertyGridFilter, DisplayName("Изображение"), Browsable(true)]
        public string ImageFileName
        {
            get => (string)GetValue(ImageFileNameProperty);
            set
            {
                SetValue(ImageFileNameProperty, value);
                RiseMnemoNeedSave();
            }
        }
        public static StyledProperty<string> ImageFileNameProperty = AvaloniaProperty.Register<CHyperLink, string>(nameof(ImageFileName), "HyperLink.png");
        private static void OnASUImageFileNamePropertyChanged(AvaloniaPropertyChangedEventArgs<string> obj)
        {
            if (obj.NewValue.Value.Length > 0)
            {
                try
                {
                    var assests = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                    var img = new Bitmap(assests.Open(new Uri($@"avares://{name}/Assets/{obj.NewValue.Value}")));
                    (obj.Sender as CHyperLink).ImageSource = img;
                    return;
                }
                catch (Exception) { }// Если по какой-либо причине не удалось, установим по умолчанию
            }
            // BitmapImage imgDef = new BitmapImage(new Uri("Images/CHyperLink.png", UriKind.Relative));
            //BitmapImage imgDef = new BitmapImage(new Uri("pack://application:,,,/MnemoElementsLibrary;component/Images/CHyperLink.png"));
            //isw.ImageSource = imgDef;
        }

        [Category("Свойства элемента мнемосхемы"), Description("Источник изображения"), PropertyGridFilter, DisplayName("Изображение"), Browsable(false)]
        private Bitmap ImageSource
        {
            get => GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        public static StyledProperty<Bitmap> ImageSourceProperty = AvaloniaProperty.Register<CHyperLink, Bitmap>("ImageSource", new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri($@"avares://{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}/Assets/HyperLink.png"))));

        static CHyperLink()
        {
            ImageFileNameProperty.Changed.Subscribe(OnASUImageFileNamePropertyChanged);
        }
        public CHyperLink() : base()
        {
            TextName = "Внешняя ссылка";
            ControlISHitTestVisible = true;
            TextNameWidth = 200;
            MarginTextName = new Thickness(0);
        }

        public override string ElementTypeFriendlyName
        {
            get => "Ссылка";
        }
        public override object Clone()
        {
            return ObjectCopier.Clone(this);
        }


        public override void Render(DrawingContext drawingContext)
        {
            var rotate = drawingContext.PushPostTransform(new RotateTransform(Angle, 15, 15).Value);
            drawingContext.DrawImage(ImageSource, new Rect(1, 1, 27, 27));
            rotate.Dispose();
        }
        
        protected override void DrawIsSelected(DrawingContext ctx)
        {
            if (ControlISSelected)
            {
                var transform = ctx.PushPostTransform(new RotateTransform(AngleTextName, DrawingVisualText.Bounds.Center.X, DrawingVisualText.Bounds.Center.Y).Value);
                ctx.DrawRectangle(BrushIsSelected, PenIsSelected, DrawingVisualText.Bounds);
                transform.Dispose();
            }
        }
        protected override void DrawMouseOver(DrawingContext ctx)
        {
            var transform = ctx.PushPostTransform(new RotateTransform(AngleTextName, DrawingVisualText.Bounds.Center.X, DrawingVisualText.Bounds.Center.Y).Value);
            ctx.DrawRectangle(BrushMouseOver, PenMouseOver, DrawingVisualText.Bounds);
            transform.Dispose();
        }
    }
}