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

            foreach (var widget in window.Children) {
                var widgetCanvas = _renderers[widget.GetType()](widget);
                canvas.Merge(widget.Position.left, widget.Position.top, widgetCanvas);
            }

            return canvas;
        }


        private static Canvas RenderWindow(object obj) {
            var win = (Window)obj;
            var canvas = new Canvas(win.Size.width, win.Size.height, win.BackgroundColor, win.ForegroundColor);
            return canvas;
        }
        
        private static Canvas RenderForm(object obj) {
            var form = (Form) obj;
            var canvas = RenderWindow(obj);
            RenderFrame();
            return canvas;


            void RenderFrame() {
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
            }
        }

        private static Canvas RenderLabel(object obj) {
            var label = (Label) obj;
            var canvas = new Canvas(label.Size.width, label.Size.height, label.BackgroundColor, label.ForegroundColor);
            
            canvas.Write(0,0,label.Text);

            return canvas;
        }
        
        private static Canvas RenderTextLine(object obj) {
            var textLine = (TextLine) obj;
            var canvas = new Canvas(textLine.Size.width, textLine.Size.height, textLine.BackgroundColor, textLine.ForegroundColor);

            var text = textLine.Text.PadRight(textLine.Size.width, '_');
            canvas.Write(0,0,text);
            
            if (textLine.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = textLine.FocusBackgroundColor;
                    p.ForegroundColor = textLine.FocusForegroundColor;
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