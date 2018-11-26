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
        public string details { get; set; }
        public List<Comments> comments = new List<Comments>();
        public int SP { get; set; }
        public int status { get; set; }
        public List<Task> subtasks = new List<Task>();
        public DateTime add_Date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime resolve_time { get; set; }
        public int parent_task { get; set; }
        public bool inJira { get; set; }
        

        [JsonConstructor]
        public Task(int id, string task_name, string asignee, Category category, string details, Comments comment, int sp, int status, Task subtask, DateTime adddate, DateTime startdate, DateTime resolvedate, int parent, bool jira)
        {
            this.id = id;
            this.task_name = task_name;
            this.asignee = asignee;
            this.category = category;
            this.details = details;
            this.comments.Add(new Comments(comment));
            this.SP = sp;
            this.status = status;
            this.subtasks.Add(subtask);
            this.add_Date = adddate;
            this.start_date = startdate;
            this.resolve_time = resolvedate;
            this.parent_task = parent;
            this.inJira = jira;
        }
        public Task(int id, string task_name, Category category, string details)
        {
            this.id = id;
            this.task_name = task_name;
            this.details = details;
            this.status = 1;
            this.SP = category.SP;
            this.asignee = "Marcin Rzeźnik";
            this.add_Date = assign_datetime();
            this.inJira = false;
            this.category = category;
        }
        public Task(int id, string task_name, Category category, string details, int parent)
        {
            this.id = id;
            this.task_name = task_name;
            this.details = details;
            this.status = 1;
            this.SP = 0;
            this.asignee = "Marcin Rzeźnik";
            this.add_Date = assign_datetime();
            this.parent_task = parent;
            this.inJira = false;
            this.category = category;
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
        
        internal void CreateSubtask(Task task, Category category)
        {
            this.subtasks.Add(new Task(task.id, task.task_name, category, task.details, this.id));
        }
        internal void isinJira(bool condition)
        {
            this.inJira = condition;
        }       
    }
    
    public class Category
    {
        public int id;
        public string name;
        public int SP;

        public Category(Category cat)
        {
            this.id = cat.id;
            this.name = cat.name;
            this.SP = cat.SP;
        }
        [JsonConstructor]
        public Category(int id, string name, int SP)
        {
            this.id = id;
            this.name = name;
            this.SP = SP;
        }
        public static List<Category> category = new List<Category>() {
            new Category(0, "Lab Maintenance - Simple setup preparation", 7),
            new Category(1, "Lab Maintenance - Advanced setup preparation", 24),
            new Category(2, "Lab Maintenance - SAS Management", 5),
            new Category(3, "Lab Maintenance - SWARM", 12),
            new Category(4, "Database/tools management - Tools management", 23),
            new Category(5, "Database/tools management - Stocktaking", 18),
            new Category(6, "Database/tools management - Database update", 11),
            new Category(7, "Simple sighting reproduction", 8),
            new Category(8, "Advanced sighting reproduction", 28),
            new Category(9, "Test Execution", 0),
            new Category(10, "Other", 0)
        };
        public static string getCategoryText(int cat_index)
        {
            return category[cat_index].name;
        }
    }
    [Serializable]
    class Sprint
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public List<Task> task_list = new List<Task>();
        public string sprint_name { get; set; }

        //public Sprint(List<Task> task_list_new, DateTime start_date, DateTime end_date, int sprint_no)
        //{
        //    this.task_list = task_list_new;
        //    this.start_date = start_date;
        //    this.end_date = end_date;
        //    this.sprint_name = start_date.Year.ToString() + "." + start_date.Month.ToString() + "." + sprint_no;
        //}
        [JsonConstructor]
        public Sprint(DateTime start_date, DateTime end_date, int sprint_no)
        {
            this.task_list = new List<Task>();
            this.start_date = start_date;
            this.end_date = end_date;
            this.sprint_name = start_date.Year.ToString() + "." + start_date.Month.ToString() + "." + sprint_no;
        }
        public Sprint(Sprint new_sprint)
        {
            this.start_date = new_sprint.start_date;
            this.end_date = new_sprint.end_date;
            this.task_list = new List<Task>();
            this.sprint_name = new_sprint.sprint_name;
        }

        public Sprint()
        {
            task_list = null;
        }
    }
}
