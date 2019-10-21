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
        public Task parentTask { get; set; }

        private bool edit { get; set; }

        public AddTask(int new_id, Task parent_task=null)
        {
            InitializeComponent();
            edit = false;
            popullateCategoryCombobox();
            sp_value.Text = 0.ToString();
            multiplier.Value = 1;
            new_task_id = new_id;
            if(new_id == 0)
            {
                this.parentTask = parent_task;
                task_name.Text = parent_task.task_name;
                multiplier.Text = parent_task.multiplier.ToString();
                category_combo.SelectedValue = parent_task.category.name;
                new_task_id = parent_task.id;
                edit = true;
                button_confirm.IsEnabled = true;
            }
            else if (parent_task != null)
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
            button_confirm.IsEnabled = true;
            string category_name = e.AddedItems[0].ToString();
            sp_value.Text = getCategoryObj(category_name).SP.ToString();
            sp_multi.Text = sp_value.Text;
            multiplier.Text = 1.ToString();
        }

        private void multiplier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var sp = Convert.ToInt32(sp_value.Text);
            sp = sp * Convert.ToInt32(multiplier.Text);
            sp_multi.Text = sp.ToString();
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (this.parentTask == null)
                genTask = new Task(new_task_id, task_name.Text, getCategoryObj(category_combo.SelectedItem.ToString()), Convert.ToInt32(multiplier.Text), task_details.Text);
            else if (edit == true)
            {
                genTask = parentTask;
                genTask.task_name = task_name.Text;
                genTask.category = getCategoryObj(category_combo.SelectedItem.ToString());
                genTask.multiplier = Convert.ToInt32(multiplier.Text);
                genTask.details = task_details.Text;
            }
            else
                genTask = new Task(new_task_id, task_name.Text, getCategoryObj(category_combo.SelectedItem.ToString()), Convert.ToInt32(multiplier.Text), task_details.Text, parentTask.id);
            this.DialogResult = true;
            this.Close();
        }
    }
}
