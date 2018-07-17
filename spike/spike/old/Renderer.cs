using System;

namespace spike
{
    /*
    class Renderer
    {
        class Point
        {
            public char Symbol = ' ';
            public ConsoleColor Color = ConsoleColor.Black;
        }

        class Canvas
        {
            private readonly Point[,] _canvas;
            
            public Canvas(int height, int width) {
                _canvas = new Point[height,width];
                for(var y=0; y<this.Height; y++)
                for (var x = 0; x < this.Width; x++)
                    _canvas[y, x] = new Point();
            }

            public Point this[int y, int x] {
                get {
                    if (y < 0 || y >= this.Height ||
                        x < 0 || x >= this.Width) return null;
                    return _canvas[y, x];
                }
                set {
                    if (y < 0 || y >= this.Height ||
                        x < 0 || x >= this.Width) return;
                    _canvas[y, x] = value;
                }
            }

            public int Width => _canvas.GetLength(1);
            public int Height => _canvas.GetLength(0);
        }
        
        public static void Render(Widget root)
        {
            var canvas = new Canvas(Console.WindowHeight, Console.WindowWidth);
            Render(canvas, root);
            
            // draw canvas
            var t = Console.BackgroundColor;
            for (var y = 0; y < canvas.Height; y++)
            for (var x = 0; x < canvas.Width; x++) {
                var p = canvas[y, x];
                if (p == null) continue;
                Console.BackgroundColor = p.Color;
                if (y < Console.WindowHeight && x < Console.WindowWidth)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(p.Symbol);
                }
            }
            Console.BackgroundColor = t;
        }

        static void Render(Canvas canvas, Widget widget)
        {
            for(var row=0; row < widget.H; row++) {
                var y = widget.Y + row;
                for (var col = 0; col < widget.W; col++) {
                    var x = widget.X + col;
                    canvas[y, x] = new Point {
                        Symbol = widget.Letter,
                        Color = widget.Color
                    };
                }
            }

            if (widget.HasFocus) {
                var p = canvas[widget.Y, widget.X];
                if (p != null) p.Symbol = '+';
                p = canvas[widget.Y, widget.X + widget.W - 1];
                if (p != null) p.Symbol = '+';
                p = canvas[widget.Y + widget.H - 1, widget.X];
                if (p != null) p.Symbol = '+';
                p = canvas[widget.Y + widget.H - 1, widget.X + widget.W - 1];
                if (p != null) p.Symbol = '+';
            }
            
            foreach(var child in widget.Children)
                Render(canvas, child);
        }
    }
    */
}