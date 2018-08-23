using System.Linq;
using bashforms.adapters;
using bashforms.data;
using bashforms.widgets.windows;
using bashforms.widgets.windows.baseclasses;

namespace bashforms.engine
{
    class Rendering
    {
        private readonly Display _display;
        
        public Rendering(Display display) { _display = display; }
        
        
        public void Render(Window[] windows) {
            if (windows.Length == 0) return;
            
            var canvas = windows.Aggregate(new Canvas(), Render);
            _display.Show(canvas, windows.Last().CursorPosition);
        }

        
        private Canvas Render(Canvas canvas, Window window) {
            var winCanvas = window.Draw();
            canvas.Merge(window.Position.left, window.Position.top, winCanvas);
            return canvas;
        }
    }
}