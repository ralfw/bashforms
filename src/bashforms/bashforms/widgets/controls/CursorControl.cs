namespace bashforms.widgets.controls
{
    public abstract class CursorControl : FocusControl
    {
        public CursorControl(int left, int top, int width, int height) : base(left, top, width, height) {}

        public abstract (int x, int y) CursorPosition { get; }
    }
}