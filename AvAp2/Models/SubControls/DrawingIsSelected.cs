using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AvAp2.Models.SubControls
{
    public class DrawingIsSelected:Control
    {
        private GeometryDrawing _drawingIsSelected => (Parent.Parent as BasicMnemoElement).DrawingIsSelected;
        private Matrix _transform => (Parent.Parent as BasicMnemoElement).DrawingIsSelectedTransform;
        public DrawingIsSelected()
        {
            ClipToBounds = false;
        }

        public override void Render(DrawingContext context)
        {
            
            var t = context.PushPostTransform(_transform);
            if(_drawingIsSelected.Geometry is not null)
                context.DrawGeometry(_drawingIsSelected.Brush, _drawingIsSelected.Pen, _drawingIsSelected.Geometry);
            t.Dispose();
        }
    }
}