using System;
using System.Net.Mime;
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
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Choose a demo"};
            var mnusw = frm.MenuBar.Menu.AddItem("Single widgets");
            mnusw.Submenu.AddItem("Lbls", "mnuLabels");
            mnusw.Submenu.AddItem("Texts", "mnuText");
            mnusw.Submenu.AddItem("Options", "mnuOptions");
            mnusw.Submenu.AddItem("Lists", "mnuLists");
            mnusw.Submenu.AddItem("Messages", "mnuMsgBox");
            var mnusc = frm.MenuBar.Menu.AddItem("Scenarios");
            mnusc.Submenu.AddItem("ToDo", "mnuToDo");
            frm.MenuBar.Menu.AddItem("Exit", "mnuExit");

            frm.MenuBar.OnSelected += (mnuItem, e) => {
                switch (mnuItem.Name) {
                    case "mnuExit":
                        BashForms.Close();
                        break;
                    
                    case "mnuLabels":
                        Test_labels();
                        break;

                    case "mnuText":
                        Test_text_editing();
                        break;
                    
                    case "mnuOptions":
                        Test_options();
                        break;

                    case "mnuLists":
                        Test_lists();
                        break;
                    
                    case "mnuMsgBox":
                        Test_messageboxes();
                        break;
                    
                    case "mnuToDo":
                        Test_todo_scenario();
                        break;
                }
            };
            
            frm.AddChild(new Label(2,3,40) {
                Text = "* Press F2 to enter menu.\n* Use left/right arrow to move between menu item.\n* Use ENTER to select menu item/enter next menu level.\n* Use ESC to back up menu level.\n\n* Use (shift-)TAB to move between controls.",
                CanBeMultiline = true,
                ForegroundColor = ConsoleColor.DarkGray
            });
            
            BashForms.Open(frm);
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
        
        
        static void Test_text_editing() {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Text editing"};
            
            frm.AddChild(new TextLine(2,2,frm.Size.width - 4){Label = "Single text line"});
            
            frm.AddChild(new TextArea(2,4,16,5){Label = "Multi-line text"});
            
            frm.AddChild(new Button(2, 10, 10, "Close"){OnPressed = (s, e) => {
                BashForms.Close();
            }});
            
            BashForms.Open(frm);
        }
        
        
        static void Test_options()
        {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Options"};
            
            frm.AddChild(new Label(2,2,"Which animals are mammals?"));
            frm.AddChild(new Option(4, 3, 20, "Dog"){Name="optDog"});
            frm.AddChild(new Option(4, 4, 20, "Ant"){Name="optAnt"});
            frm.AddChild(new Option(4, 5, 20, "Cat"){Name="optCat"});
            frm.AddChild(new Option(4, 6, 20, "Dolphin"){Name="optDolphin"});
            frm.AddChild(new Button(4,7, 10, "Check"){OnPressed = (s, _) => {
                var correct = frm.Child<Option>("optDog").Selected && frm.Child<Option>("optCat").Selected &&
                              frm.Child<Option>("optDolphin").Selected &&
                              !frm.Child<Option>("optAnt").Selected;
                MessageBox.ShowInfo(correct ? "Correct! You're the best!" : "Sorry, wrong. Try again", " Quiz Result");
            }});
            
            frm.AddChild(new Label(2,9,"Who's your favorite hero?"));
            var grp = new SingleOptionGroup();
            grp.OnSelected += (s, _) => MessageBox.ShowInfo("Good choice: " + s.Text);
            frm.AddChild(new Option(4, 10, 20, "Superman"){OptionGroup = grp});
            frm.AddChild(new Option(4, 11, 20, "Donald Duck"){OptionGroup = grp});
            frm.AddChild(new Option(4, 12, 20, "Batman"){OptionGroup = grp});
            frm.AddChild(new Option(4, 13, 20, "Antman"){OptionGroup = grp});
            
            frm.AddChild(new Button(2, 14, 10, "Close"){OnPressed = (s, e) => {
                BashForms.Close();
            }});
            
            BashForms.Open(frm);
        }
        
        
        static void Test_lists()
        {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Options"};
            
            var lb = new Listbox(2,2,10,5,new[]{"Balin", "Dwalin", 
                                                "Kili", "Fili", 
                                                "Oin", "Gloin", 
                                                "Ori", "Nori", "Dori",
                                                "Bifur", "Bofur", "Bombur",
                                                "Thorin"
            });
            lb.OnPressed += (s, e) => MessageBox.ShowInfo("Dwarf selected: " + lb.Items[lb.CurrentItemIndex].Text);
            frm.AddChild(lb);
            
            var cb = new Combobox(2,8,21,5, new[]{"Paris", "London", "Oslo", "Berlin", "New York", "Tokyo", "Rio", "Prague"}) {
                Label = "Your favorite city"
            };
            frm.AddChild(cb);
            
            frm.AddChild(new Label(24,8,20) {
                Text = "Press down-arrow to open list of choices.\nPress ESC to close list of choices.",
                CanBeMultiline = true,
                ForegroundColor = ConsoleColor.DarkGray
            });
            
            frm.AddChild(new Button(2, 10, 10, "Close"){OnPressed = (s, e) => {
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