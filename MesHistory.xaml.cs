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

namespace BS
{
    /// <summary>
    /// MesHistory.xaml 的交互逻辑
    /// </summary>
    public partial class MesHistory : Window
    {
        User userSearch = null;
        DetailDAL dbhistory = new DetailDAL();
        public MesHistory()
        {
            InitializeComponent();
        }
        public MesHistory(User usernow)
        {
            this.userSearch = usernow;
            InitializeComponent();
        }

        private void btClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dbhistory.DeleteOne(userSearch);
            this.Close();
        }

        private void History_Loaded(object sender, RoutedEventArgs e)
        {
            if (userSearch != null)
            {
                List<Detail> MesList = dbhistory.FindOne(userSearch);
                foreach (Detail i in MesList)
                {
                    if (i.DetType == 0)
                    {
                        
                        string mess = i.DetDatetim.ToLongTimeString().ToString() + " " + i.DetSpeak + ":" + i.DetCont;
                        Lv_History.Items.Add(new Display(0, mess, null));
                    }
                    else if (i.DetType == 1)
                    {
                        string mess = i.DetDatetim.ToLongTimeString().ToString() + " " + i.DetSpeak + ":文件：" + i.DetCont;
                        Lv_History.Items.Add(new Display(1, mess, i.DetCont));
                    }
                    else if (i.DetType == 2)
                    {
                        string mess = i.DetDatetim.ToLongTimeString().ToString() + " " + i.DetSpeak + ":截图：" + i.DetCont;
                        Lv_History.Items.Add(new Display(2, mess, i.DetCont));
                    }
                }
            }
        }

        private void Lv_History_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Display DetMess = Lv_History.SelectedItem as Display;
            if (DetMess.Type == 1)
            {
                string path = DetMess.Path.Substring(0,DetMess.Path.LastIndexOf("\\"));
                System.Diagnostics.Process.Start("explorer.exe", path);//调用explorer资源管理器打开
            }
            else if (DetMess.Type == 2)
            {
                DisplayPic showPicForm = new DisplayPic(DetMess.Path);
                showPicForm.Show();
            }
        }
    }
}
