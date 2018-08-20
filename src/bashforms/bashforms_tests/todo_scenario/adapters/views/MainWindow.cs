using System;
using System.Linq;
using bashforms;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;
using bashforms.widgets.windows.baseclasses;
using bashforms_tests.todo_scenario.data;

namespace bashforms_tests.todo_scenario.adapters.views
{
    class MainWindow
    {
        private readonly Form _frm;
        private readonly Listbox _lstTasks;
        private readonly TextLine _txtQuery;
        
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
                        OnNewTaskRequest();
                        break;
                    case "mnuDel":
                        if (_lstTasks.CurrentItemIndex >= 0 && MessageBox.AskForYes("Delete current item?"))
                            OnDeleteRequest((string)_lstTasks.CurrentItem.Attachment);
                        break;
                    case "mnuClose":
                        BashForms.Close();
                        break;
                }
            };

            
            const int SUBJECT_COL_WIDTH = 20;
            const int DESC_COL_WIDTH = 30;
            const int DUE_COL_WIDTH = 10;
            const int PRIO_COL_WIDTH = 13;
            
            _frm.AddChild(new Label(2, 2, "Subject".PadRight(SUBJECT_COL_WIDTH) + "|" + "Description".PadRight(DESC_COL_WIDTH) + "|" + "Due".PadRight(DUE_COL_WIDTH) + "|" + "Priority".PadRight(PRIO_COL_WIDTH)){
                BackgroundColor = ConsoleColor.DarkYellow,
                ForegroundColor = ConsoleColor.Black
            });

            _lstTasks = new Listbox(2, 3, _frm.Size.width - 4, _frm.Size.height - 6) {
                FocusBackgroundColor = ConsoleColor.Black,
                Columns = new[] {SUBJECT_COL_WIDTH, DESC_COL_WIDTH, DUE_COL_WIDTH, PRIO_COL_WIDTH}
            };

            _lstTasks.OnPressed = (w, e) => {
                MessageBox.Show($"Selected item: {_lstTasks.Items[_lstTasks.CurrentItemIndex].Attachment}");
            };
            _frm.AddChild(_lstTasks);
            
            _txtQuery = new TextLine(2, _lstTasks.Position.top + _lstTasks.Size.height + 1, 20){Label = "query"};
            _frm.AddChild(_txtQuery);
            
            _frm.AddChild(new Button(_txtQuery.Position.left + _txtQuery.Size.width + 2, _txtQuery.Position.top, 10, "Filter") { OnPressed = (s, e) => {
                OnQueryRequest(_txtQuery.Text);
            }});
        }


        public Action<string> OnQueryRequest;
        public Action OnNewTaskRequest;
        public Action<string> OnDeleteRequest;
        

        public void Show() => BashForms.Open(_frm);

        
        public void Display(Task[] tasks) {
            _lstTasks.Clear();
            foreach (var t in tasks) {
                _lstTasks.Add(Format_task_info(t))
                    .Attachment = t.Id;
            }
        }

        public void DisplayUpdate(Task task) {
            var taskItem = _lstTasks.Items.Select((item,index) => new{item,index})
                                          .FirstOrDefault(t => (string)t.item.Attachment == task.Id);
            if (taskItem == null)
                _lstTasks.Add(Format_task_info(task)).Attachment = task.Id;
            else {
                _lstTasks.RemoveAt(taskItem.index);
                _lstTasks.Insert(taskItem.index, new Listbox.Item(Format_task_info(task)){Attachment = task.Id});
            }
        }


        string Format_task_info(Task task) => $"{task.Subject}\t{task.Description}\t{task.DueAt.ToString("d")}\t{task.Priority.ToString()}";
    }
}