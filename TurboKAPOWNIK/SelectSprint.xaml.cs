using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SelectSprint.xaml
    /// </summary>
    public partial class SelectSprint : Window
    {
        internal string selected { get; set; }
        internal Sprint newSprint { get; set; }
        public SelectSprint(string[] sprints)
        {
            InitializeComponent();
            foreach (var sprint in sprints)
            {
                slist.Items.Add(sprint);
            }
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void slist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = slist.SelectedItem.ToString();
            load.IsEnabled = true;
            getSprintDateDetails();
        }

        private void getSprintDateDetails()
        {
            Sprint selectedsprint = new Sprint();
            using (StreamReader r = new StreamReader(selected))
            {
                string json = r.ReadToEnd();
                selectedsprint = JsonConvert.DeserializeObject<Sprint>(json);
            }
            sstart.Content = "Start date: " + selectedsprint.start_date;
            send.Content = "End Date : " + selectedsprint.end_date;
        }

        private void addnew_Click(object sender, RoutedEventArgs e)
        {
            AddSprint addn = new AddSprint();
            addn.ShowDialog();
            if (addn.genSprint != null)
            {
                newSprint = addn.genSprint;
                Close();
            }
            else
                this.Close();
        }
    }
}
