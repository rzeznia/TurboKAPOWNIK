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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace TurboKAPOWNIK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<Task> Task_List = new List<Task>();
        private Task SelectedTask { get; set; }
        internal Task newTask { get; set; }
        internal bool JiraFilterSwitch { get; set; }
        private Sprint current_sprint = new Sprint();
        internal string sprint_path_file { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            rp_sp.Text = "0";
            all_sp.Text = "0";
            string[] sprints = CheckForSprintFiles();
            if(sprints.Length > 0)
                SelectExtSprint(sprints);
            else
                AddNewSprint();


            if (current_sprint.task_list != null)
                TreeRefresh();
            
        }

        private void AddNewSprint()
        {
            AddSprint add_sprint = new AddSprint();
            add_sprint.ShowDialog();
            if (add_sprint.genSprint != null)
            {
                current_sprint = add_sprint.genSprint;
                refreshSprintInfo();
                sprint_path_file = Directory.GetCurrentDirectory().ToString() + "\\" + current_sprint.sprint_name + ".json";
                sbar_main_message.Text = "Sprint Added Successfully";
            }
        }
        private void AddNewSprint(Sprint new_sprint)
        {
            if (new_sprint != null)
            {
                current_sprint = new_sprint;
                refreshSprintInfo();
                sprint_path_file = Directory.GetCurrentDirectory().ToString() + "\\" + new_sprint.sprint_name + ".json";
                sbar_main_message.Text = "Sprint Added Successfully";
            }
        }

        private void SelectExtSprint(string[] sprints)
        {
            SelectSprint ssprint = new SelectSprint(sprints);
            ssprint.ShowDialog();
            if (ssprint.selected != null)
            {
                sprint_path_file = ssprint.selected;
                LoadSprintData(sprint_path_file);
            }
            else if (ssprint.newSprint != null)
                AddNewSprint(ssprint.newSprint);
            else
                this.Close();
        }

        private void LoadSprintData(string sprint_path_file)
        {
            using (StreamReader r = new StreamReader(sprint_path_file))
            {
                string json = r.ReadToEnd();
                current_sprint = JsonConvert.DeserializeObject<Sprint>(json);
            }
            refreshSprintInfo();
            sbar_main_message.Text = "Sprint " + current_sprint.sprint_name + " loaded";
            if (current_sprint.task_list != null)
                TreeRefresh();
        }

        private void TreeRefresh()
        {
            treeNew.Items.Clear();
            treeActive.Items.Clear();
            treeDone.Items.Clear();
            Dictionary<int, int> DiffSubStatus = new Dictionary<int, int>();
            foreach (var task in current_sprint.task_list)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem = new TreeViewItem();
                treeItem.Header = task.id + "-" + task.task_name;
                DiffSubStatus.Clear();
                int InJiraSub = 0;
                if (task.subtasks.Count() > 0)
                {                    
                    foreach (var subtask in task.subtasks)
                    {
                        
                        if(subtask.status == task.status)
                        {
                            TreeViewItem subItem = new TreeViewItem() { Header = subtask.id + "-" + subtask.task_name };
                            if (subtask.inJira == true)
                            {
                                FormatItemIfJira(subItem);
                                InJiraSub++;
                            }                                
                            else
                                FormatItemIfNotJira(subItem);
                            if(JiraFilterSwitch == false || (JiraFilterSwitch == true && subtask.inJira == false))
                                treeItem.Items.Add(subItem);
                        }                            
                        else
                        {
                            DiffSubStatus.Add(subtask.id, subtask.status);
                            if (subtask.inJira == true)
                                InJiraSub++;
                            
                        }
                    }
                }

                if (task.inJira == true)
                    FormatItemIfJira(treeItem);

                if(JiraFilterSwitch == false || (task.inJira == false && JiraFilterSwitch == true || InJiraSub != task.subtasks.Count()))
                {
                    if (task.status == 1)
                        treeNew.Items.Add(treeItem);
                    else if (task.status == 2)
                        treeActive.Items.Add(treeItem);
                    else
                        treeDone.Items.Add(treeItem);
                }                

                if (DiffSubStatus.Count > 0)
                {
                    List<int> subsNew = new List<int>();
                    List<int> subsAtv = new List<int>();
                    List<int> subsDne = new List<int>();
                    foreach (var subid in DiffSubStatus)
                    {

                        if (subid.Value == 1)
                            subsNew.Add(subid.Key);
                        else if (subid.Value == 2)
                            subsAtv.Add(subid.Key);
                        else
                            subsDne.Add(subid.Key);
                    }
                    if (subsNew.Count() > 0)
                        treeNew.Items.Add(BuildDiffSubTaskView(subsNew));
                    if (subsAtv.Count() > 0)
                        treeActive.Items.Add(BuildDiffSubTaskView(subsAtv));
                    if (subsDne.Count() > 0)
                        treeDone.Items.Add(BuildDiffSubTaskView(subsDne));
                }
                
            }
            treeNew.Items.OfType<TreeViewItem>().ToList().ForEach(ExpandAllNodes);
            treeActive.Items.OfType<TreeViewItem>().ToList().ForEach(ExpandAllNodes);
            treeDone.Items.OfType<TreeViewItem>().ToList().ForEach(ExpandAllNodes);
            count_sps();
        }

        private void ExpandAllNodes(TreeViewItem treeItem)
        {
            treeItem.IsExpanded = true;
            foreach (var childItem in treeItem.Items.OfType<TreeViewItem>())
            {
                ExpandAllNodes(childItem);
            }
        }

        private void FormatItemIfJira(TreeViewItem item, bool parent=false)
        {
            item.FontStyle = FontStyles.Italic;
            item.FontWeight = FontWeights.Bold;
            if (parent == true)
                item.Foreground = Brushes.Gray;
        }
        private void FormatItemIfNotJira(TreeViewItem item, bool parent = false)
        {
            item.FontStyle = FontStyles.Normal;
            item.FontWeight = FontWeights.Normal;
            if (parent == true)
                item.Foreground = Brushes.Gray;
        }

        private TreeViewItem BuildDiffSubTaskView(List<int> subsNew)
        {
            int InJiraCount = 0;
            TreeViewItem treeItem = new TreeViewItem();
            treeItem = new TreeViewItem();
            int Parent = taskFinder(subsNew[0]).parent_task;
            Task ParentTask = taskFinder(Parent);
            treeItem.Header = ParentTask.id + "-" + ParentTask.task_name;
            if (ParentTask.inJira == true)
                FormatItemIfJira(treeItem, true);
            else
                FormatItemIfNotJira(treeItem, true);
            foreach (var sub in subsNew)
            {
                Task subtask = taskFinder(sub);
                TreeViewItem subItem = new TreeViewItem() { Header = subtask.id + "-" + subtask.task_name };
                if (subtask.inJira == true)
                {
                    FormatItemIfJira(subItem);
                    InJiraCount++;
                }
                else
                    FormatItemIfNotJira(subItem);
                if(JiraFilterSwitch == false || (subtask.inJira == false && JiraFilterSwitch == true))
                    treeItem.Items.Add(subItem);
            }
            if (subsNew.Count() == InJiraCount && JiraFilterSwitch == true)
                return null;
            else
                return treeItem;
        }

        private void add_task_Click(object sender, RoutedEventArgs e)
        {
            AddTask add = new AddTask(generateIdForNewTask());
            add.ShowDialog();
            if(add.genTask != null)
            {
                current_sprint.task_list.Add(add.genTask);               
                TreeRefresh();
                sbar_main_message.Text = "Task " + add.genTask.id + "-" + add.genTask.task_name + " added succesfully";
            }
            
        }

        private void count_sps()
        {
            var count_reported = 0;
            var count_all = 0;
            foreach (var item in current_sprint.task_list)
            {
                count_all += (item.category.SP * item.multiplier);
                if (item.inJira)
                {
                    count_reported += (item.category.SP * item.multiplier);
                }
                if(item.subtasks.Count != 0)
                {
                    foreach (var subitem in item.subtasks)
                    {
                        count_all += (subitem.category.SP * subitem.multiplier);
                        if (subitem.inJira)
                        {
                            count_reported += (subitem.category.SP * subitem.multiplier);
                        }
                    }
                }
            }
            rp_sp.Text = count_reported.ToString();
            all_sp.Text = count_all.ToString();
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                jira_copy.IsEnabled = true;
                string selectedString = ((TreeViewItem)e.NewValue).Header.ToString();
                string[] starr = selectedString.Split('-'); 
                SelectedTask = taskFinder(Convert.ToInt32(starr[0]));
                textBox.Text = SelectedTask.task_name;
                label_status.Content = "Task Status: " + SelectedTask.getStatusName();
                inJira_chBox_Handle();
                categoryBox.Content = SelectedTask.category.name;
                spBox.Content = "SP:" + SelectedTask.category.SP + " x " + SelectedTask.multiplier+ " = " + (SelectedTask.category.SP * SelectedTask.multiplier);
                statusButtonRefresh();
                commentsBoxRefresh();
                subTaskButtonHandler();
                sbar_main_message.Text = "Selected task: " + selectedString;
                
            }
            catch (NullReferenceException) { }          
        }

        private void subTaskButtonHandler()
        {
            if (SelectedTask.parent_task == 0)
                subAdd.IsEnabled = true;
            else
                subAdd.IsEnabled = false;
        }

        private void commentsBoxRefresh()
        {
            int comment_count = 0;
            commentsView.Items.Clear();
            if(SelectedTask.comments.Count > 0)
            {
                comment_count++;
                foreach (var comment in SelectedTask.comments)
                {
                    comment_count++;
                    TreeViewItem comment_item = new TreeViewItem();
                    comment_item.Header = comment_count + "--" + comment.add_date;
                    comment_item.Items.Add(new TreeViewItem() { Header = comment.comment});
                    commentsView.Items.Add(comment_item);
                }
            }
        }

        private void statusButtonRefresh()
        {
            if (SelectedTask.status == 1)
            {
                reopen.IsEnabled = false;
                active.IsEnabled = true;
                resolve.IsEnabled = true;
            }
            else if (SelectedTask.status == 2)
            {
                reopen.IsEnabled = true;
                active.IsEnabled = false;
                resolve.IsEnabled = true;
            }
            else
            {
                reopen.IsEnabled = true;
                active.IsEnabled = true;
                resolve.IsEnabled = false;
            }
            label_status.Content = SelectedTask.getStatusName();
        }

        private Task taskFinder(int v)
        {
            foreach (var task in current_sprint.task_list)
            {
                if (task.id == v)
                    return task;
                else
                {
                    if (task.subtasks.Count() > 0)
                    {
                        foreach (var subtask in task.subtasks)
                        {
                            if (subtask.id == Convert.ToInt32(v))
                            {
                                return subtask;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void active_Click(object sender, RoutedEventArgs e)
        {
            SelectedTask.status = 2;
            SelectedTask.start_date = DateTime.Now;
            statusButtonRefresh();
            TreeRefresh();
            sbar_main_message.Text = sbar_main_message.Text + " -> made active";
        }

        private void reopen_Click(object sender, RoutedEventArgs e)
        {
            SelectedTask.status = 1;
            statusButtonRefresh();
            TreeRefresh();
            sbar_main_message.Text = sbar_main_message.Text + " -> reopened";
        }

        private void resolve_Click(object sender, RoutedEventArgs e)
        {
            SelectedTask.status = 3;
            statusButtonRefresh();
            TreeRefresh();
            sbar_main_message.Text = sbar_main_message.Text + " -> resolved";
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {       
            var json = JsonConvert.SerializeObject(current_sprint);
            System.IO.File.WriteAllText(sprint_path_file, json);
            sbar_last_saved.Text = "Last Saved Sprint: " + DateTime.Now.ToString();
            sbar_main_message.Text = "Sprint " + current_sprint.sprint_name + " saved sucessfully.";
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader r = new StreamReader("tasks.json"))
            {
                string json = r.ReadToEnd();
                current_sprint = JsonConvert.DeserializeObject<Sprint>(json);
            }
            refreshSprintInfo();
            if(current_sprint.task_list != null)
                TreeRefresh();
        }
        private int generateIdForNewTask()
        {
            if (current_sprint.task_list.Count > 0)
            {
                List<int> ids = new List<int>();
                foreach (var task in current_sprint.task_list)
                {
                    ids.Add(task.id);
                    if (task.subtasks.Count() > 0)
                    {
                        foreach (var subtask in task.subtasks)
                            ids.Add(subtask.id);
                    }
                }
                return ids.Max() + 1;
            }
            else
                return 1;            
        }

        private void inJiraChBox_Checked(object sender, RoutedEventArgs e)
        {
            bool ch_value;
            if (inJiraChBox.IsChecked == true)
                ch_value = true;
            else
                ch_value = false;
            SelectedTask.inJira = ch_value;
            inJira_chBox_Handle();
        }
        private void inJira_chBox_Handle()
        {
            if (SelectedTask.inJira == true)
            {
                inJiraChBox.IsChecked = true;
                sbar_main_message.Text = "Task marked in Jira";
            }                
            else
            {
                inJiraChBox.IsChecked = false;
                sbar_main_message.Text = "Task not marked in Jira";
            }                
            TreeRefresh();
        }

        private void HideIfJira_Checked(object sender, RoutedEventArgs e)
        {
            if (HideIfJira.IsChecked == true)
            {
                JiraFilterSwitch = true;
                sbar_main_message.Text = "Jira tasks not visible";
            }                
            else
            {
                sbar_main_message.Text = "Jira tasks visible";
                JiraFilterSwitch = false;
            }
                
            TreeRefresh();
        }

        private void deleteTask_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTask != null)
            {
                current_sprint.task_list.Remove(SelectedTask);
                sbar_main_message.Text = "Task " + SelectedTask.id + "-" + SelectedTask.task_name + " deleted succesfully";
                TreeRefresh();
                selectedTaskReset();                
            }
        }

        private void selectedTaskReset()
        {
            throw new NotImplementedException();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void commentsView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void cmtAdd_Click(object sender, RoutedEventArgs e)
        {
            AddComment comment = new AddComment();
            comment.ShowDialog();
            if(comment.genComment != null)
            {
                SelectedTask.comments.Add(comment.genComment);
                commentsBoxRefresh();
                sbar_main_message.Text = "Comment added";
            }
        }

        private void add_sprint_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void refreshSprintInfo()
        {
            cs_start.Content = current_sprint.start_date;
            cs_end.Content = current_sprint.end_date;
            cs_day.Content = getWorkingDays(current_sprint.start_date);
            cs_name.Content = "Active sprint: " + current_sprint.sprint_name;
            add_sprint.IsEnabled = false;
        }

        private string getWorkingDays(DateTime day)
        {
            int days_count = 0;
            DateTime now = DateTime.Now.Date;
            for(;;)
            {
                if (now.DayOfWeek == DayOfWeek.Sunday && now.DayOfWeek == DayOfWeek.Saturday)
                    now = new DateTime(now.Year, now.Month, now.Day - 1);
                else
                    break;
            }
            int now_day = now.Day;
            int start_day = day.Day;
            for(int i = start_day; i <= now_day; i++)
            {
                DateTime temp_date = new DateTime(day.Year, day.Month, i);
                if (temp_date.DayOfWeek != DayOfWeek.Saturday && temp_date.DayOfWeek != DayOfWeek.Sunday)
                    days_count++;
            }
            return days_count.ToString();
            

        }

        private void subAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTask addsub = new AddTask(generateIdForNewTask(), SelectedTask);
            addsub.ShowDialog();
            Task TempTask = taskFinder(SelectedTask.id);
            TempTask.subtasks.Add(addsub.genTask);
            //sbar_main_message.Text = "Subtask " + addsub.genTask.id + "-" + addsub.genTask.task_name + " for task " + TempTask.id + "-" + TempTask.task_name + " added successfully.";
            TreeRefresh();
        }

        private string[] CheckForSprintFiles()
        {
            string dir = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(dir,"*.json");
            return files;
        }

        private void task_edit_Click(object sender, RoutedEventArgs e)
        {
            var elo = 0;
            if (SelectedTask.parent_task != 0)
            {
                var parent = taskFinder(SelectedTask.parent_task);
                elo = current_sprint.task_list.IndexOf(parent);
            }
            else
            {
                elo = current_sprint.task_list.IndexOf(SelectedTask);
            }
            
            AddTask edit = new AddTask(0, SelectedTask);
            edit.ShowDialog();

            if (SelectedTask.parent_task != 0)
            {
                var parent = taskFinder(SelectedTask.parent_task);
                var subindex = parent.subtasks.IndexOf(SelectedTask);
                parent.subtasks[subindex] = edit.genTask;
            }
            else
            {
                current_sprint.task_list[elo] = edit.genTask;
            }

            TreeRefresh();

        }

        private void jira_copy_Click(object sender, RoutedEventArgs e)
        {
            var jira_str = "";
            if(SelectedTask.multiplier == 1)
                jira_str = "||Category||SP||\n| " + SelectedTask.category.name + "| " + SelectedTask.category.SP + "|";
            else
                jira_str = "||Category||SP||\n| " + SelectedTask.category.name + "| " + SelectedTask.category.SP + "x" + SelectedTask.multiplier + "|";
            Clipboard.SetText(jira_str);
            sbar_main_message.Text = "Jira string copied!";
        }
    }
}
