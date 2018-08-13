using System;
using bashforms;
using bashforms.data;
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
            frmOverview.MenuBar.Menu.AddItem("File").Submenu.AddItems(new[]{
                new MenuItem("Open", "mnuOpen"){Shortcut = 'o'},
                new MenuItem("Save"), 
                new MenuItem("Close", "mnuClose")
            });
            frmOverview.MenuBar.Menu.Items[0].Submenu.Items[1].Submenu.AddItem("Save");
            frmOverview.MenuBar.Menu.Items[0].Submenu.Items[1].Submenu.AddItem("Save as");
            frmOverview.MenuBar.Menu.AddItem(new MenuItem("Edit")).Submenu.AddItems(new[] {
                new MenuItem("Cut", "mnuCut"),
                new MenuItem("Copy", "mnuCopy"),
                new MenuItem("Paste", "mnuPaste") 
            });
            frmOverview.MenuBar.Menu.AddItem(new MenuItem("Close application", "mnuClose"){Shortcut='x'});
            frmOverview.MenuBar.OnSelected += (item, e) =>
            {
                if (item.Name == "mnuCut") item.Checked = !item.Checked;
            };
            
            
            var listBox = new Listbox(2, 2, frmOverview.Size.width - 4, frmOverview.Size.height - 5) {
                FocusBackgroundColor = ConsoleColor.Black
            };
            listBox.Columns = new[] {6, listBox.Size.width-2};

            listBox.Add("123\tsome description").Attachment = "12";
            listBox.Add("abcdefxyz\tanother description which is somewhat longer to extend across the line").Attachment = "ab";
            listBox.Add("hello, world!\tyet another description").Attachment = "hw";

            listBox.OnPressed = (w, e) => {
                MessageBox.ShowInfo($"Selected item: {listBox.Items[listBox.CurrentItemIndex].Attachment}");
            };
            
            frmOverview.AddChild(listBox);
            
            frmOverview.AddChild(new Button(2, frmOverview.Size.height-2, 7, "Add") {
                OnPressed = (w, e) => {
                    MessageBox.ShowInfo($"Selected item: {listBox.CurrentItemIndex}");
                }
            });
            
            
            var frmExperiments = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Experiments"};
            frmExperiments.AddChild(new Label(2,2,15)
            {
                CanBeMultiline = true, 
                Text = "She should have died hereafter, there would have been a time for such a word.",
                BackgroundColor = ConsoleColor.DarkBlue
            });
            
            BashForms.Open(frmExperiments);
        }
    }
}