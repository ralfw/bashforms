using System;
using System.Collections.Generic;
using System.Linq;

namespace spike
{
    public class Window
    {
        private readonly int _left;
        private readonly int _top;
        private readonly int _width;
        private readonly int _height;
        private ConsoleColor _backgroundColor;
        private ConsoleColor _foregroundColor;
        
        private readonly List<Widget> _children;

        public Action<Window, EventArgs> OnChanged = (w, a) => { }; 

        
        public Window(int left, int top, int width, int height) {
            _left = left;
            _top = top;
            _width = width;
            _height = height;
            _children = new List<Widget>();
            _backgroundColor = Console.BackgroundColor;
            _foregroundColor = Console.ForegroundColor;
        }
        
        public (int left, int top) Position => (_left, _top);
        public (int width, int height) Size => (_width, _height);
        
        public string Name { get; set; }

        public ConsoleColor BackgroundColor {
            get => _backgroundColor;
            set { _backgroundColor = value; OnChanged(this,new EventArgs()); }
        }

        public ConsoleColor ForegroundColor { 
            get => _foregroundColor;
            set { _foregroundColor = value; OnChanged(this,new EventArgs()); }
        }

        
        public void AddChild(Widget child) {
            _children.Add(child);
            OnChanged(this, new EventArgs());
        }

        public Widget[] Children => _children.ToArray();

        public void InitializeFocus() {
            var focusCandidates = _children.OfType<FocusWidget>().ToList();
            focusCandidates.ForEach(c => c.HasFocus = false);
            
            var focusCandidate = focusCandidates.Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).FirstOrDefault();
            if (focusCandidate != null) focusCandidate.HasFocus = true;
        }

        
        public void HandleKey(ConsoleKeyInfo key) {
            Check_tab(
                Move_focus,
                Let_focus_handle_key);


            void Check_tab(Action<bool> onTab, Action onNotTab) {
                if (key.Key == ConsoleKey.Tab) {
                    var isBackTAB = (key.Modifiers & ConsoleModifiers.Shift) > 0;
                    onTab(!isBackTAB);
                }
                else
                    onNotTab();
            }

            void Let_focus_handle_key() {
                var focus = _children.OfType<FocusWidget>().FirstOrDefault(c => c.HasFocus);
                focus?.HandleKey(key);
            }
        }
        
        
        void Move_focus(bool moveForward)
        {
            var focusCandidates = _children.OfType<FocusWidget>().Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).ToArray();
            var focus = focusCandidates.FirstOrDefault(fc => fc.HasFocus);
            
            if (focus == null) { // no current focus -> move to first focus widget
                var focusCandidate = focusCandidates.FirstOrDefault();
                if (focusCandidate != null) focusCandidate.HasFocus = true;
            }
            else {
                focus.HasFocus = false;
                
                if (moveForward) {
                    var nextCandidate = focusCandidates.FirstOrDefault(fc => fc.TabIndex > focus.TabIndex);
                    if (nextCandidate == null) { // no next after current -> move focus to first focus widget
                        nextCandidate = focusCandidates.FirstOrDefault();
                        if (nextCandidate != null) nextCandidate.HasFocus = true;
                    }
                    else
                        nextCandidate.HasFocus = true;
                }
                else {
                    var prevCandidate = focusCandidates.LastOrDefault(fc => fc.TabIndex < focus.TabIndex);
                    if (prevCandidate == null) { // no previous before current -> move to last focus widget
                        prevCandidate = focusCandidates.LastOrDefault();
                        if (prevCandidate != null) prevCandidate.HasFocus = true;
                    }
                    else
                        prevCandidate.HasFocus = true;   
                }
            }
        }
    }
}