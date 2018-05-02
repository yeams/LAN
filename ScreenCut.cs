using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BS
{
    public partial class ScreenCut : Form
    {
        public IPEndPoint ipsent;
        string name1;
        string name2;

        public ScreenCut()
        {
            InitializeComponent();
        }
        public ScreenCut(string _name1, string _name2)
        {
            this.name1 = _name1;
            this.name2 = _name2;
            InitializeComponent();
        }

        public Image ig;
        private Graphics MainPainter;//负责将图片绘制在屏幕上
        private Pen pen;
        private bool isDowned;
        private Image baseImage;
        private Rectangle Rect;
        private bool RectReady;
        private Point downPoint;
        int tmpx;
        int tmpy;

        private void ScreenCut_Load(object sender, EventArgs e)
        {
            this.ig = GetScreenImage();
            this.BackgroundImage = ig;//将截屏设置为窗体背景
            this.WindowState = FormWindowState.Maximized;//最大化窗体
            MainPainter = this.CreateGraphics();//创建Graphics对象
            pen = new Pen(Brushes.Red);
            isDowned = false;//判断鼠标是否按下
            baseImage = this.BackgroundImage;//获取捕获的屏幕图像
            Rect = new Rectangle();//创建Rectangle图像
            RectReady = false;//是否出现边框

            MessageBox.Show("已经截图，退出请按Esc", "提示");
        }

        private void ScreenCut_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void ScreenCut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//落下的是左键
            {
                isDowned = true;
                if (RectReady == true)//如果边框已经出现了，则记录鼠标位置
                {
                    tmpx = e.X;
                    tmpy = e.Y;
                }
                else
                {
                    Rect.X = e.X;
                    Rect.Y = e.Y;
                    downPoint = new Point(e.X, e.Y);//边框没有出现，记录鼠标下落的点
                }
            }
            if (e.Button == MouseButtons.Right)//落下的是右键
            {
                if (RectReady != true)//没有边框，则退出
                {
                    this.Close();
                    return;
                }
                this.CreateGraphics().DrawImage(baseImage, 0, 0);//在指定位置绘制指定的图像
                RectReady = false;
            }
        }

        private void ScreenCut_MouseMove(object sender, MouseEventArgs e)
        {
            if (RectReady == true)//如果已经有边框了，则移动边框
            {
                if (Rect.Contains(e.X, e.Y))
                {
                    if (isDowned == true)
                    {
                        //和上一次的位置比较获取偏移量
                        Rect.X = Rect.X + e.X - tmpx;
                        Rect.Y = Rect.Y + e.Y - tmpy;
                        //记录现在的位置
                        tmpx = e.X;
                        tmpy = e.Y;
                        MoveRect((Image)baseImage.Clone(), Rect);
                    }
                }
            }
            else//还没有边框
            {
                if (isDowned == true)
                {
                    Image New = DrawScreen((Image)baseImage.Clone(), e.X, e.Y);
                    MainPainter.DrawImage(New, 0, 0);
                    New.Dispose();
                }
            }
        }

        private void ScreenCut_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDowned = false;
                if (Rect.Width > 0 && Rect.Height > 0)
                    RectReady = true;
            }
        }

        private void ScreenCut_DoubleClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left && Rect.Contains(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y))
            {
                Image memory = new Bitmap(Rect.Width - 1, Rect.Height - 1);
                Graphics g = Graphics.FromImage(memory);
                g.CopyFromScreen(Rect.X + 1, Rect.Y + 24, 0, 0, Rect.Size);

                string path = Environment.CurrentDirectory + "\\pic";
                if (!Directory.Exists(path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    directoryInfo.Create();
                }

                string picPathName = path + "\\" + DateTime.Now.ToFileTime().ToString() + ".png";
                memory.Save(picPathName);
                Clipboard.SetImage(memory);

                SendPic showPic = new SendPic(name1,name2);
                showPic.picPathString = picPathName;
                showPic.ipsent = ipsent;
                showPic.Show();
                this.Close();
            }
        }

        private Image GetScreenImage()
        {
            Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);//用屏幕的大小（以像素为单位）初始化Bitmap 类
            Graphics g = Graphics.FromImage(img);//（Graphics封装DI+绘图图画）从指定Image类创建Graphics
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);//屏幕到 System.Drawing.Graphics 的绘图图面的位块传输.位于源矩形左上角的点,位于目标矩形左上角的点,要传输的区域大小。
            //Screen.AllScreens[0].Bounds.Size 第一个显示屏幕的大小
            return img;
        }
        private void DrawRect(Graphics Painter, int Mouse_x, int Mouse_y)
        {
            int width = 0;
            int heigth = 0;
            //竖直方向
            //移动方向：：上
            if (Mouse_y < downPoint.Y)
            {
                Rect.Y = Mouse_y;
                heigth = downPoint.Y - Mouse_y;
            }
            else//移动方向：：下
            {
                heigth = Mouse_y - downPoint.Y;
            }
            //水平方向
            //移动方向：：左
            if (Mouse_x < downPoint.X)
            {
                Rect.X = Mouse_x;
                width = downPoint.X - Mouse_x;
            }
            else//移动方向：：右
            {
                width = Mouse_x - downPoint.X;
            }
            Rect.Size = new Size(width, heigth);
            Painter.DrawRectangle(pen, Rect);
        }
        private void MoveRect(Image image, Rectangle Rect)
        {
            Graphics Painter = Graphics.FromImage(image);
            Painter.DrawRectangle(pen, Rect.X, Rect.Y, Rect.Width, Rect.Height);
            MainPainter.DrawImage(image, 0, 0);
            image.Dispose();//释放image占用的资源
        }
        private Image DrawScreen(Image back, int Mouse_x, int Mouse_y)
        {
            Graphics Painter = Graphics.FromImage(back);
            DrawRect(Painter, Mouse_x, Mouse_y);
            return back;
        }
    }
}
