using System;
using System.Linq;
using bashforms_tests.todo_scenario.adapters;
using bashforms_tests.todo_scenario.data;

namespace bashforms_tests.todo_scenario
{
    public class QueryProcessor
    {
        private readonly TaskRepository _repo;
        private string _latestQuery;

        
        public QueryProcessor(TaskRepository repo) {
            _repo = repo;
            _latestQuery = "";
        }
        
        
        public data.Task[] Query(string query) {
            _latestQuery = query;
            var tasks = _repo.Tasks;
            return tasks.Where(t => Matches_query(t, query)).ToArray();
        }

        public Task[] Requery() => Query(_latestQuery);
        
        
        bool Matches_query(Task task, string query) {
            return task.Subject.IndexOf(query) >= 0 || task.Description.IndexOf(query) >= 0;
        }
    }
}