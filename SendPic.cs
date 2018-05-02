using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BS
{
    public partial class SendPic : Form
    {
        public string picPathString;
        Image baseImage = null;
        Image AdjustImage = null;
        Cursor myCursor = new Cursor(@"C:\WINDOWS\Cursors\cross_r.cur"); //自定义鼠标 

        public IPEndPoint ipsent;
        string name1 = null;
        string name2 = null;

        public SendPic()
        {
            InitializeComponent();
        }
        public SendPic(string _name1, string _name2)
        {
            this.name1 = _name1;
            this.name2 = _name2;
            InitializeComponent();
        }

        private void SendPic_Load(object sender, EventArgs e)
        {
            baseImage = Image.FromFile(@picPathString);
            picBox_showPic.SizeMode = PictureBoxSizeMode.Zoom;
            picBox_showPic.Image = baseImage;
            AdjustImage = picBox_showPic.Image;
        }

        private void bt_CancelPic_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_SendPic_Click(object sender, EventArgs e)
        {
            ClassSendPic sendFile = new ClassSendPic(picPathString,name1,name2);
            ThreadPool.QueueUserWorkItem(sendFile.SendFile, ipsent);
            this.Close();
        }

        private void picBox_showPic_MouseMove(object sender, MouseEventArgs e)
        {
            //局部图片的放大
            /*try
            {
                Cursor.Current = myCursor;//定义鼠标
                Graphics graphics = picBox_showPic.CreateGraphics();//创建picBox_showPic控件的Graphics对象
                //声明两个Rectangle对象，分别用来指定要放大的区域和放大后的区域
                Rectangle sourceRectangle = new Rectangle(e.X-10,e.Y-10,20,20); //要放大的区域
                Rectangle destRectangle = new Rectangle(e.X - 20, e.Y - 20, 40, 40); //放大后的区域
                //调用DrawRectangle方法对选定区域进行重新绘制，以放大该部分
                Image New = DrawBlowUp((Image)AdjustImage.Clone(), destRectangle, sourceRectangle);
                graphics.DrawImage(New,0,0);
            }
            catch { }
             */
        }
        private Image DrawBlowUp(Image back, Rectangle source, Rectangle dest)
        {
            Graphics Painter = Graphics.FromImage(back);
            Painter.DrawImage(back, source, dest, GraphicsUnit.Pixel);
            return back;
        }
    }
}
