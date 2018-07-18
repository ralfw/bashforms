using bashforms.data;

namespace bashforms.widgets.controls
{
    partial class TextLine
    {
        public override Canvas Draw() {
            var showLabel = _text.Length == 0 && _label.Length > 0;
        
            var canvas = new Canvas(_width, _height, 
                _backgroundColor, 
                showLabel ? _labelForegroundColor : _foregroundColor);

            //??? darstellung bei alloscrolling abhängig von insertion point
            var text = _text.PadRight(_width, '_');
            if (showLabel) text = _label.PadRight(_width, '_');
            canvas.Write(0,0,text);
        
            if (this.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = _focusBackgroundColor;
                    p.ForegroundColor = showLabel ? _labelForegroundColor : _focusForegroundColor;
                }

            return canvas;
        }
    }
}