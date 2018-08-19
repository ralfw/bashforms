using System;
using bashforms_tests.todo_scenario.adapters;
using bashforms_tests.todo_scenario.data;

namespace bashforms_tests.todo_scenario
{
    public class RequestHandler
    {
        private readonly TaskRepository _repo;
        public RequestHandler(TaskRepository repo) { _repo = repo; }

        
        public data.Task[] Query(string query) {
            return _repo.Tasks;
            
        }
        
        
        public Task StoreNewTask(Task newTask) {
            _repo.Save(newTask);
            return newTask;
        }
    }
}