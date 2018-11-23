using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace UpgradeClient
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class CommonTool
    {

        /// <summary>
        /// 数据备份
        /// </summary>
        public static void TheBackup(string url)
        {
            //判断文件是否存在
            bool isFileExists = File.Exists(url);
            //拿到根目录
            string this_Path = ConfigurationManager.AppSettings["OpenProgramUrl"].Trim('\\');
            //存在即备份
            if (isFileExists)
            {
                //拿到数据名
                string name = url.Replace(this_Path, "").Trim('\\');
                name = string.Format(@"{0}\{1}\{2}", this_Path, "数据备份", name);
                //拿到本地地址
                byte[] bt = File.ReadAllBytes(url);
                if (!Directory.Exists(name.Substring(0, name.LastIndexOf(@"\"))))
                    Directory.CreateDirectory(name.Substring(0, name.LastIndexOf(@"\")));
                if (!Directory.Exists(this_Path + @"\数据备份"))
                    Directory.CreateDirectory(this_Path + @"\数据备份");
                    
                File.WriteAllBytes(name, bt);
            }
        }


        /// <summary>
        /// 数据回滚
        /// </summary>
        public static void DataRollback()
        {
            string url = ConfigurationManager.AppSettings["OpenProgramUrl"].Trim('\\') + @"\数据备份";
            //判断改地址是存在
            //if (Directory.Exists(url))
            //{
            //    //取出所有需要回滚的子文件
            //    string[] strArr = Directory.GetFiles(url, "*", SearchOption.AllDirectories);
            //    foreach (var item in strArr)
            //    {
            //        //取出文件流
            //        byte[] bt = File.ReadAllBytes(item);
            //        //回滚以后的地址
            //        string strUrl = item.Replace(@"\数据备份", "");
            //        //文件流的写入
            //        File.WriteAllBytes(strUrl, bt);
            //    }
            //    Directory.Move(url, url + DateTime.Now.ToString("yyyyMMdd")+new Random().Next(0,100));
            //}
        }


    }
}
