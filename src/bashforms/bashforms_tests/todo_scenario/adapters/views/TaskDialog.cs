using System;
using System.Runtime.InteropServices.WindowsRuntime;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;
using bashforms_tests.todo_scenario.data;

namespace bashforms_tests.todo_scenario.adapters.views
{
    class TaskDialog
    {
        private readonly Dialog<bool> _dlg;
        private readonly TextLine _txtSubject;
        private readonly TextArea _txtDescription;
        private readonly TextLine _txtDueDate;
        private readonly Combobox _cboPriority;
        private readonly TextLine _txtTags;
        
        
        public TaskDialog() {
            _dlg = new Dialog<bool>(2,2,Console.WindowWidth * 2 / 3,Console.WindowHeight * 3 / 4){Title = "Edit task"};

            _txtSubject = new TextLine(2, 2, _dlg.Size.width - 4) {Label = "subject"};
            _dlg.AddChild(_txtSubject);

            _txtDescription = new TextArea(2,4, _txtSubject.Size.width / 2, _dlg.Size.height - 7){Label = "description"};
            _dlg.AddChild(_txtDescription);
            
            _txtDueDate = new TextLine(_txtDescription.Position.left+_txtDescription.Size.width + 2, 4, 10){Label = "due date"};
            _dlg.AddChild(_txtDueDate);

            _cboPriority = new Combobox(_txtDueDate.Position.left, _txtDueDate.Position.top + 2, _txtDueDate.Size.width, 6,
                new[] {"", "Low", "Medium", "High", "ASAP"}) {LimitTextToItems = true};
            _dlg.AddChild(_cboPriority);
            
            _txtTags = new TextLine(_cboPriority.Position.left, _cboPriority.Position.top+2,_cboPriority.Size.width){Label = "tags"};
            _dlg.AddChild(_txtTags);

            var btnSave = new Button(2, _txtDescription.Position.top + _txtDescription.Size.height + 1, 10, "Save") { OnPressed = (w, e) => {
                _dlg.Result = true;
                BashForms.Close();
            }};
            _dlg.AddChild(btnSave);
            
            _dlg.AddChild(new Button(14,btnSave.Position.top,10, "Cancel"){OnPressed = (w, e) => {
                _dlg.Result = false;
                BashForms.Close();
            }});
        }


        public Task EditNew() => Edit(null);
        public Task Edit(Task task) {
            if (task == null) {
                task = new Task();
                _dlg.Title = "Edit new task";
            }
            
            _txtSubject.Text = task.Subject;
            _txtDescription.Text = task.Description;
            _txtDueDate.Text = task.DueAt == DateTime.MaxValue ? "" : task.DueAt.ToString("d");
            _cboPriority.Text = task.Priority == TaskPriorities.No ? "" : task.Priority.ToString();
            _txtTags.Text = String.Join(",", task.Tags);

            if (!BashForms.OpenModal(_dlg)) return null;

            task.Subject = _txtDescription.Text;
            task.Description = _txtDescription.Text;
            task.DueAt = _txtDueDate.Text != "" ? DateTime.Parse(_txtDueDate.Text) : DateTime.MaxValue;
            if (!Enum.TryParse(_cboPriority.Text, true, out task.Priority)) task.Priority = TaskPriorities.No;
            task.Tags = _txtTags.Text.Split(new[] {',', ';', '#',' '}, StringSplitOptions.RemoveEmptyEntries);

            return task;
        }
    }
}