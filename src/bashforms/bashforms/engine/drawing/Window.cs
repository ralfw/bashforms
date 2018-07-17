using bashforms.data;

namespace bashforms.engine.drawing
{
    class Window : IDrawing
    {
        public virtual Canvas Draw(object obj) {
            var win = (widgets.windows.Window)obj;
            var canvas = new Canvas(win.Size.width, win.Size.height, win.BackgroundColor, win.ForegroundColor);
            return canvas;
        }
    }
}