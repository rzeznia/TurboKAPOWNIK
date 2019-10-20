using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboKAPOWNIK
{
    [Serializable]
    class Sprint
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public List<Task> task_list = new List<Task>();
        public string sprint_name { get; set; }

        [JsonConstructor]
        public Sprint(DateTime start_date, DateTime end_date, int sprint_no)
        {
            this.task_list = new List<Task>();
            this.start_date = start_date;
            this.end_date = end_date;
            this.sprint_name = start_date.Year.ToString() + "-" + start_date.Month.ToString() + "-" + sprint_no;
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
