using System;
using System.Runtime.InteropServices.ComTypes;
using bashforms;
using bashforms.data;
using bashforms.widgets.controls;
using bashforms.widgets.windows;
using bashforms_tests.todo_scenario.adapters;

namespace bashforms_tests.todo_scenario
{
    public class ToDoApp
    {
        public static void Enterypoint() {
            var ui = new ConsolePortal();
            var repo = new TaskRepository();
            var app = new ToDoApp(ui, repo);
            
            app.Run();
        }
        
        
        private readonly ConsolePortal _ui;
        private readonly TaskRepository _repo;
        
        internal ToDoApp(ConsolePortal ui, TaskRepository repo) {
            _ui = ui;
            _repo = repo;
        }
        
        
        public void Run(){ 
            var rh = new RequestHandler(_repo);

            _ui.OnQueryRequest += query => {
                var queryResult = rh.Query(query);
                _ui.Display(queryResult);
            };

            _ui.OnNewTaskRequest += () => {
                if (_ui.AskUserForNewTask(out var newTask)) {
                    newTask = rh.StoreNewTask(newTask);
                    _ui.DisplayUpdate(newTask);
                }
            };

            var tasks = rh.Query("");
            _ui.Display(tasks);
            _ui.Show();
        }
    }
}