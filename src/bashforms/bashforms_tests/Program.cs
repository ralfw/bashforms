using System;
using bashforms;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;
using NUnit.Framework.Internal;

namespace bashforms_tests
{
    public class Program
    {
        public static void Main(string[] args) {
            Choose_a_demo();
        }

        static void Choose_a_demo() {
            var frmDemoSelection = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Choose a demo"};
            var mnusw = frmDemoSelection.MenuBar.Menu.AddItem("Single widgets");
            mnusw.Submenu.AddItem("Labels", "mnuLabels");
            mnusw.Submenu.AddItem("Message boxes", "mnuMsgBox");
            var mnusc = frmDemoSelection.MenuBar.Menu.AddItem("Scenarios");
            mnusc.Submenu.AddItem("ToDo", "mnuToDo");
            frmDemoSelection.MenuBar.Menu.AddItem("Exit", "mnuExit");

            frmDemoSelection.MenuBar.OnSelected += (mnuItem, e) => {
                switch (mnuItem.Name) {
                    case "mnuExit":
                        BashForms.Close();
                        break;
                    
                    case "mnuLabels":
                        Test_labels();
                        break;
                    
                    case "mnuMsgBox":
                        Test_messageboxes();
                        break;
                    
                    case "mnuToDo":
                        Test_todo_scenario();
                        break;
                }
            };
            
            BashForms.Open(frmDemoSelection);
        }
        
        
        static void Test_messageboxes()
        {
            var frmExperiments = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Experiments"};
            frmExperiments.AddChild(new Label(2,2,"Messagebox result:"));
            frmExperiments.AddChild(new Label(2,3, 18){BackgroundColor = ConsoleColor.DarkRed, Name = "lblResult"});
            
            frmExperiments.MenuBar.Menu.AddItem("Ok");
            frmExperiments.MenuBar.Menu.AddItem("YesNo");
            frmExperiments.MenuBar.Menu.AddItem("OkCancelIgnore");
            frmExperiments.MenuBar.Menu.AddItem("None");
            frmExperiments.MenuBar.Menu.AddItem("Exit");

            frmExperiments.MenuBar.OnSelected += (mnuItem, e) =>
            {
                switch (mnuItem.Text)
                {
                    case "Ok":
                        var result = MessageBox.Show("Gimme an ok!", (MessageBox.Results.Ok, "Okay!"));
                        frmExperiments.Child<Label>("lblResult").Text = result.ToString();
                        break;
                    case "YesNo":
                        result = MessageBox.Show("Shall I?", 
                                                 (MessageBox.Results.Yes, "YES"),
                                                 (MessageBox.Results.No, "No,no!"), 
                                                 "Question");
                        frmExperiments.Child<Label>("lblResult").Text = result.ToString();
                        break;
                    case "OkCancelIgnore":
                        result = MessageBox.Show("Shall I?", 
                                                 (MessageBox.Results.Continue, "Continue"),
                                                 (MessageBox.Results.Cancel, "Cancel"), 
                                                 (MessageBox.Results.Ignore, "Never mind"), 
                                                 "What's your choice?");
                        frmExperiments.Child<Label>("lblResult").Text = result.ToString();
                        break;
                    case "None":
                        result = MessageBox.Show("I am confused", 
                                                (MessageBox.Results.None, "Continue"),
                                                (MessageBox.Results.None, "Cancel"), 
                                                (MessageBox.Results.None, "Never mind"), 
                                                "No choice?");
                        frmExperiments.Child<Label>("lblResult").Text = result.ToString();
                        break;
                    case "Exit":
                        BashForms.Close();
                        break;
                }
            };
            
            BashForms.Open(frmExperiments);
        }


        static void Test_labels()
        {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Labels"};
            
            frm.AddChild(new Label(2,2,"Macbeth:"));
            
            frm.AddChild(new Label(11,2,15) {
                Name = "lbl",
                CanBeMultiline = true, 
                Text = "She should have died hereafter, there would have been a time for such a word.",
                BackgroundColor = ConsoleColor.DarkRed
            });
            frm.AddChild(new Button(2, 9, 10, "Close"){OnPressed = (s, e) => {
                BashForms.Close();
            }});
            
            BashForms.Open(frm);
        }
        
        
        static void Test_todo_scenario()
        {
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
            frmOverview.MenuBar.Menu.AddItem(new MenuItem("Edit")).Submenu.AddItems(new[] {
                new MenuItem("Change", "mnuChange"){Shortcut = 'c'}, 
                new MenuItem("Add", "mnuAdd") { Shortcut = '+'},
                new MenuItem("Delete", "mnuDel"){Shortcut = '-'}
            });
            frmOverview.MenuBar.Menu.AddItem(new MenuItem("Close", "mnuClose"){Shortcut='x'});
            frmOverview.MenuBar.OnSelected += (item, e) => {
                switch (item.Name) {
                    case "mnuChange":
                        MessageBox.Show("Change item...");
                        break;
                    case "mnuAdd":
                        MessageBox.Show("Add new item...");
                        break;
                    case "mnuDel":
                        if (MessageBox.AskForYes("Delete current item?"))
                            MessageBox.Show("Item deleted!");
                        break;
                    case "mnuClose":
                        BashForms.Close();
                        break;
                }
            };
            
            var listBox = new Listbox(2, 3, frmOverview.Size.width - 4, frmOverview.Size.height - 5) {
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
            
            
            BashForms.Open(frmOverview);
        }
    }
}