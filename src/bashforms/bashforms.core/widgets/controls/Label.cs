using System;
using bashforms.data;
using bashforms.widgets.controls.baseclasses;
using bashforms.widgets.controls.formatting;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class Label : Control
    {
        private string[] _lines;
        private bool _canBeMultiline;

        public Label(int left, int top, string text) : this(left, top, text.Length) {
            _lines = new[]{text};
            _canBeMultiline = false;
        }
        public Label(int left, int top, int width, string text = "") : base(left, top, width, 1) {
            _lines = new[] {text};
            CanBeMultiline = true;
        }

        public string Text {
            get => string.Join("\n", _lines);
            set {
                _lines = _canBeMultiline ? value.Wrap(_width, true) : new[] {value};
                _height = _lines.Length;
                OnUpdated(this, new EventArgs());
            }
        }


        public bool CanBeMultiline {
            get => _canBeMultiline;
            set {
                if (_canBeMultiline == value) return;

                _canBeMultiline = value;
                if (_canBeMultiline && _lines.Length == 1)
                    _lines = _lines[0].Wrap(_width, true);
                else if (!_canBeMultiline && _lines.Length > 1)
                    _lines = new[] {this.Text};
                _height = _lines.Length;
                
                OnUpdated(this, new EventArgs());
            }

        }
        

        public override bool HandleKey(ConsoleKeyInfo key) { return false; }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);

            for(var i=0; i<_lines.Length; i++)
                canvas.Write(0,i,_lines[i]);
            
            return canvas;
        }
    }
}