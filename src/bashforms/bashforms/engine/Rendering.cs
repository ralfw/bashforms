using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms.engine
{
    partial class Rendering
    {
        private readonly Dictionary<Type, Func<object, Canvas>> _renderers;

        
        public Rendering() {
            _renderers = new Dictionary<Type, Func<object, Canvas>> {
                {typeof(Window), RenderWindow},
                {typeof(Form), RenderForm},
                {typeof(Label), RenderLabel},
                {typeof(TextLine), RenderTextLine}
            };
        }
        

        public void Render(IEnumerable<Window> windows) {
            var canvas = windows.Aggregate(new Canvas(), Render);
            Draw(canvas);
        }
        
        private Canvas Render(Canvas canvas, Window window) {
            var winCanvas = _renderers[window.GetType()](window);
            canvas.Merge(0,0,winCanvas);

            foreach (var widget in window.Children) {
                var widgetCanvas = _renderers[widget.GetType()](widget);
                canvas.Merge(widget.Position.left, widget.Position.top, widgetCanvas);
            }

            return canvas;
        }

        
        private static void Draw(Canvas canvas) {
            Console.CursorVisible = false;
            
            var tBackground = Console.BackgroundColor;
            var tForeground = Console.ForegroundColor;
            
            for (var x = 0; x < canvas.Width; x++)
            for (var y = 0; y < canvas.Height; y++) {
                var p = canvas[x,y]; if (p == null) continue;
                
                Console.BackgroundColor = p.BackgroundColor;
                Console.ForegroundColor = p.ForegroundColor;
                
                if (y < Console.WindowHeight && x < Console.WindowWidth) {
                    Console.SetCursorPosition(x, y);
                    Console.Write(p.Symbol);
                }
            }

            Console.BackgroundColor = tBackground;
            Console.ForegroundColor = tForeground;
            Console.CursorVisible = true;
        }
    }
}