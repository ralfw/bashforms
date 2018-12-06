using System;
using System.Collections.Generic;

namespace bashforms.data
{
    public class Canvas
    {
        public class Point {
            public char Symbol = ' ';
            public ConsoleColor BackgroundColor = Console.BackgroundColor;
            public ConsoleColor ForegroundColor = Console.ForegroundColor;

            public override bool Equals(object obj) => Equals(obj as Point);
            public bool Equals(Point that) => that != null &&
                                              this.Symbol == that.Symbol && 
                                              this.BackgroundColor == that.BackgroundColor &&
                                              this.ForegroundColor == that.ForegroundColor;
            
            public static bool operator == (Point a, Point b) => (object)a == (object)b || ((object)a != null && a.Equals(b));
            public static bool operator !=(Point a, Point b) => !(a == b);

            public override int GetHashCode() => base.GetHashCode();
        }
        
        
        private readonly Point[,] _canvas;
            
        public Canvas() : this(Console.WindowWidth, Console.WindowHeight, Console.BackgroundColor, Console.ForegroundColor) {}
        public Canvas(int width, int height, ConsoleColor backgroundColor, ConsoleColor foregroundColor) {
            _canvas = new Point[height,width];
            for(var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                _canvas[y, x] = new Point();
            Colorize(backgroundColor,foregroundColor);
        }

        
        public Point this[int x, int y] {
            get
            {
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


        public IEnumerable<Point> Points => PointsInArea(0, 0, this.Width, this.Height);
        public IEnumerable<Point> PointsInArea(int left, int top, int width, int height) {
            for(var x = left; x < width; x++)
            for (var y = top; y < height; y++)
                yield return this[x, y];
        }


        public void Write(int left, int top, string text) {
            for (var i = 0; i < text.Length; i++)
                if (left + i < this.Width) {
                    var p = this[left + i, top];
                    if (p != null) p.Symbol = text[i];
                }
        }

        public void Colorize(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
            => Colorize(0, 0, this.Width, this.Height, backgroundColor, foregroundColor);
        public void Colorize(int left, int top, int width, int height, ConsoleColor backgroundColor, ConsoleColor foregroundColor) {
            for(var x = left; x < left+width; x++)
            for (var y = top; y < top+height; y++) {
                var p = this[x, y];
                p.BackgroundColor = backgroundColor;
                p.ForegroundColor = foregroundColor;
            }
        }
        
        
        public void Merge(int left, int top, Canvas source) {
            for(var x = 0; x < source.Width; x++)
            for(var y = 0; y < source.Height; y++)
                this[left + x, top + y] = source[x, y];
        }

        
        public int Width => _canvas.GetLength(1);
        public int Height => _canvas.GetLength(0);
    }
}