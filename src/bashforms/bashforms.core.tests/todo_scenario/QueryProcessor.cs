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
            var preprocessedQuery = Preprocess(query);
            return tasks.Where(t => Matches_query(t, preprocessedQuery)).ToArray();
        }

        public Task[] Requery() => Query(_latestQuery);


        (string[] words, string[] tags, string[] filters) Preprocess(string preprocessedQuery) {
            var parts = preprocessedQuery.Split(new[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);
            return (parts.Where(p => !p.StartsWith("#") && !p.StartsWith("!")).ToArray(),
                    parts.Where(p => p.StartsWith("#")).Select(p => p.Substring(1)).ToArray(),
                    parts.Where(p => p.StartsWith("!")).Select(p => p.Substring(1)).ToArray());
        }
        
        
        bool Matches_query(Task task, (string[] words, string[] tags, string[] filters) preprocessedQuery) {
            // apply filters
            if (preprocessedQuery.filters.Contains("overdue") && DateTime.Now < task.DueAt) return false;
            if (preprocessedQuery.filters.Contains("due") && DateTime.Now.Date != task.DueAt.Date) return false;
            if (preprocessedQuery.filters.Contains("asap") && task.Priority != TaskPriorities.ASAP) return false;
            
            // match words and tags
            if (preprocessedQuery.words.Length == 0 && preprocessedQuery.tags.Length == 0) return true;

            if (preprocessedQuery.words.Any(w => task.Subject.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0)) return true;
            if (preprocessedQuery.words.Any(w => task.Description.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0)) return true;
            return preprocessedQuery.tags.Any(t => task.Tags.Contains(t));
        }
    }
}