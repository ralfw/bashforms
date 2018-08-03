using System;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms_tests
{
    public class Program
    {
        public static void Main(string[] args) {
            var frmEdit = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "ToDo Item"};
            frmEdit.AddChild(new TextLine(2,2,40){Label = "title", Name = "txtTitle"});
            frmEdit.AddChild(new TextArea(2,4,40,10) {Label = "description", Name = "txtDescription"});
            frmEdit.AddChild(new TextLine(2,15,8){Label = "due date", Name = "txtDueDate"});

            frmEdit.AddChild(new Label(44,2,"importance:"));
            frmEdit.AddChild(new Listbox(44,3,10,3, new[]{"Top!", "Very high", "High", "Moderate", "Low", "Very low"})
            {
                SelectionMode = Listbox.SelectionModes.SingleSelection
            });
            
            frmEdit.AddChild(new Button(2,17,10,"Save") { OnPressed = (w, e) =>
            {
                MessageBox.ShowInfo($"Saving: {frmEdit.Child<TextLine>("txtTitle").Text}");
            }});
            frmEdit.AddChild(new Button(14,17,10, "Cancel"){OnPressed = (w, e) =>
            {
                if (MessageBox.ShowQuestion($"Depth: {BashForms.Current.Depth} - Close?"))
                    BashForms.Close();
            }});
            
            
            var frmOverview = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "ToDo"};
            frmOverview.Menu = new TextLine(2,1,10){Label = "the menue"};
            var lbItems = new Listbox(2, 2, frmOverview.Size.width - 4, frmOverview.Size.height - 5) {
                FocusBackgroundColor = ConsoleColor.Black
            };
            lbItems.Columns = new[] {6, lbItems.Size.width-2};

            lbItems.Add("123\tsome description").Attachment = "12";
            lbItems.Add("abcdefxyz\tanother description which is somewhat longer to extend across the line").Attachment = "ab";
            lbItems.Add("hello, world!\tyet another description").Attachment = "hw";

            lbItems.OnPressed = (w, e) => {
                MessageBox.ShowInfo($"Selected item: {lbItems.Items[lbItems.CurrentItemIndex].Attachment}");
            };
            
            frmOverview.AddChild(lbItems);
            frmOverview.AddChild(new Button(2, frmOverview.Size.height-2, 7, "Add") {
                OnPressed = (w, e) => {
                    MessageBox.ShowInfo($"Selected item: {lbItems.CurrentItemIndex}");
                }
            });
            
            
            BashForms.Open(frmOverview);
        }
    }
}