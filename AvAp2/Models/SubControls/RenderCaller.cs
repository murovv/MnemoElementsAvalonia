using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models.SubControls
{
    [Description("Контрол - оболочка принимающий функцию отрисовки для рендера")]
    public class RenderCaller:Control
    {
        private Action<DrawingContext> DrawThis { get; set; }

        public RenderCaller(Action<DrawingContext> drawThis)
        {
            DrawThis = drawThis;
        }

        public override void Render(DrawingContext context)
        {
            DrawThis(context);
        }
    }
}