using System;
using System.Diagnostics;
using System.IO;
using bashforms.data;

namespace bashforms.adapters
{
    internal class Display
    {
        public void Show(Canvas canvas, (int x, int y) cursorPosition)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var ww = 0;
            var pw = 0;
            
            Console.CursorVisible = false;
            
            var tBackground = Console.BackgroundColor;
            var tForeground = Console.ForegroundColor;

            var prevx = -99;
            var prevy = -99;
            for (var y = 0; y < canvas.Height; y++)
            for (var x = 0; x < canvas.Width; x++) {
                var p = canvas[x,y]; if (p == null) continue;
                
                Console.BackgroundColor = p.BackgroundColor;
                Console.ForegroundColor = p.ForegroundColor;
                
                if (y < Console.WindowHeight && x < Console.WindowWidth) {
                    if (y-prevy != 0 || x-prevx != 1) {
                        Console.SetCursorPosition(x, y);
                        pw++;
                    }
                    prevy = y;
                    prevx = x;

                    Console.Write(p.Symbol);
                    ww++;
                }
            }

            Console.BackgroundColor = tBackground;
            Console.ForegroundColor = tForeground;

            if (cursorPosition.x >= 0) {
                Console.CursorVisible = true;
                Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            }
            
            stopwatch.Stop();
            File.AppendAllLines("stopwatch.txt", new[]{$"Display.Show: {stopwatch.ElapsedMilliseconds} / {ww} / {pw}"});
        }
    }
}