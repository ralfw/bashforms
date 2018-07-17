using bashforms.data;

namespace bashforms.engine.drawing
{
    class TextLine : IDrawing
    {
        public Canvas Draw(object obj) {
            var textLine = (widgets.controls.TextLine) obj;
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