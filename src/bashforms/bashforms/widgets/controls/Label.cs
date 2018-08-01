using System;
using bashforms.data;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class Label : Control
    {
        private string _text;

        public Label(int left, int top, string text) : this(left, top, text.Length) {
            _text = text;
        }
        public Label(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
        }

        public string Text {
            get => _text;
            set {
                _text = value;
                OnUpdated(this, new EventArgs());
            }
        }

        public override bool HandleKey(ConsoleKeyInfo key) { return false; }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            
            canvas.Write(0,0,_text);

            return canvas;
        }
    }
}