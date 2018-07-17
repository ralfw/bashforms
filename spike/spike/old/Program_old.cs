namespace spike.old
{
    /*
    internal class Program_old
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
    */
}