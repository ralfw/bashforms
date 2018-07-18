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
        }
        
        
        private readonly Point[,] _canvas;
            
        public Canvas() : this(Console.WindowWidth, Console.WindowHeight, Console.BackgroundColor, Console.ForegroundColor) {}
        public Canvas(int width, int height, ConsoleColor backgroundCoor, ConsoleColor foregroundColor) {
            _canvas = new Point[height,width];
            for(var y=0; y<this.Height; y++)
            for (var x = 0; x < this.Width; x++)
                _canvas[y, x] = new Point {BackgroundColor = backgroundCoor, ForegroundColor = foregroundColor};
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
                if (left+i < this.Width)
                    this[left+i, top].Symbol = text[i];
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