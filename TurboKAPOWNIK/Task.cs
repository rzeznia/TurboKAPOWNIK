using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboKAPOWNIK
{
    [Serializable]
    public class Task
    {
        public int id { get; set; }
        public string task_name { get; set; }
        public string asignee { get; set; }
        public Category category { get; set; }
        public int multiplier { get; set; }
        public string details { get; set; }
        public List<int> comments = new List<int>();
        public int SP { get; set; }
        public int status { get; set; }
        public List<int> subtasks = new List<int>();
        private int new_task_id;
        private string text1;
        private int v1;
        private string text2;
        private int v2;

        public DateTime add_Date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime resolve_time { get; set; }
        public int parent_task { get; set; }
        public bool inJira { get; set; }
        public bool isSubtask { get; set; }

        [JsonConstructor]
        public Task(int id, string task_name, string asignee, Category category, int multiplier, string details, int comment, int sp, int status, int subtask, DateTime adddate, DateTime startdate, DateTime resolvedate, int parent, bool jira, bool isSubtask)
        {
            this.id = id;
            this.task_name = task_name;
            this.asignee = asignee;
            this.category = category;
            this.details = details;
            this.comments.Add(comment);
            this.SP = sp;
            this.multiplier = multiplier;
            this.status = status;
            this.subtasks.Add(subtask);
            this.isSubtask = isSubtask;
            this.add_Date = adddate;
            this.start_date = startdate;
            this.resolve_time = resolvedate;
            this.parent_task = parent;
            this.inJira = jira;
        }
        public Task(int id, string task_name, Category category, int multiplier, string details)
        {
            this.id = id;
            this.task_name = task_name;
            this.details = details;
            this.status = 1;
            this.SP = category.SP;
            this.multiplier = multiplier;
            this.asignee = "Marcin Rzeźnik";
            this.add_Date = assign_datetime();
            this.inJira = false;
            this.category = category;
        }
        public Task(int id, string task_name, Category category, int multiplier, string details, int parent)
        {
            this.id = id;
            this.task_name = task_name;
            this.details = details;
            this.status = 1;
            this.SP = category.SP;
            this.multiplier = multiplier;
            this.asignee = "Marcin Rzeźnik";
            this.add_Date = assign_datetime();
            this.parent_task = parent;
            this.inJira = false;
            this.category = category;
            this.isSubtask = true;
        }

        public Task(int new_task_id, string asignee, string text1, Category category, int v1, string text2, List<int> comments, int v2, int status, List<int> subtasks, DateTime add_Date, DateTime start_date, DateTime resolve_time, int parent_task, bool inJira, bool isSubtask)
        {
            this.new_task_id = new_task_id;
            this.asignee = asignee;
            this.text1 = text1;
            this.category = category;
            this.v1 = v1;
            this.text2 = text2;
            this.comments = comments;
            this.v2 = v2;
            this.status = status;
            this.subtasks = subtasks;
            this.isSubtask = isSubtask;
            this.add_Date = add_Date;
            this.start_date = start_date;
            this.resolve_time = resolve_time;
            this.parent_task = parent_task;
            this.inJira = inJira;
        }

        internal string getStatusName()
        {
            switch(this.status)
            {
                case 1:
                    return "New";
                case 2:
                    return "In progress";
                case 3:
                    return "Resolved";
                default:
                    return "Corrupted";
            }                
        }
        internal DateTime assign_datetime()
        {
            return DateTime.Now;
        } 
        
        //internal void CreateSubtask(Task task, Category category)
        //{
        //    this.subtasks.Add(new Task(task.id, task.task_name, category, 1, task.details, this.id));
        //}
        internal void isinJira(bool condition)
        {
            this.inJira = condition;
        }       
    }
}
