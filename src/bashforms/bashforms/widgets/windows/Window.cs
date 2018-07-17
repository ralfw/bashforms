using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.widgets.controls;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows
{
    public class Window : Widget
    {
        private readonly List<Control> _children;

        public Action<Window, EventArgs> OnChanged = (w, a) => { }; 

        
        public Window(int left, int top, int width, int height) : base(left,top,width,height) {
            _children = new List<Control>();
        }
        
        
        public void AddChild(Control child) {
            _children.Add(child);
            OnChanged(this, new EventArgs());
        }

        public Control[] Children => _children.ToArray();

        public void InitializeFocus() {
            var focusCandidates = _children.OfType<FocusControl>().ToList();
            focusCandidates.ForEach(c => c.HasFocus = false);
            
            var focusCandidate = focusCandidates.Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).FirstOrDefault();
            if (focusCandidate != null) focusCandidate.HasFocus = true;
        }

        
        public override void HandleKey(ConsoleKeyInfo key) {
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
                var focus = _children.OfType<FocusControl>().FirstOrDefault(c => c.HasFocus);
                focus?.HandleKey(key);
            }
        }
        
        
        void Move_focus(bool moveForward)
        {
            var focusCandidates = _children.OfType<FocusControl>().Where(c => c.CanHaveFocus).OrderBy(c => c.TabIndex).ToList();
            var focus = focusCandidates.FirstOrDefault(fc => fc.HasFocus);
            
            if (focus == null) { // no current focus -> move to first focus widget
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
                var focus = _children.OfType<CursorControl>().FirstOrDefault(fc => fc.HasFocus);
                if (focus == null) return (-1, -1);
                
                return (focus.Position.left + focus.CursorPosition.x, focus.Position.top + focus.CursorPosition.y);
            }
        }
    }
}