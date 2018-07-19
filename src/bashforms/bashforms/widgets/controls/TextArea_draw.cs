using System.Linq;
using bashforms.data;
using bashforms.widgets.controls.utils;

namespace bashforms.widgets.controls
{
    partial class TextArea
    {
        public override Canvas Draw() {
            var showLabel = _text.Length == 0 && _label.Length > 0;

            var lines = Project_text();
            
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

            string[] Project_text() {
                if (showLabel)
                    return new[] {_label};

                var paragraphs = _text.ToParagraphs().Select(p => p.Wrap(_width));
                var text = string.Join("\n\n", paragraphs);
                return text.Split('\n');
            }
        }
    }
}