using System;

namespace bashforms_tests.todo_scenario.data
{
    public enum TaskPriorities {
        No=0,
        Low=1,
        Medium=2,
        High=3,
        ASAP=4
    }
    
    public class Task {
        public string Id;
        public string Subject = "";
        public string Description = "";
        public DateTime DueAt = DateTime.MaxValue;
        public TaskPriorities Priority = TaskPriorities.No;
        public string[] Tags = new string[0];
        public DateTime CreatedAt = DateTime.Now;
    }
}