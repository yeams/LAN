using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    class FileNameHead
    {
        public static byte[] GetBytes(string FileName)//传入的filename是不包含路径的，eg. hub.txt
        {
            byte[] fileNameByte = Encoding.Unicode.GetBytes(FileName);
            byte[] fileNameLengthForVauleByte = Encoding.Unicode.GetBytes(fileNameByte.Length.ToString());
            byte[] fileAttributeByte = new byte[fileNameByte.Length + fileNameLengthForVauleByte.Length];

            fileNameLengthForVauleByte.CopyTo(fileAttributeByte,0);
            fileNameByte.CopyTo(fileAttributeByte,fileNameLengthForVauleByte.Length);

            return fileAttributeByte;
        }

        public static int GetFileNameByteLength(byte[] FileNameHeadByte)
        {
            byte[] fileNameLengthForVauleByte = new byte[4];
            Array.Copy(FileNameHeadByte, 8, fileNameLengthForVauleByte,0,4);

            string FileNameByteLength = Encoding.Unicode.GetString(fileNameLengthForVauleByte);
            int FileNameByteLengthNum = Convert.ToInt32(FileNameByteLength);

            return FileNameByteLengthNum;
        }

        public static string GetFileName(byte[] FileNameHeadByte, int FileNameByteLength)//返回的filename，应类似 hub.txt
        {
            byte[] fileNameBytes = new byte[FileNameByteLength];
            Array.Copy(FileNameHeadByte, 12, fileNameBytes, 0, FileNameByteLength);

            string FileName = Encoding.Unicode.GetString(fileNameBytes);
            return FileName;
        }
    }
}
