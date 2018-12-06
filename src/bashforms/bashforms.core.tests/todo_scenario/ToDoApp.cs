using System;
using System.Linq;
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

        private ToDoApp(ConsolePortal ui, TaskRepository repo) {
            _ui = ui;
            _repo = repo;
        }


        private void Run(){ 
            var queries = new QueryProcessor(_repo);
            Set_up_request_handling_processes();
            
            var tasks = queries.Query("");
            _ui.Display(tasks);
            _ui.Show();


            void Set_up_request_handling_processes() {
                _ui.OnQueryTasksRequest += query => {
                    var queryResult = queries.Query(query);
                    _ui.Display(queryResult);
                };

                _ui.OnEditTaskRequest += taskId => {
                    var task = _repo.Tasks.First(t => t.Id == taskId);
                    if (_ui.AllowUserToEditTask(ref task)) {
                        _repo.Save(task);
                        _ui.DisplayUpdate(task);
                    }
                };
            
                _ui.OnNewTaskRequest += () => {
                    if (_ui.AskUserForNewTask(out var newTask)) {
                        var taskId = _repo.Save(newTask);
                        newTask.Id = taskId;
                        _ui.DisplayUpdate(newTask);
                    }
                };

                _ui.OnDeleteTaskRequest += taskId => {
                    _repo.Delete(taskId);
                    var queryResult = queries.Requery();
                    _ui.Display(queryResult);
                };
            }
        }
    }
}