using System;
using System.Threading.Tasks;
using bashforms;
using bashforms_tests.todo_scenario.adapters.views;

namespace bashforms_tests.todo_scenario.adapters
{
    public class ConsolePortal
    {
        private readonly MainWindow _win;
        private readonly TaskDialog _dlg;

        public ConsolePortal() {
            _win = new MainWindow();
            _dlg = new TaskDialog();

            _win.OnQueryTasksRequest += query => OnQueryTasksRequest(query);
            _win.OnEditTaskRequest += taskId => OnEditTaskRequest(taskId);
            _win.OnNewTaskRequest += () => OnNewTaskRequest();
            _win.OnDeleteTaskRequest += taskId => OnDeleteTaskRequest(taskId);
        }
        
        
        public event Action<string> OnQueryTasksRequest;
        public event Action<string> OnEditTaskRequest;
        public event Action OnNewTaskRequest;
        public event Action<string> OnDeleteTaskRequest;
        
        
        public void Show() {
            _win.Show();
        }
        
        
        public void Display(data.Task[] tasks) {
            _win.Display(tasks);
        }

        public void DisplayUpdate(data.Task task) {
            _win.DisplayUpdate(task);
        }

        public bool AskUserForNewTask(out data.Task newTask) {
            newTask = _dlg.EditNew();
            return newTask != null;
        }

        public bool AllowUserToEditTask(ref data.Task task) {
            task = _dlg.Edit(task);
            return task != null;
        }
    }
}