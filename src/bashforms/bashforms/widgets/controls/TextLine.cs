using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class TextLine : FocusControl
    {
        private string _text;
        
        public TextLine(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
        }

        public string Text {
            get => _text;
            set { 
                _text = value;
                OnChanged(this, new EventArgs());
            }
        }

        public override void HandleKey(ConsoleKeyInfo key) {
            if (char.IsLetterOrDigit(key.KeyChar)) this.Text += key.KeyChar;
        }
    }
}