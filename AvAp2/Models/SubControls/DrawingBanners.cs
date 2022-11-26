using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models.SubControls
{
    public class DrawingBanners:Control
    {
        private TagDataItem TagDataBanners => (Parent.Parent as BasicCommutationDevice).TagDataBanners;
        private ISolidColorBrush BrushContentColor => (Parent.Parent as BasicCommutationDevice).BrushContentColor;
        private ISolidColorBrush BrushContentColorAlternate => (Parent.Parent as BasicCommutationDevice).BrushContentColorAlternate;

        private Brush BrushBlue => (Parent.Parent as BasicCommutationDevice).BrushBlue;
        
        private Pen PenBlack => (Parent.Parent as BasicCommutationDevice).PenBlack;

        private Pen PenContentColorThin => (Parent.Parent as BasicCommutationDevice).PenContentColorThin;
        private Thickness MarginBanner => (Parent.Parent as BasicCommutationDevice).MarginBanner;

        public DrawingBanners()
        {
            
        }
        public override void Render(DrawingContext ctx)
        {
            var transform = ctx.PushPostTransform(new TranslateTransform(MarginBanner.Left, MarginBanner.Top).Value);
            if (TagDataBanners == null) //На время настройки
            {
                #region На время настройки
                FormattedText ft = new FormattedText("Плакаты", CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Black,
                        FontStretch.Normal),
                    12, BrushContentColor);
                ft.TextAlignment = TextAlignment.Left;
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(0, 0, 60, 30));
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(2, 2, 60, 30));
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(0, 0, 60, 30));
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(4, 4, 60, 30));
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(6, 6, 60, 30));
                ctx.DrawRectangle(BrushContentColorAlternate, PenContentColorThin, new Rect(8, 8, 60, 30));
                ctx.DrawText(ft, new Point(12, 13));

                #endregion На время настройки
                
            }
            else
            {
                #region В работе
                if (TagDataBanners?.TagValueString != null)
                {
                    int bannersState = 0;

                    if (int.TryParse(TagDataBanners.TagValueString, out bannersState))
                    {
                        if (bannersState > 0)
                        {
                            if (Convert.ToBoolean(bannersState & 1))
                            {
                                #region 1. Заземлено

                                FormattedText ft = new FormattedText("Заземлено", CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    10, Brushes.WhiteSmoke);

                                ft.MaxTextWidth = 60;
                                ft.TextAlignment = TextAlignment.Left;
                                ctx.DrawRectangle(BrushBlue, PenBlack, new Rect(0, 0, 60, 30));
                                ctx.DrawText(ft, new Point(4, 7));
                                #endregion 1. Заземлено
                            }

                            if (Convert.ToBoolean(bannersState & 2))
                            {
                                #region 2. ИСПЫТАНИЕ

                                FormattedText ft = new FormattedText("ИСПЫТАНИЕ опасно для жизни",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6.2, Brushes.WhiteSmoke);

                                ft.MaxTextWidth = 60;
                                ft.TextAlignment = TextAlignment.Center;
                                ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(2, 2, 60, 30));
                                ctx.DrawText(ft, new Point(2, 7));

                                #endregion 2. ИСПЫТАНИЕ
                            }

                            if (Convert.ToBoolean(bannersState & 4))
                            {
                                #region 3. Транзит разомкнут

                                FormattedText ft = new FormattedText("Транзит разомкнут",
                                    CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    7, Brushes.Black);

                                ft.MaxTextWidth = 50;
                                ft.TextAlignment = TextAlignment.Center;
                                ctx.DrawRectangle(BrushBlue, PenBlack, new Rect(4, 4, 60, 30));
                                ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(8, 8, 52, 22));
                                ctx.DrawText(ft, new Point(9, 8.5));


                                #endregion 3. Транзит разомкнут
                            }

                            if (Convert.ToBoolean(bannersState & 8))
                            {
                                #region 4. Работа под напряжением

                                FormattedText ft = new FormattedText(
                                    "Работа под напряжением \nповторно не включать",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    4.5, Brushes.Red);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;
                                ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(6, 6, 60, 30));
                                ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(10, 10, 52, 22));
                                ctx.DrawText(ft, new Point(10, 11));

                                #endregion 4. Работа под напряжением
                            }

                            if (Convert.ToBoolean(bannersState & 16))
                            {
                                #region 5. НЕ ВКЛЮЧАТЬ! Работают люди

                                FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРаботают люди",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6, Brushes.Red);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;
                                ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(8, 8, 60, 30));
                                ctx.DrawRectangle(Brushes.WhiteSmoke, PenBlack, new Rect(12, 12, 52, 22));
                                ctx.DrawText(ft, new Point(12, 15));

                                #endregion 5. НЕ ВКЛЮЧАТЬ! Работают люди
                            }

                            if (Convert.ToBoolean(bannersState & 32))
                            {
                                #region 6. НЕ ВКЛЮЧАТЬ! Работа на линии

                                FormattedText ft = new FormattedText("НЕ ВКЛЮЧАТЬ!\nРабота на линии",
                                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                    new Typeface(new FontFamily("Segoe UI"), FontStyle.Normal, FontWeight.Bold,
                                        FontStretch.Normal),
                                    6, Brushes.WhiteSmoke);

                                ft.MaxTextWidth = 56;
                                ft.TextAlignment = TextAlignment.Center;

                                ctx.DrawRectangle(Brushes.Red, PenBlack, new Rect(10, 10, 60, 30));
                                ctx.DrawText(ft, new Point(12, 17));
                                #endregion 6. НЕ ВКЛЮЧАТЬ! Работа на линии
                            }
                        }
                    }
                }
                #endregion В работе
            }
            transform.Dispose();
        }
    }
}