using System;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms.engine
{
    partial class Rendering
    {
        private static Canvas RenderWindow(object obj) {
            var win = (Window)obj;
            var canvas = new Canvas(win.Size.width, win.Size.height, win.BackgroundColor, win.ForegroundColor);
            return canvas;
        }
        
        private static Canvas RenderForm(object obj) {
            var form = (Form) obj;
            var canvas = RenderWindow(obj);
            RenderFrame();
            RenderTitle();
            return canvas;


            void RenderFrame() {
                for (var y = 0; y < canvas.Height; y++) {
                    canvas[0, y].Symbol = '|';
                    canvas[canvas.Width - 1, y].Symbol = '|';
                }

                for (var x = 0; x < canvas.Width; x++){
                    canvas[x, 0].Symbol = '–';
                    canvas[x, canvas.Height - 1].Symbol = '-';
                }
            
                canvas[0, 0].Symbol = '+';
                canvas[canvas.Width-1, 0].Symbol = '+';
                canvas[0, canvas.Height-1].Symbol = '+';
                canvas[canvas.Width-1, canvas.Height-1].Symbol = '+';
            }

            void RenderTitle() {
                var title = form.Title.Substring(0, Math.Min(form.Title.Length, canvas.Width - 4));
                title = $"[{title}]";
                var xTitel = (canvas.Width - title.Length) / 2;
                canvas.Write(xTitel,0, title);
            }
        }

        private static Canvas RenderLabel(object obj) {
            var label = (Label) obj;
            var canvas = new Canvas(label.Size.width, label.Size.height, label.BackgroundColor, label.ForegroundColor);
            
            canvas.Write(0,0,label.Text);

            return canvas;
        }
        
        private static Canvas RenderTextLine(object obj) {
            var textLine = (TextLine) obj;
            var canvas = new Canvas(textLine.Size.width, textLine.Size.height, textLine.BackgroundColor, textLine.ForegroundColor);

            var text = textLine.Text.PadRight(textLine.Size.width, '_');
            canvas.Write(0,0,text);
            
            if (textLine.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = textLine.FocusBackgroundColor;
                    p.ForegroundColor = textLine.FocusForegroundColor;
                }

            return canvas;
        }
    }
}