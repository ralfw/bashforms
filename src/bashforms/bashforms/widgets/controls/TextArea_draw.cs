using System;
using System.Linq;
using bashforms.data;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.controls
{
    partial class TextArea
    {
        public override Canvas Draw() {
            var showLabel = _text.Text.Length == 0 && _label.Length > 0;
            var canvas = Initialize_canvas();
            
            if (showLabel)
                canvas.Write(0,0, _label);
            else {
                var n = Math.Min(_height, _text.SoftLines.Length - _displayFromSoftRow);
                for(var i=0; i<n; i++)
                    canvas.Write(0,i, _text.SoftLines[_displayFromSoftRow + i]);
            }
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