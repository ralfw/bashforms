using System;
using bashforms.data;

namespace bashforms.engine
{
    class Display
    {
        public static void Show(Canvas canvas, (int x, int y) cursorPosition) {
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

            if (cursorPosition.x >= 0) {
                Console.CursorVisible = true;
                Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            }
        }
    }
}