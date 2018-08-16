using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.data;

namespace bashforms.widgets.controls
{
    public class Option : FocusControl
    {
        private string _text;
        private bool _selected;
        private IOptionGroup _optionGroup;

        
        public Action<Widget, EventArgs> OnEdited = (w, a) => { };
        
        
        public Option(int left, int top, int width, string text) : base(left, top, width, 1) {
            _text = text;
            _selected = false;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
        }
        
        
        public string Text {
            get => _text;
            set {
                _text = value;
                OnUpdated(this, new data.eventargs.EventArgs());
            }
        }
        
        public bool Selected {
            get => _selected;
            set {
                _selected = value;
                OnUpdated(this, new data.eventargs.EventArgs());
            }
        }

        
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar) return false;
            
            if (_optionGroup != null)
                _optionGroup.ReportSelection(this);
            else
                _selected = !_selected;
            OnEdited(this,new EventArgs());
            return true;
        }


        public IOptionGroup OptionGroup {
            get => _optionGroup;

            set {
                _optionGroup?.Unregister(this);
                _optionGroup = value;
                _optionGroup?.Register(this);
            }
        }

        
        public override Canvas Draw() {
            var selectionChar = _optionGroup == null ? 'X' : '•';
            var (leftBracket, rightBracket) = _optionGroup == null ? ('[', ']') : ('(', ')');
            
            var value = _selected ? selectionChar : ' ';
            var text = $"{leftBracket}{value}{rightBracket} {_text}";
            
            var canvas = new Canvas(text.Length, _height, _backgroundColor, _foregroundColor);
            canvas.Write(0,0,text);
            
            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);

            return canvas;
        }
    }
}