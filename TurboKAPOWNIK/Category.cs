using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboKAPOWNIK
{
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
            new Category(10, "Other/Aggregation Task", 0)
        };
        public static string getCategoryText(int cat_index)
        {
            return category[cat_index].name;
        }
    }
}
