using System.Linq;
using bashforms.data;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.controls
{
    partial class TextArea
    {
        public override Canvas Draw() {
            var showLabel = _text.Text.Length == 0 && _label.Length > 0;

            var lines = showLabel ? new[] {_label} : _text.SoftLines;
            
            var canvas = Initialize_canvas();
            for(var i=0; i<lines.Length; i++)
                canvas.Write(0,i, lines[i]);
            return canvas;


            Canvas Initialize_canvas() {
                var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
                var fgColor = showLabel ? _labelForegroundColor
                    : (this.HasFocus ? _focusForegroundColor : _foregroundColor);
                return new Canvas(_width, _height, bgColor, fgColor);
            }
        }
    }
}