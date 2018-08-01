using System;
using System.Collections.Generic;
using System.Linq;
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


        protected readonly List<Item> _items;

        
        public Listbox(int left, int top, int width, int height, IEnumerable<string> itemTexts) : this(left, top, width, height) {
            var items = itemTexts.Select(t => new Item(t));
            this.AddRange(items);
        }
        public Listbox(int left, int top, int width, int height) : base(left, top, width, height) {
            _items = new List<Item>();
            
            _focusBackgroundColor = ConsoleColor.DarkMagenta;
            _focusForegroundColor = ConsoleColor.White;
        }


        public Item[] Items => _items.ToArray();


        public void Clear() {
            _items.Clear();
            this.OnUpdated(this, new EventArgs());
        }

        public Item Add(string itemText) {
            var item = new Item(itemText);
            _items.Add(item);
            this.OnUpdated(this, new EventArgs());
            return item;
        }

        public void Add(Item item) {
            if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
            _items.Add(item);
            this.OnUpdated(this, new EventArgs());
        }

        public void AddRange(IEnumerable<Item> items) {
            foreach (var item in items) {
                if (_items.Contains(item)) throw new InvalidOperationException("All items in Listbox must be unique!");
                _items.Add(item);
            }
            this.OnUpdated(this, new EventArgs());
        }

        public void RemoveAt(int index) {
            _items.RemoveAt(index);
            this.OnUpdated(this, new EventArgs());
        }
        
        
        public override bool HandleKey(ConsoleKeyInfo key) {
            return false;
        }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            
            for(var i=0; i<Math.Min(_height,_items.Count-1); i++)
                canvas.Write(0,i,_items[i].Text);

            if (this.HasFocus)
                canvas.Colorize(_focusBackgroundColor, _focusForegroundColor);

            return canvas;
        }
    }
}