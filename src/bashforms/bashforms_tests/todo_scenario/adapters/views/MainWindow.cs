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
                        _frm.MenuBar.HasFocus = false;
                        break;
                    case "mnuDel":
                        if (_lstTasks.CurrentItemIndex >= 0 && MessageBox.AskForYes("Delete current item?"))
                            OnDeleteTaskRequest((string)_lstTasks.CurrentItem.Attachment);
                        _frm.MenuBar.HasFocus = false;
                        break;
                    case "mnuClose":
                        BashForms.Close();
                        _frm.MenuBar.HasFocus = false;
                        break;
                }
            };

            
            const int SUBJECT_COL_WIDTH = 20;
            const int DESC_COL_WIDTH = 30;
            const int DUE_COL_WIDTH = 10;
            const int PRIO_COL_WIDTH = 13;
            
            _frm.AddChild(new Label(2, 2, "Subject".PadRight(SUBJECT_COL_WIDTH) + "|" + "Description".PadRight(DESC_COL_WIDTH) + "|" + "Due".PadRight(DUE_COL_WIDTH) + "|" + "Priority".PadRight(PRIO_COL_WIDTH)){
                BackgroundColor = ConsoleColor.DarkBlue,
                ForegroundColor = ConsoleColor.White
            });

            _lstTasks = new Listbox(2, 3, _frm.Size.width - 4, _frm.Size.height - 6) {
                FocusBackgroundColor = ConsoleColor.Black,
                Columns = new[] {SUBJECT_COL_WIDTH, DESC_COL_WIDTH, DUE_COL_WIDTH, PRIO_COL_WIDTH}
            };

            _lstTasks.OnPressed = (w, e) => {
                if (_lstTasks.CurrentItemIndex >= 0)
                    OnEditTaskRequest((string)_lstTasks.CurrentItem.Attachment);
            };
            _frm.AddChild(_lstTasks);
            
            var txtQuery = new TextLine(2, _lstTasks.Position.top + _lstTasks.Size.height + 1, 45) {
                Label = "query: words or #tags separated by spaces"
            };
            _frm.AddChild(txtQuery);
            
            _frm.AddChild(new Button(txtQuery.Position.left + txtQuery.Size.width + 2, txtQuery.Position.top, 10, "Filter") { OnPressed = (s, e) => {
                OnQueryTasksRequest(txtQuery.Text);
            }});
        }


        public Action<string> OnQueryTasksRequest;
        public Action OnNewTaskRequest;
        public Action<string> OnEditTaskRequest;
        public Action<string> OnDeleteTaskRequest;
        

        public void Show() => BashForms.Open(_frm);

        
        public void Display(Task[] tasks) {
            var currentTaskId = (string)_lstTasks.CurrentItem?.Attachment;
            
            _lstTasks.Clear();
            foreach (var t in tasks) {
                var item = _lstTasks.Add(Format_task_info(t));
                Embelish_item(item, t);
            }

            if (_lstTasks.Items.Length == 0) return;
            if (currentTaskId == null) {
                _lstTasks.CurrentItemIndex = 0;
            }
            else {
                var taskEntry = Locate_task_entry(currentTaskId);
                if (taskEntry.item == null)
                    _lstTasks.CurrentItemIndex = 0;
                else
                    _lstTasks.CurrentItemIndex = taskEntry.index;
            }
        }

        public void DisplayUpdate(Task task) {
            var taskEntry = Locate_task_entry(task.Id);
            if (taskEntry.item == null) {
                taskEntry.item = _lstTasks.Add(Format_task_info(task));
                _lstTasks.CurrentItemIndex = _lstTasks.Items.Length - 1;
            }
            else {
                var currentItemIndex = _lstTasks.CurrentItemIndex;
                _lstTasks.RemoveAt(taskEntry.index);
                taskEntry.item = new Listbox.Item(Format_task_info(task));
                _lstTasks.Insert(taskEntry.index, taskEntry.item);
                _lstTasks.CurrentItemIndex = currentItemIndex;
            }
            Embelish_item(taskEntry.item, task);
        }


        void Embelish_item(Listbox.Item item, Task task) {
            item.Attachment = task.Id;
            
            if (task.Priority == TaskPriorities.ASAP)
                item.BackgroundColor = ConsoleColor.DarkYellow;
            if (DateTime.Now >= task.DueAt)
                item.ForegroundColor = ConsoleColor.Red;
        }

        
        (Listbox.Item item, int index) Locate_task_entry(string taskId) {
            var entry = _lstTasks.Items.Select((item, index) => new {item, index})
                                       .FirstOrDefault(t => (string) t.item.Attachment == taskId);
            return entry != null ? (entry.item, entry.index) : (null, -1);
        }

        
        static string Format_task_info(Task task) {
            var dueAt = task.DueAt.Year == DateTime.MaxValue.Year ? "" : task.DueAt.ToString("d");
            var prio = task.Priority == TaskPriorities.No ? "" : task.Priority.ToString();
            return $"{task.Subject}\t{task.Description}\t{dueAt}\t{prio}";
        }
    }
}