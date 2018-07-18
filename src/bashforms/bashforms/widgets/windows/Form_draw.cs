using System;
using bashforms.data;

namespace bashforms.widgets.windows
{
    partial class Form
    {
        public override Canvas Draw() {
            var canvas = base.Draw();
            
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
                var title = _title.Substring(0, Math.Min(_title.Length, canvas.Width - 4));
                title = $"[{title}]";
                var xTitel = (canvas.Width - title.Length) / 2;
                canvas.Write(xTitel,0, title);
            }
        }
    }
}