using System;
using System.Threading.Tasks;
using bashforms;
using bashforms_tests.todo_scenario.adapters.views;

namespace bashforms_tests.todo_scenario.adapters
{
    public class ConsolePortal
    {
        private readonly MainWindow _win;
        private TaskDialog _dlg;

        public ConsolePortal() {
            _win = new MainWindow();
            _dlg = new TaskDialog();
        }
        
        
        public void Show() {
            _win.Show();
        }
        
        
        public void Display(data.Task[] tasks)
        {
            
        }

        public void DisplayUpdate(data.Task task)
        {
            
        }

        public event Action<string> OnQueryRequest;
        public event Action OnNewTaskRequest;

        public bool AskUserForNewTask(out data.Task newTask)
        {
            throw new NotImplementedException();
        }
    }
}