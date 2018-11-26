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
    /// Interaction logic for AddSprint.xaml
    /// </summary>
    public partial class AddSprint : Window
    {
        internal DateTime start_date_field { get; set; }
        internal DateTime end_date_field { get; set; }
        internal Sprint genSprint { get; set; }
        internal int sp_no { get; set; }

        public AddSprint()
        {
            InitializeComponent();
            //valdiate_add_button();
        }

        private void valdiate_add_button()
        {
            if(sp_no == null)
            {
                add_sp.IsEnabled = false;
            }
        }

        private void start_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            start_date_field = start_date.SelectedDate.Value;
            date_boxes_update();
        }

        private void end_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            end_date_field = end_date.SelectedDate.Value;
            date_boxes_update();
        }
        private void date_boxes_update()
        {
            if (start_date_field != null)
                label_start.Content = "Start Date: " + start_date_field.Year + "." + start_date_field.Month + "." + start_date_field.Day;
            if (end_date_field != null)
                label_end.Content = "End Date: " + end_date_field.Year + "." + end_date_field.Month + "." + end_date_field.Day;
        }

        private void add_sp_Click(object sender, RoutedEventArgs e)
        {
            genSprint = new Sprint(start_date_field, end_date_field, sp_no);
            Close();
        }

        private void sprint_chooser_Checked(object sender, RoutedEventArgs e)
        {
            if (sprint_chooser.IsChecked == true)
                sp_no = 2;
            else
                sp_no = 1;
        }
    }
}

