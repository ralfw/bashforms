using System;
using System.Collections.Generic;
using System.Linq;

namespace spike
{
    class Rendering
    {
        private readonly Dictionary<Type, Func<object, Canvas>> _renderers;

        
        public Rendering()
        {
            _renderers = new Dictionary<Type, Func<object, Canvas>> {
                {typeof(Window), RenderWindow},
                {typeof(Form), RenderForm},
                {typeof(Label), RenderLabel},
                {typeof(TextLine), RenderTextLine}
            };
        }
        
        
        public void Render(IEnumerable<Window> windows) {
            var canvas = windows.Aggregate(new Canvas(), 
                                           Render);
            Draw(canvas);
        }
        
        private Canvas Render(Canvas canvas, Window window) {
            var winCanvas = _renderers[window.GetType()](window);
            canvas.Merge(0,0,winCanvas);
            return canvas;
        }


        private static Canvas RenderWindow(object obj) {
            var win = (Window)obj;
            var canvas = new Canvas(win.Size.width, win.Size.height);
            foreach (var p in canvas.Points) {
                p.BackgroundColor = win.BackgroundColor;
                p.ForegroundColor = win.ForegroundColor;
            }
            return canvas;
        }
        
        private static Canvas RenderForm(object obj) {
            var canvas = RenderWindow(obj);
            
            var form = (Form) obj;

            for (var y = 0; y < canvas.Height; y++) {
                canvas[0, y].Symbol = '|';
                canvas[canvas.Width - 1, y].Symbol = '|';
            }

            for (var x = 0; x < canvas.Width; x++){
                canvas[x, 0].Symbol = '–';
                canvas[x, canvas.Height - 1].Symbol = '-';
            }
            
            canvas[0, 0].Symbol = '+';
            canvas[canvas.Width-1, 0].Symbol = '+';
            canvas[0, canvas.Height-1].Symbol = '+';
            canvas[canvas.Width-1, canvas.Height-1].Symbol = '+';

            canvas.Write(1,0, form.Title);
            
            return canvas;
        }

        private static Canvas RenderLabel(object obj) {
            var label = (Label) obj;
            return new Canvas(label.Size.width, label.Size.height);
        }
        
        private static Canvas RenderTextLine(object obj) {
            var textLine = (TextLine) obj;
            return new Canvas(textLine.Size.width, textLine.Size.height);
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