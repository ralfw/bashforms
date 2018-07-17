using bashforms.data;

namespace bashforms.engine.drawing
{
    class Button : IDrawing
    {
        public Canvas Draw(object obj)
        {
            var button = (widgets.controls.Button) obj;
            var canvas = new Canvas(button.Size.width, button.Size.height, button.BackgroundColor, button.ForegroundColor);

            var xLabel = (button.Size.width - button.Text.Length) / 2;
            canvas.Write(xLabel,0, button.Text);
            canvas.Write(0,0,"[");
            canvas.Write(button.Size.width-1,0,"]");
            
            if (button.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = button.FocusBackgroundColor;
                    p.ForegroundColor = button.FocusForegroundColor;
                }

            return canvas;
        }
    }
}