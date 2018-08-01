using System.Linq;
using bashforms.adapters;
using bashforms.data;
using bashforms.widgets.windows;

namespace bashforms.engine
{
    class Rendering
    {
        public void Render(Window[] windows) {
            if (windows.Length == 0) return;
            
            var canvas = windows.Aggregate(new Canvas(), Render);
            Display.Show(canvas, windows.Last().CursorPosition);
        }

        
        private Canvas Render(Canvas canvas, Window window) {
            var winCanvas = window.Draw();
            canvas.Merge(window.Position.left, window.Position.top, winCanvas);

            foreach (var widget in window.Children) {
                var widgetCanvas = widget.Draw();
                canvas.Merge(window.Position.left + widget.Position.left, window.Position.top + widget.Position.top, widgetCanvas);
            }

            return canvas;
        }
    }
}