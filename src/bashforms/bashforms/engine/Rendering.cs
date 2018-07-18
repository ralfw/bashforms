using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.adapters;
using bashforms.data;
using bashforms.engine.drawing;

namespace bashforms.engine
{
    class Rendering
    {
        private readonly Dictionary<string, IDrawing> _renderers;


        public Rendering()
        {
            _renderers = new Dictionary<string, IDrawing> {
                {typeof(widgets.windows.Window).Name, new Window()},
                {typeof(widgets.windows.Form).Name, new Form()},
                {"Dialog`1", new Form()},
                
                {typeof(widgets.controls.Label).Name, new Label()},
                {typeof(widgets.controls.TextLine).Name, new TextLine()},
                {typeof(widgets.controls.Button).Name, new Button()}
            };
        }


        public void Render(widgets.windows.Window[] windows) {
            var canvas = windows.Aggregate(new Canvas(), Render);
            Display.Show(canvas, windows.Last().CursorPosition);
        }

        private Canvas Render(Canvas canvas, widgets.windows.Window window) {
            var winCanvas = _renderers[window.GetType().Name].Draw(window);
            canvas.Merge(window.Position.left, window.Position.top, winCanvas);

            foreach (var widget in window.Children) {
                var widgetCanvas = _renderers[widget.GetType().Name].Draw(widget);
                canvas.Merge(window.Position.left + widget.Position.left, window.Position.top + widget.Position.top, widgetCanvas);
            }

            return canvas;
        }
    }
}