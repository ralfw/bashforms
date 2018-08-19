using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace bashforms_tests.todo_scenario.adapters
{
    public class TaskRepository
    {
        private readonly string _path;
        private readonly JavaScriptSerializer _json;
        
        public TaskRepository() : this("tasks") {}
        internal TaskRepository(string path) {
            _path = path;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            _json = new JavaScriptSerializer();
        }
        
        
        public string Save(data.Task task) {
            if (task.Id == null) task.Id = Guid.NewGuid().ToString();
            var jsonTask = _json.Serialize(task);
            File.WriteAllText(Build_task_filepath(task.Id), jsonTask);
            return task.Id;
        }


        public bool Delete(string taskId) {
            var filepath = Build_task_filepath(taskId);
            if (File.Exists(filepath)) {
                File.Delete(filepath);
                return true;
            }
            return false;
        }

        
        public data.Task[] Tasks {
            get {
                var taskFilepaths = Directory.GetFiles(_path, "*.txt");
                return taskFilepaths.Select(Load).OrderBy(t => t.CreatedAt).ToArray();


                data.Task Load(string filepath) {
                    var jsonTask = File.ReadAllText(filepath);
                    return _json.Deserialize<data.Task>(jsonTask);
                }
            }
        }
        
        
        string Build_task_filepath(string taskId) => Path.Combine(_path, taskId + ".txt");
    }
}