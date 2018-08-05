using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using bashforms.data;
using bashforms.widgets.controls;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows
{
    public class Window : Widget
    {
        protected readonly List<Control> _children;
        protected MenuBar _menu;
        
        public Action<Window, EventArgs> OnChanged = (w, a) => { }; 

        
        public Window(int left, int top, int width, int height) : base(left,top,width,height) {
            _children = new List<Control>();
            MenuKey = new ConsoleKeyInfo(' ', ConsoleKey.F2, false, false, false);
        }
        
        
        public void AddChild(Control child) {
            _children.Add(child);
            OnChanged(this, new EventArgs());
        }

        public Control[] Children => _children.ToArray();
        public T Child<T> (string name) where T : Control  => (T)this[name];
        
        public Control this[string name] => _children.FirstOrDefault(c => c.Name == name);

        
        public MenuBar MenuBar {
            get {
                if (_menu == null) _menu = new MenuBar(2,1,_width-4);                    
                return _menu;
            }
            set {
                _menu = value;
                this.OnChanged(this, new EventArgs());
            }
        }

        public ConsoleKeyInfo MenuKey { get; set; }

        
        public void InitializeFocus() {
            var focusCandidates = _children.OfType<FocusControl>().ToList();
            focusCandidates.ForEach(c => c.HasFocus = false);
            
            var focusCandidate = focusCandidates.Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).FirstOrDefault();
            if (focusCandidate != null) focusCandidate.HasFocus = true;
        }

        
        public override bool HandleKey(ConsoleKeyInfo key) {
            return Check_tab(
                Move_focus,
                () => Toggle_menu() || Let_focus_handle_key());


            bool Check_tab(Action<bool> onTab, Func<bool> onNotTab) {
                if (key.Key == ConsoleKey.Tab) {
                    var isBackTAB = (key.Modifiers & ConsoleModifiers.Shift) > 0;
                    onTab(!isBackTAB);
                    return true;
                }
                return onNotTab();
            }

            bool Toggle_menu() {
                if (key.Key == this.MenuKey.Key && key.Modifiers == this.MenuKey.Modifiers) {
                    if (_menu != null)
                        _menu.HasFocus = !_menu.HasFocus;
                    return true;
                }
                return false;
            }
            
            bool Let_focus_handle_key() {
                var focus = Find_focus();
                return focus != null && focus.HandleKey(key);
            }
        }
        
        
        void Move_focus(bool moveForward) {
            if (_menu?.HasFocus == true) return;
            
            var focusCandidates = _children.OfType<FocusControl>().Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).ToList();
            var focus = focusCandidates.FirstOrDefault(fc => fc.HasFocus);
            
            if (focus == null) {
                var focusCandidate = focusCandidates.FirstOrDefault();
                if (focusCandidate != null) focusCandidate.HasFocus = true;
            }
            else {
                focus.HasFocus = false;

                var focusIndex = focusCandidates.FindIndex(fc => fc == focus);
                if (moveForward)
                    focusIndex = (focusIndex + 1) % focusCandidates.Count;
                else
                    focusIndex = (focusIndex > 0) ? focusIndex - 1 : focusCandidates.Count - 1;

                focusCandidates[focusIndex].HasFocus = true;
            }
        }
        
        
        public (int x, int y) CursorPosition {
            get {
                var focus = Find_focus();
                if (!(focus is CursorControl)) return (-1, -1);
                return (focus.Position.left + ((CursorControl)focus).CursorPosition.x, 
                        focus.Position.top + ((CursorControl)focus).CursorPosition.y);
            }
        }
        
        
        public override Canvas Draw() {
            var canvas = new Canvas(_width, _height, _backgroundColor, _foregroundColor);
            var focus = Find_focus();
            
            foreach (var widget in this.Children) {
                if (widget != focus) Draw_widget(widget);
            }
            // draw focus last so it always comes out on top of all other widgets
            if (focus != null) Draw_widget(focus);
            // oh, no, draw menu last and on top of all others; maybe the real focus is there
            if (_menu != null) Draw_widget(_menu);
            
            return canvas;


            void Draw_widget(Widget w) {
                var widgetCanvas = w.Draw();
                canvas.Merge(w.Position.left, w.Position.top, widgetCanvas);
            }
        }


        FocusControl Find_focus() => _menu?.HasFocus == true 
                        ? _menu 
                        : _children.OfType<FocusControl>().FirstOrDefault(c => c.HasFocus);
    }
}