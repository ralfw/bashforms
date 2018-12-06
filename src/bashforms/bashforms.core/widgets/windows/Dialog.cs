namespace bashforms.widgets.windows
{
    public class Dialog<TResult> : Form
    {
        public Dialog(int left, int top, int width, int height) : base(left, top, width, height) {
            this.Result = default(TResult);
        }
        
        public TResult Result { get; set; }
    }
}