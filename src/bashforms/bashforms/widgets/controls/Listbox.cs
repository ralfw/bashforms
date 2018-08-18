using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using bashforms.data.eventargs;
using bashforms.widgets.controls.baseclasses;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public partial class Listbox : FocusControl
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
        protected int[] _columns;
        
        
        public Action<Listbox, EventArgs> OnPressed = (w, a) => { };

        
        public Listbox(int left, int top, int width, int height, IEnumerable<string> itemTexts) : this(left, top, width, height) {
            var items = itemTexts != null ? itemTexts.Select(t => new Item(t)) : new Item[0];
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
            return item;
        }
        public void Add(Item item) => this.Insert(_items.Count, item);
        public void AddRange(IEnumerable<Item> items) => this.InsertRange(_items.Count, items);

        public void Insert(int index, Item item) {
            if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
            _items.Insert(index, item);
            if (_currentItemIndex < 0) _currentItemIndex = 0;
            this.OnUpdated(this, new EventArgs());
        }

        public void InsertRange(int index, IEnumerable<Item> items) {
            foreach (var item in items) {
                if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
                _items.Insert(index++,item);
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


        public void AddSelection(int index) {
            if (index >= _items.Count) return;
            _selectedItemIndexes.Add(index);
            this.OnUpdated(this, new EventArgs());
        }

        
        public int CurrentItemIndex {
            get => _currentItemIndex;
            set {
                if (value >= _items.Count) return;
                _currentItemIndex = value;
                this.OnUpdated(this, new EventArgs());
            }
        }


        public int[] Columns {
            get => _columns;
            set {
                _columns = value;
                this.OnUpdated(this, new EventArgs());
            }
        }
        
    }
}