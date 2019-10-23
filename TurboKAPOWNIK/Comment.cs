using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboKAPOWNIK
{
    public class Comment
    {
        public int id { get; set; }
        public DateTime add_date { get; set; }
        public string comment { get; set; }

        public Comment(string comment_, int id_)
        {
            this.comment = comment_;
            this.id = id_;
            this.add_date = assign_datetime();
        }

        [JsonConstructor]
        public Comment(Comment comment1)
        {
            if (comment1 != null)
            {
                this.add_date = comment1.add_date;
                this.comment = comment1.comment;
            }
        }

        internal DateTime assign_datetime()
        {
            return DateTime.Now;
        }
    }
}
