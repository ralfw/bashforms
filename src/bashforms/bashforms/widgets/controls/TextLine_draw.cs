using System;
using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class TextLine
    {
        public override Canvas Draw() {
            var showLabel = _text.Length == 0 && _label.Length > 0;

            var text = Project_text();
            
            var canvas = Initialize_canvas();
            canvas.Write(0,0,text);
            return canvas;


            Canvas Initialize_canvas() {
                var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
                var fgColor = showLabel ? _labelForegroundColor
                    : (this.HasFocus ? _focusForegroundColor : _foregroundColor);
                return new Canvas(_width, _height, bgColor, fgColor);
            }

            string Project_text() {
                var displayText = "";
                if (showLabel)
                    displayText = _label.PadRight(_width, '_');
                else {
                    displayText = _text.Substring(_displayFromIndex, Math.Min(_width, _text.Length - _displayFromIndex));
                    displayText = displayText.PadRight(_width, '_');
                }
                return displayText;
            }
        }
    }
}