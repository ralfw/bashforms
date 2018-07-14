using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

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
                Color = ConsoleColor.Yellow
            });
            background.Children.Add(new Widget
            {
                X=40,Y=20,W=17,H=3,
                Letter = 'I',
                HasFocus = false,
                TabIndex = 0,
                Color = ConsoleColor.Green
            });

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
        public static void Render(Widget root)
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

            var c = Console.BackgroundColor;
            Console.BackgroundColor = root.Color;
            for (var y = 0; y < root.H; y++)
            {
                Console.SetCursorPosition(root.X, root.Y + y);
                Console.Write(area[y]);
            }
            Console.BackgroundColor = c;
            
            foreach(var child in root.Children)
                Render(child);
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