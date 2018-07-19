using System;
using bashforms.data;

namespace bashforms.widgets.controls
{
    public class Button : FocusControl
    {
        private string _text;

        public Action<Widget, EventArgs> OnPressed = (w, a) => { };
        
        
        public Button(int left, int top, int width, string text) : base(left, top, width, 1) {
            _text = text;
            _focusBackgroundColor = ConsoleColor.DarkMagenta;
            _focusForegroundColor = ConsoleColor.White;
        }
        
        
        public string Text {
            get => _text;
            set {
                _text = value;
                OnUpdated(this, new data.eventargs.EventArgs());
            }
        }

        
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar) {
                OnPressed(this, new EventArgs());
                return true;
            }
            return false;
        }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);

            var xLabel = (_width - _text.Length) / 2;
            canvas.Write(xLabel,0, _text);
            canvas.Write(0,0,"[");
            canvas.Write(_width-1,0,"]");
            
            if (this.HasFocus)
                foreach (var p in canvas.Points) {
                    p.BackgroundColor = _focusBackgroundColor;
                    p.ForegroundColor = _focusForegroundColor;
                }

            return canvas;
        }
    }
}