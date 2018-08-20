using System;
using System.Linq;
using bashforms_tests.todo_scenario.adapters;
using bashforms_tests.todo_scenario.data;

namespace bashforms_tests.todo_scenario
{
    public class RequestHandler
    {
        private readonly TaskRepository _repo;
        private string _latestQuery;

        
        public RequestHandler(TaskRepository repo) {
            _repo = repo;
            _latestQuery = "";
        }
        
        
        public data.Task[] Query(string query) {
            _latestQuery = query;
            var tasks = _repo.Tasks;
            return tasks.Where(t => Matches_query(t, query)).ToArray();
        }

        public Task[] Requery() => Query(_latestQuery);
        
        
        public Task StoreNewTask(Task newTask) {
            _repo.Save(newTask);
            return newTask;
        }
        

        public void DeleteTask(string taskId) {
            _repo.Delete(taskId);
        }


        bool Matches_query(Task task, string query) {
            return task.Subject.IndexOf(query) >= 0 || task.Description.IndexOf(query) >= 0;
        }
    }
}