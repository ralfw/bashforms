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
                MessageBox.Show($"Saving: {frmEdit.Child<TextLine>("txtTitle").Text}");
            }});
            frmEdit.AddChild(new Button(14,17,10, "Cancel"){OnPressed = (w, e) =>
            {
                if (MessageBox.Show($"Depth: {BashForms.Current.Depth} - Close?", 
                                    (MessageBox.Results.Yes, "YES"), (MessageBox.Results.No, "no")) == MessageBox.Results.Yes)
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
                MessageBox.Show($"Selected item: {listBox.Items[listBox.CurrentItemIndex].Attachment}");
            };
            
            frmOverview.AddChild(listBox);
            
            frmOverview.AddChild(new Button(2, frmOverview.Size.height-2, 7, "Add") {
                OnPressed = (w, e) => {
                    MessageBox.Show($"Selected item: {listBox.CurrentItemIndex}");
                }
            });
            
            
            var frmExperiments = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Experiments"};
            frmExperiments.AddChild(new Label(2,2,15)
            {
                Name = "lbl",
                CanBeMultiline = true, 
                Text = "She should have died hereafter, there would have been a time for such a word.",
                BackgroundColor = ConsoleColor.DarkBlue
            });
            frmExperiments.AddChild(new Button(17, 2, 10, "info..."){OnPressed = (s, e) =>
            {
                MessageBox.Show("She should have died\nhereafter, there would have been\na time for such a word.", "Info");
            }});
            
            
            frmExperiments.MenuBar.Menu.AddItem("Ok");
            frmExperiments.MenuBar.Menu.AddItem("YesNo");
            frmExperiments.MenuBar.Menu.AddItem("OkCancelIgnore");
            frmExperiments.MenuBar.Menu.AddItem("None");

            frmExperiments.MenuBar.OnSelected += (mnuItem, e) =>
            {
                switch (mnuItem.Text)
                {
                    case "Ok":
                        var result = MessageBox.Show("Gimme an ok!", (MessageBox.Results.Ok, "Okay!"));
                        frmExperiments.Child<Label>("lbl").Text = result.ToString();
                        break;
                    case "YesNo":
                        result = MessageBox.Show("Shall I?", 
                                                 (MessageBox.Results.Yes, "YES"),
                                                 (MessageBox.Results.No, "No,no!"), 
                                                 "Question");
                        frmExperiments.Child<Label>("lbl").Text = result.ToString();
                        break;
                    case "OkCancelIgnore":
                        result = MessageBox.Show("Shall I?", 
                                                 (MessageBox.Results.Continue, "Continue"),
                                                 (MessageBox.Results.Cancel, "Cancel"), 
                                                 (MessageBox.Results.Ignore, "Never mind"), 
                                                 "What's your choice?");
                        frmExperiments.Child<Label>("lbl").Text = result.ToString();
                        break;
                    case "None":
                        result = MessageBox.Show("I am confused", 
                                                (MessageBox.Results.None, "Continue"),
                                                (MessageBox.Results.None, "Cancel"), 
                                                (MessageBox.Results.None, "Never mind"), 
                                                "No choice?");
                        frmExperiments.Child<Label>("lbl").Text = result.ToString();
                        break;
                }
            };
            
            BashForms.Open(frmExperiments);
        }
    }
}