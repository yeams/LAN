using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    public class Display
    {
        public int Type { get; set; }//0：文本信息  1：文件信息  2：截图信息
        public string Message { get; set; }//DetType为0时，存放文本信息的内容；DetType为1或2时，存放"点击查看详细信息"
        public string Path { get; set; }//DetType为0时，为空；DetType为1或2时，存放文件或截图的路径
        public Display(int type, string message, string path)
        {
            this.Type = type;
            this.Message = message;
            this.Path = path;
        }
    }
}