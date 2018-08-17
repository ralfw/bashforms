using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.data;
using bashforms.data.eventargs;
using bashforms.widgets.controls.baseclasses;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class Combobox : CursorControl
    {
        private readonly TextLine _textline;
        private readonly Listbox _listbox;
        private bool _limitTextToListItems;
        
        
        public Combobox(int left, int top, int width, int height, IEnumerable<string> itemTexts = null) : base(left, top, width, height) {
            _textline = new TextLine(0,0,width-2);
            _textline.OnUpdated += (s, e) => this.OnUpdated(this, new EventArgs());
            
            _listbox = new Listbox(0,1,width,height-1, itemTexts);
            _listbox.OnUpdated += (s, e) => this.OnUpdated(this, new EventArgs());
            _listbox.OnPressed += (s, e) => {
                _textline.Text = _listbox.Items[_listbox.CurrentItemIndex].Text;
                _textline.HasFocus = true;
                _listbox.HasFocus = false;
            };
            _limitTextToListItems = false;
        }

        
        public override bool HasFocus {
            get => base.HasFocus;
            set {
                _textline.HasFocus = value;
                _listbox.HasFocus = false;

                base.HasFocus = value;
            }
        }

        
        public string Text {
            get => _textline.Text;
            set => _textline.Text = value;
        }
        public int MaxTextLength {
            get => _textline.MaxTextLength;
            set => _textline.MaxTextLength = value;
        }
        public string Label {
            get => _textline.Label;
            set => _textline.Label = value;
        }
        public ConsoleColor LabelForegroundColor {
            get => _textline.LabelForegroundColor;
            set => _textline.LabelForegroundColor = value;
        }

        
        public void Clear() => _listbox.Clear();
        public Listbox.Item Add(string itemText) => _listbox.Add(itemText);
        public void Add(Listbox.Item item) => _listbox.Add(item);
        public void AddRange(IEnumerable<Listbox.Item> items) => _listbox.AddRange(items);
        public void RemoveAt(int index) => _listbox.RemoveAt(index);


        public bool LimitTextToItems
        {
            get => _limitTextToListItems;
            set {
                _limitTextToListItems = value;
                // clear text field if it does not match any list item
                if (_limitTextToListItems && _listbox.Items.All(item => item.Text != _textline.Text)) {
                    _textline.Text = _listbox.Items[0].Text;
                    this.OnUpdated(this, new EventArgs());
                }
            }
        }
        
        
        public override bool HandleKey(ConsoleKeyInfo key) {
            if (_listbox.HasFocus) {
                if (key.Key == ConsoleKey.Escape) {
                    _textline.HasFocus = true;
                    _listbox.HasFocus = false;
                    return true;
                }
                return _listbox.HandleKey(key);
            }
            
            if (key.Key == ConsoleKey.DownArrow) {
                _textline.HasFocus = false;
                _listbox.HasFocus = true;
                return true;
            }

            if (_limitTextToListItems) return true; // no direct input allowed into text field
            return _textline.HandleKey(key);
        }

        
        public override Canvas Draw() {
            var bgColor = this.HasFocus ? _focusBackgroundColor : _backgroundColor;
            var fgColor = this.HasFocus ? _focusForegroundColor : _foregroundColor;
            
            var canvas = new Canvas(_width, _listbox.HasFocus ? _height : 1, bgColor, fgColor);
            
            var txtCanvas = _textline.Draw();
            canvas.Write(0,0,"<");
            canvas.Merge(1,0,txtCanvas);
            canvas.Write(txtCanvas.Width+1,0,">");

            if (_listbox.HasFocus) {
                var lbCanvas = _listbox.Draw();
                canvas.Merge(0, 1, lbCanvas);
            }

            return canvas;
        }


        public override (int x, int y) CursorPosition => (_textline.CursorPosition.x+1,_textline.CursorPosition.y);
    }
}