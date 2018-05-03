using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Detail
{
    public int DetId { get; set; }
    public string DetMac { get; set; }
    public string DetSpeak { get; set; }
    public int DetType { get; set; }//0：文本信息  1：文件信息  2：截图信息
    public string DetCont { get; set; }//DetType为0时，存放文本信息的内容；DetType为1或2时，存放文件或截图的路径
    public int DetRead { get; set; }//0：未读 1：已读
    public DateTime DetDatetim { get; set; }
}
