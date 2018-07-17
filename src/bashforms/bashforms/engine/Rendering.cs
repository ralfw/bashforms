using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.data;
using bashforms.engine.drawing;

namespace bashforms.engine
{
    class Rendering
    {
        private readonly Dictionary<Type, IDrawing> _renderers;


        public Rendering()
        {
            _renderers = new Dictionary<Type, IDrawing> {
                {typeof(widgets.windows.Window), new Window()},
                {typeof(widgets.windows.Form), new Form()},
                {typeof(widgets.controls.Label), new Label()},
                {typeof(widgets.controls.TextLine), new TextLine()},
                {typeof(widgets.controls.Button), new Button()}
            };
        }


        public void Render(widgets.windows.Window[] windows) {
            var canvas = windows.Aggregate(new Canvas(), Render);
            Display.Show(canvas, windows.Last().CursorPosition);
        }

        private Canvas Render(Canvas canvas, widgets.windows.Window window) {
            var winCanvas = _renderers[window.GetType()].Draw(window);
            canvas.Merge(0, 0, winCanvas);

            foreach (var widget in window.Children) {
                var widgetCanvas = _renderers[widget.GetType()].Draw(widget);
                canvas.Merge(widget.Position.left, widget.Position.top, widgetCanvas);
            }

            return canvas;
        }
    }
}