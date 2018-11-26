using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TurboKAPOWNIK
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        
        private int new_task_id { get; set; }
        internal Task genTask { get; set; }
        internal Task parentTask { get; set; }

        public AddTask(int new_id, Task parent_task=null)
        {
            InitializeComponent();
            popullateCategoryCombobox();
            sp_value.Text = 0.ToString();
            new_task_id = new_id;
            if (parent_task != null)
            {
                parent.Content = "Parent task: " + parent_task.id + "-" + parent_task.task_name;
                parentTask = parent_task;
            }                
        }

        private void popullateCategoryCombobox()
        {
            foreach (var category in Category.category)
            {
                category_combo.Items.Add(category.name);
            }
        }
        private Category getCategoryObj(string name)
        {
            foreach (var category in Category.category)
            {
                if (category.name == name)
                    return category;
            }
            return null;
        }

        private void category_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string category_name = e.AddedItems[0].ToString();
            sp_value.Text = getCategoryObj(category_name).SP.ToString();
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (parentTask == null)
                genTask = new Task(new_task_id, task_name.Text, getCategoryObj(category_combo.SelectedItem.ToString()), task_details.Text);
            else
                genTask = new Task(new_task_id, task_name.Text, getCategoryObj(category_combo.SelectedItem.ToString()), task_details.Text, parentTask.id);
            this.Close();
        }
    }
}
