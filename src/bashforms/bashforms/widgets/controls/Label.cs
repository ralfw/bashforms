using System;
using bashforms.data;
using bashforms.widgets.controls.formatting;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class Label : Control
    {
        private string _text;

        public Label(int left, int top, string text) : this(left, top, text.Length) {
            _text = text;
            CanBeMultiLine = false;
        }
        public Label(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
            CanBeMultiLine = false;
        }

        public string Text {
            get => _text;
            set {
                _text = value;
                OnUpdated(this, new EventArgs());
            }
        }
        
        
        public bool CanBeMultiLine { get; set; }
        

        public override bool HandleKey(ConsoleKeyInfo key) { return false; }
        
        
        public override Canvas Draw() {
            var wrappedText = CanBeMultiLine ? _text.Wrap(_width) : new[] {_text};
            var canvas = new Canvas(_width, wrappedText.Length, _backgroundColor, _foregroundColor);

            for(var i=0; i<wrappedText.Length; i++)
                canvas.Write(0,i,wrappedText[i]);
            
            return canvas;
        }
    }
}