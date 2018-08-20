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
            var pquery = Preprocess(query);
            return tasks.Where(t => Matches_query(t, pquery)).ToArray();
        }

        public Task[] Requery() => Query(_latestQuery);


        (string[] words, string[] tags) Preprocess(string query) {
            var parts = query.Split(new[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);
            return (parts.Where(p => !p.StartsWith("#")).ToArray(),
                    parts.Where(p => p.StartsWith("#")).Select(p => p.Substring(1)).ToArray());
        }
        
        
        bool Matches_query(Task task, (string[] words, string[] tags) pquery) {
            if (pquery.words.Length == 0 && pquery.tags.Length == 0) return true;
            
            if (pquery.words.Any(w => task.Subject.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0)) return true;
            if (pquery.words.Any(w => task.Description.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0)) return true;
            return pquery.tags.Any(t => task.Tags.Contains(t));
        }
    }
}