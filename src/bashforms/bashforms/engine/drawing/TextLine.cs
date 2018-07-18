using bashforms.data;

namespace bashforms.engine.drawing
{
    class TextLine : IDrawing
    {
        public Canvas Draw(object obj) {
            var textLine = (widgets.controls.TextLine) obj;

            var showLabel = textLine.Text.Length == 0 && textLine.Label.Length > 0;
            
            var canvas = new Canvas(textLine.Size.width, textLine.Size.height, 
                                    textLine.BackgroundColor, 
                                    showLabel ? textLine.LabelForegroundColor : textLine.ForegroundColor);

            var text = textLine.Text.PadRight(textLine.Size.width, '_');
            if (showLabel) text = textLine.Label.PadRight(textLine.Size.width, '_');
            canvas.Write(0,0,text);
            
            if (textLine.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = textLine.FocusBackgroundColor;
                    p.ForegroundColor = showLabel ? textLine.LabelForegroundColor : textLine.FocusForegroundColor;
                }

            return canvas;
        }
    }
}