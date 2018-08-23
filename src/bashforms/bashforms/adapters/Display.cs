using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using bashforms.data;

namespace bashforms.adapters
{
    internal class Display
    {
        public void Show(Canvas canvas, (int x, int y) cursorPosition) {
            canvas = Optimize(canvas);
            Print(canvas, cursorPosition);
        }


        /*
         * Optimization reduces the canvas to be drawn to the points which
         * differ from the previously shown canvas.
         * This can considerably lower the number of points to draw and thus
         * reduces slow I/O.
         */
        private Canvas _prevCanvas;

        Canvas Optimize(Canvas current) {
            Canvas diff;
            
            if (_prevCanvas == null ||
                _prevCanvas.Width != current.Width ||
                _prevCanvas.Height != current.Height)
                diff = current;
            else {
                diff = new Canvas(_prevCanvas.Width, _prevCanvas.Height, ConsoleColor.Black, ConsoleColor.White);

                for (var y = 0; y < _prevCanvas.Height; y++)
                for (var x = 0; x < _prevCanvas.Width; x++)
                    diff[x, y] = _prevCanvas[x, y] != current[x, y] ? current[x, y] : null;
            }

            _prevCanvas = diff;
            return diff;
        }


        static void Print(Canvas canvas, (int x, int y) cursorPosition) {
            var tBackground = Console.BackgroundColor;
            var tForeground = Console.ForegroundColor;
            Console.CursorVisible = false;

            var prevy = -99;
            var prevx = -99;
            for (var y = 0; y < canvas.Height; y++)
            for (var x = 0; x < canvas.Width; x++) {
                if (y >= Console.WindowHeight || x >= Console.WindowWidth) continue;
                var p = canvas[x,y]; if (p == null) continue;
                
                /*
                 * The cursor needs only be positioned if the symbol to print is not
                 * right after the previous symbol printed.
                 */
                if (y-prevy != 0 || x-prevx != 1) {
                    Console.SetCursorPosition(x, y);
                }
                prevy = y;
                prevx = x;

                Console.BackgroundColor = p.BackgroundColor;
                Console.ForegroundColor = p.ForegroundColor;
                Console.Write(p.Symbol);
            }

            if (cursorPosition.x >= 0) {
                Console.CursorVisible = true;
                Console.SetCursorPosition(cursorPosition.x, cursorPosition.y);
            }
            Console.BackgroundColor = tBackground;
            Console.ForegroundColor = tForeground;
        }
    }
}