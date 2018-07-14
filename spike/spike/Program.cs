using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace spike
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var background = new Widget {
                X=0,Y=0,W=Console.WindowWidth,H=Console.WindowHeight,
                Letter = '.',
                Color = ConsoleColor.Blue
            };
            
            background.Children.Add(new Widget
            {
                X=1,Y=1,W=10,H=5,
                Letter = 'X',
                TabIndex = 1,
                Color = ConsoleColor.Red
            });
            background.Children.Add(new Widget
            {
                X=7,Y=10,W=20,H=10,
                Letter = 'O',
                HasFocus = true,
                TabIndex = 2,
                Color = ConsoleColor.DarkMagenta
            });
            background.Children.Add(new Widget
            {
                X=40,Y=20,W=17,H=3,
                Letter = 'I',
                HasFocus = false,
                TabIndex = 0,
                Color = ConsoleColor.Green
            });

            var prevH = Console.WindowHeight;
            var prevW = Console.WindowWidth;
            var busy = false;
            var t = new System.Threading.Timer(
                _ =>
                {
                    if (busy) return;
                    if (prevH != Console.WindowHeight || prevW != Console.WindowWidth)
                    {
                        busy = true;
                        prevH = Console.WindowHeight;
                        prevW = Console.WindowWidth;
                        Renderer.Render(background);
                        busy = false;
                    }
                }, null, 100, 100);
            
            SetFocus();
            while (true)
            {
                Renderer.Render(background);
                var key = Console.ReadKey(true);
                // handle TAB
                if (key.Key == ConsoleKey.Tab)
                {
                    SetFocus();
                }
            }

            
            void SetFocus()
            {
                var focusWidget = background.TabOrder.First(w => w.HasFocus);
                focusWidget.HasFocus = false;
                var nextFocusWidget = background.Flattened.FirstOrDefault(w => w.TabIndex > focusWidget.TabIndex);
                if (nextFocusWidget == null) nextFocusWidget = background.TabOrder.First();
                nextFocusWidget.HasFocus = true;
            }
        }
    }

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
        
        
        
        public static void Render1(Widget root)
        {
            var area = new char[root.H][];
            for (var y = 0; y < root.H; y++)
                area[y] = "".PadLeft(root.W, root.Letter).ToCharArray();

            if (root.HasFocus)
            {
                area[0] = "".PadLeft(root.W, '-').ToCharArray();
                area[root.H - 1] = "".PadLeft(root.W, '-').ToCharArray();

                for (var y = 0; y < root.H; y++)
                {
                    area[y][0] = '|';
                    area[y][root.W - 1] = '|';
                }
                
                area[0][0] = '+';
                area[root.H - 1][0] = '+';
                area[0][root.W - 1] = '+';
                area[root.H - 1][root.W - 1] = '+';
            }

            
            var displayHeight = Math.Min(root.H, Console.WindowHeight);
            if (root.Y >= displayHeight)
            { 
                Console.WriteLine($"{root.Y}>={displayHeight}, <{root.Letter}>");
                return;
            }
            var viewportWidth = Math.Min(root.X + root.W - 1, Console.WindowWidth);
            if (root.X >= viewportWidth) return;

            var c = Console.BackgroundColor;
            Console.BackgroundColor = root.Color;
            for (var y = 0; y < displayHeight; y++) {
                try
                {
                    Console.SetCursorPosition(root.X, root.Y + y);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"({root.X},{root.Y+y} - {Console.WindowWidth}/{Console.WindowHeight}");
                    Environment.Exit(1);
                }

                Console.Write(new String(area[y]).Substring(0, viewportWidth));
            }
            Console.BackgroundColor = c;
            
            foreach(var child in root.Children)
                Render1(child);
        }
    }

    class Widget
    {
        public int X, Y, W, H;
        public char Letter;
        public ConsoleColor Color = ConsoleColor.Red;
        public bool HasFocus = false;
        public int TabIndex = -1;
        public bool CanHaveFocus => TabIndex >= 0;
        public List<Widget> Children = new List<Widget>();

        public Widget[] Flattened {
            get {
                var allNodes = new List<Widget>();
                DepthFirstFlatten(allNodes);
                return allNodes.ToArray();
            }
        }
        
        private void DepthFirstFlatten(List<Widget> nodes) {
            nodes.Add(this);
            foreach (var child in Children)
                child.DepthFirstFlatten(nodes);
        }
        
        public Widget[] TabOrder {
            get { return Flattened.Where(n => n.CanHaveFocus).OrderBy(n => n.TabIndex).ToArray(); }
        }
    }
}