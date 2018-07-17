using bashforms.data;

namespace bashforms.engine.drawing
{
    class Label : IDrawing
    {
        public Canvas Draw(object obj)
        {
            var label = (widgets.controls.Label) obj;
            var canvas = new Canvas(label.Size.width, label.Size.height, label.BackgroundColor, label.ForegroundColor);
            
            canvas.Write(0,0,label.Text);

            return canvas;
        }
    }
}