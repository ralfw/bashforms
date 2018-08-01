using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using bashforms.data;
using bashforms.data.eventargs;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public class Listbox : FocusControl
    {
        public class Item {
            public Item(string text) {
                Text = text;
            }
            
            public string Text { get; }
            
            public object Attachment { get; set; }
        }

        public enum SelectionModes {
            NoSelections,
            SingleSelection,
            MultipleSelections
        }


        protected List<Item> _items;
        protected List<int> _selectedItemIndexes;
        protected int _firstItemToDisplayIndex;
        protected int _currentItemIndex;
        protected SelectionModes _selectionMode;
        
        
        public Action<Widget, EventArgs> OnPressed = (w, a) => { };

        
        public Listbox(int left, int top, int width, int height, IEnumerable<string> itemTexts) : this(left, top, width, height) {
            var items = itemTexts.Select(t => new Item(t));
            this.AddRange(items);
            if (_items.Count > 0) _currentItemIndex = 0;
        }
        public Listbox(int left, int top, int width, int height) : base(left, top, width, height) {
            Clear();
            _focusBackgroundColor = ConsoleColor.DarkMagenta;
            _focusForegroundColor = ConsoleColor.White;
        }


        public Item[] Items => _items.ToArray();


        public void Clear() {
            _items = new List<Item>();
            _selectedItemIndexes = new List<int>();

            _firstItemToDisplayIndex = 0;
            _currentItemIndex = -1;
            
            this.OnUpdated(this, new EventArgs());
        }

        public Item Add(string itemText) {
            var item = new Item(itemText);
            _items.Add(item);
            if (_currentItemIndex < 0) _currentItemIndex = 0;
            this.OnUpdated(this, new EventArgs());
            return item;
        }

        public void Add(Item item) {
            if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
            _items.Add(item);
            if (_currentItemIndex < 0) _currentItemIndex = 0;
            this.OnUpdated(this, new EventArgs());
        }

        public void AddRange(IEnumerable<Item> items) {
            foreach (var item in items) {
                if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
                _items.Add(item);
            }
            if (_currentItemIndex < 0) _currentItemIndex = 0;
            this.OnUpdated(this, new EventArgs());
        }

        public void RemoveAt(int index) {
            _items.RemoveAt(index);
            _selectedItemIndexes.Remove(index);
            if (index <= _currentItemIndex) _currentItemIndex--;
            if (_currentItemIndex < 0 && _items.Count > 0) _currentItemIndex = 0;
            this.OnUpdated(this, new EventArgs());
        }


        public SelectionModes SelectionMode {
            get => _selectionMode;
            set {
                _selectionMode = value; 
                _selectedItemIndexes.Clear();
                this.OnUpdated(this, new EventArgs());
            }
        }
        
        public int[] SelectedItemIndexes => _selectedItemIndexes.ToArray();

        public void ClearSelections() {
            _selectedItemIndexes.Clear();
            this.OnUpdated(this, new EventArgs());
        }

        public void ClearSelection(int index) {
            _selectedItemIndexes.Remove(index);
            this.OnUpdated(this, new EventArgs());
        }


        public int CurrentItemIndex => _currentItemIndex;


        public override bool HandleKey(ConsoleKeyInfo key) {
            switch (key.Key)
            {
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    SelectCurrentItem();
                    this.OnPressed(this, new EventArgs());
                    return true;
                case ConsoleKey.UpArrow:
                    if (_items.Count == 0) return true;
                    
                    _currentItemIndex--;
                    if (_currentItemIndex < 0) _currentItemIndex = 0;

                    if (_currentItemIndex < _firstItemToDisplayIndex) _firstItemToDisplayIndex = _currentItemIndex;
                    return true;
                
                case ConsoleKey.DownArrow:
                    if (_items.Count == 0) return true;

                    _currentItemIndex++;
                    if (_currentItemIndex >= _items.Count) _currentItemIndex = _items.Count - 1;

                    if (_currentItemIndex > _firstItemToDisplayIndex + _height - 1) _firstItemToDisplayIndex++;
                    return true;
            }
            return false;
        }

        
        private void SelectCurrentItem() {
            if (_currentItemIndex < 0) return;

            if (_selectedItemIndexes.Contains(_currentItemIndex)) {
                _selectedItemIndexes.Remove(_currentItemIndex);
                return;
            }

            switch (_selectionMode) {
                case SelectionModes.SingleSelection:
                    _selectedItemIndexes.Clear();
                    _selectedItemIndexes.Add(_currentItemIndex);
                    break;
                case SelectionModes.MultipleSelections:
                    _selectedItemIndexes.Remove(_currentItemIndex); // avoid duplicates
                    _selectedItemIndexes.Add(_currentItemIndex);
                    break;
                case SelectionModes.NoSelections:
                    break;
            }
        }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);

            for (var row = 0; row < _height; row++) {
                var i = _firstItemToDisplayIndex + row;
                if (i >= _items.Count) break;
                
                canvas.Write(0,row, _items[i].Text);
                
                if (_selectedItemIndexes.Contains(i))
                    canvas.Colorize(0,row,_width,1, ConsoleColor.DarkYellow, ConsoleColor.White);
                
                if (this.HasFocus && i == _currentItemIndex)
                    canvas.Colorize(0,row,_width,1, ConsoleColor.Gray, ConsoleColor.Black);
            }
            
            return canvas;
        }
    }
}