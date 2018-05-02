using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BS
{
    public partial class DisplayPic : Form
    {
        public string picPath;
        private Image image;
        public DisplayPic()
        {
            InitializeComponent();
        }
        public DisplayPic(string path)
        {
            this.picPath = path;
            InitializeComponent();
        }

        private void DisplayPic_Load(object sender, EventArgs e)
        {
            image = Image.FromFile(@picPath);
            picBox_showPic.Image = image;
            this.Width = image.Width+20;
            this.Height = image.Height+40;
            picBox_showPic.Width = image.Width;
            picBox_showPic.Height = image.Height;
        }
    }
}
