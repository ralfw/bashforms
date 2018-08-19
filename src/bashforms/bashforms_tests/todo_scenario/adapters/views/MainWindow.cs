using System;
using bashforms;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;
using bashforms.widgets.windows.baseclasses;

namespace bashforms_tests.todo_scenario.adapters.views
{
    class MainWindow
    {
        private readonly Form _frm;
        
        public MainWindow()
        {
            _frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "ToDoBeDoBeDo Task Management"};
            
            _frm.MenuBar.Menu.AddItem(new MenuItem("File")).Submenu.AddItem(new MenuItem("Close", "mnuClose"){Shortcut = 'x'});
            
            _frm.MenuBar.Menu.AddItem(new MenuItem("Edit")).Submenu.AddItems(new[] {
                new MenuItem("Add", "mnuAdd") { Shortcut = '+'},
                new MenuItem("Delete", "mnuDel"){Shortcut = '-'}
            });
            _frm.MenuBar.Menu.AddItem(new MenuItem("View")).Submenu.AddItems(new[] {
                new MenuItem("Only overdue", "mnuOnlyOverdue"),
                new MenuItem("Only due", "mnuOnlyDue"),
                new MenuItem("Only ASAP", "mnuOnlyASAP"), 
            });
            
            _frm.MenuBar.OnSelected += (item, e) => {
                switch (item.Name) {
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
           
            _frm.AddChild(new Label(2, 2,_frm.Size.width - 4){
                Text = "Subject".PadRight(20) + "|" + "Description".PadRight(30) + "|" + "Due".PadRight(10) + "|" + "Priority".PadRight(10),
                BackgroundColor = ConsoleColor.DarkYellow,
                ForegroundColor = ConsoleColor.Black
            });

            var listBox = new Listbox(2, 3, _frm.Size.width - 4, _frm.Size.height - 2) {
                FocusBackgroundColor = ConsoleColor.Black,
                Columns = new[] {20, 30, 10, 10}
            };
            listBox.Add("some subject\tsome description\n12.05.2018\nASAP");

            listBox.OnPressed = (w, e) => {
                MessageBox.Show($"Selected item: {listBox.Items[listBox.CurrentItemIndex].Attachment}");
            };
            _frm.AddChild(listBox);
        }


        public void Show() => BashForms.Open(_frm);
    }
}