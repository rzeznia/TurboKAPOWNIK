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
    /// Interaction logic for AddComment.xaml
    /// </summary>
    public partial class AddComment : Window
    {
        internal Comment genComment { get; set; }
        internal int com_id { get; set; }
        public AddComment(int com_id)
        {
            InitializeComponent();
            this.com_id = com_id;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            genComment = new Comment(textBox.Text, com_id);
            Close();
        }
    }
}
