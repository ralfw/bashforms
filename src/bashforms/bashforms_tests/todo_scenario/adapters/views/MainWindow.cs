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
        
        public MainWindow() {
            _frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "ToDoBeDoBeDo Task Management"};
            
            _frm.MenuBar.Menu.AddItem(new MenuItem("File")).Submenu.AddItem(new MenuItem("Close", "mnuClose"){Shortcut = 'x'});
            
            _frm.MenuBar.Menu.AddItem(new MenuItem("Edit")).Submenu.AddItems(new[] {
                new MenuItem("Add", "mnuAdd") { Shortcut = '+'},
                new MenuItem("Delete", "mnuDel"){Shortcut = '-'}
            });

            var mnuFilterOverdue = new MenuItem("Filter for overdue", "mnuFilterOverdue");
            var mnuFilterDue = new MenuItem("Filter for due", "mnuFilterDue");
            var mnuFilterASAP = new MenuItem("Filter for ASAP", "mnuFilterASAP");
            _frm.MenuBar.Menu.AddItem(new MenuItem("View")).Submenu.AddItems(new[] {
                mnuFilterOverdue, mnuFilterDue, mnuFilterASAP
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
                    
                    case "mnuFilterOverdue":
                    case "mnuFilterDue":
                    case "mnuFilterASAP":
                        item.Checked = !item.Checked;
                        OnQueryTasksRequest(Build_query());
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
            
            _txtQuery = new TextLine(2, _lstTasks.Position.top + _lstTasks.Size.height + 1, 45) {
                Label = "query: words or #tags separated by spaces"
            };
            _frm.AddChild(_txtQuery);
            
            _frm.AddChild(new Button(_txtQuery.Position.left + _txtQuery.Size.width + 2, _txtQuery.Position.top, 10, "Filter") { OnPressed = (s, e) => {
                OnQueryTasksRequest(Build_query());
            }});


            string Build_query() {
                var query = _txtQuery.Text;
                if (mnuFilterOverdue.Checked) query += " !overdue";
                if (mnuFilterDue.Checked) query += " !due";
                if (mnuFilterASAP.Checked) query += " !asap";
                return query;
            }
        }


        public Action<string> OnQueryTasksRequest;
        public Action OnNewTaskRequest;
        public Action<string> OnEditTaskRequest;
        public Action<string> OnDeleteTaskRequest;
        

        public void Show() => BashForms.Open(_frm);

        
        public void Display(Task[] tasks) {
            var currentTaskId = (string)_lstTasks.CurrentItem?.Attachment;

            Display_items();
            Set_current_item();


            void Display_items() {
                _lstTasks.Clear();
                foreach (var t in tasks) {
                    var item = _lstTasks.Add(Format_task_info(t));
                    Embelish_item(item, t);
                }
            }

            void Set_current_item() {
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
        }

        
        public void DisplayUpdate(Task task) {
            var taskEntry = Locate_task_entry(task.Id);
            if (!Try_update_existing())
                Add_new();
            Embelish_item(taskEntry.item, task);


            bool Try_update_existing() {
                if (taskEntry.item == null) return false;
                var currentItemIndex = _lstTasks.CurrentItemIndex;
                _lstTasks.RemoveAt(taskEntry.index);
                taskEntry.item = new Listbox.Item(Format_task_info(task));
                _lstTasks.Insert(taskEntry.index, taskEntry.item);
                _lstTasks.CurrentItemIndex = currentItemIndex;
                return true;
            }

            void Add_new() {
                taskEntry.item = _lstTasks.Add(Format_task_info(task));
                _lstTasks.CurrentItemIndex = _lstTasks.Items.Length - 1;
            }
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
            var dueAt = task.DueAt.Date == DateTime.MaxValue.Date ? "" : task.DueAt.ToString("d");
            var prio = task.Priority == TaskPriorities.No ? "" : task.Priority.ToString();
            var description = task.Description.Replace("\n", " ");
            return $"{task.Subject}\t{description}\t{dueAt}\t{prio}";
        }
    }
}