using System;
using bashforms.data;

namespace bashforms.engine.drawing
{
    class Form : Window
    {
        public override Canvas Draw(object obj) {
            var canvas = base.Draw(obj);
            
            var form = (widgets.windows.Form) obj;
            RenderFrame();
            RenderTitle();
            return canvas;

            void RenderFrame() {
                for (var y = 0; y < canvas.Height; y++) {
                    canvas[0, y].Symbol = '│';
                    canvas[canvas.Width - 1, y].Symbol = '│';
                }

                for (var x = 0; x < canvas.Width; x++){
                    canvas[x, 0].Symbol = '─';
                    canvas[x, canvas.Height - 1].Symbol = '─';
                }
            
                canvas[0, 0].Symbol = '┌';
                canvas[canvas.Width-1, 0].Symbol = '┐';
                canvas[0, canvas.Height-1].Symbol = '└';
                canvas[canvas.Width-1, canvas.Height-1].Symbol = '┘';
            }

            void RenderTitle() {
                var title = form.Title.Substring(0, Math.Min(form.Title.Length, canvas.Width - 4));
                title = $"[{title}]";
                var xTitel = (canvas.Width - title.Length) / 2;
                canvas.Write(xTitel,0, title);
            }
        }
    }
}