using System;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms_tests
{
    public class Program
    {
        public static void Main(string[] args) {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "ToDo"};
            frm.AddChild(new TextLine(2,2,40){Label = "title", Name = "txtTitle"});
            frm.AddChild(new TextArea(2,4,40,10) {Label = "description", Name = "txtDescription"});
            frm.AddChild(new TextLine(2,15,8){Label = "due date", Name = "txtDueDate"});
            frm.AddChild(new Button(2,17,10,"Save") { OnPressed = (w, e) =>
            {
                MessageBox.ShowInfo($"Saving: {((TextLine)frm["txtTitle"]).Text}");
            }});
            frm.AddChild(new Button(14,17,10, "Cancel"){OnPressed = (w, e) =>
            {
                if (MessageBox.ShowQuestion($"Depth: {BashForms.Current.Depth} - Close?"))
                    BashForms.Close();
            }});
            
            BashForms.Open(frm);
        }
    }
}