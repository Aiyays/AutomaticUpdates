using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// 日志记录
/// </summary>
public class Log
{


    #region 设置上传地址

    /// <summary>
    /// 正常上传的日志
    /// </summary>
    public static string GetOrdinaryPath() => @".\" + @"数据抓取日志记录\" + DateTime.Now.ToString("yyyyMM");


    /// <summary>
    /// 桌面文件生成地址
    /// </summary>
    public static string DeskTorUrl => "";/*ConfigurationManager.AppSettings["desktop"]*/


    /// <summary>
    /// 本地数据存储
    /// </summary>
    /// <returns></returns>
    public static string GetOrdnaryUrl() => @".\" + @"本地数据存储\" + DateTime.Now.ToString("yyyyMM");

    #endregion

    #region 公共方法

    /// <summary>
    /// 判断传入地址是否有文件夹 没有则创建
    /// </summry>
    /// <param name="txtFileSaveDir"></param>
    private static void FileIsExists(string txtFileSaveDir)
    {
        if (!Directory.Exists(txtFileSaveDir))
            Directory.CreateDirectory(txtFileSaveDir);
    }

    /// <summary>
    /// 判断是否存在该文件
    /// </summary>
    /// <returns></returns>
    private static bool DecideExist(string path) => File.Exists(path);

    /// <summary>
    /// 读取一个地址的txt文档，存在按行返回数组不存在返回null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string[] ReadGeneral(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(DateTime.Now + "=> 在Excelopenration类96行捕获异常 异常信息：" + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 向已满数组添加一个新元素
    /// </summary>
    /// <param 已满数组="strList"></param>
    /// <param 新的元素="addstr"></param>
    /// <returns></returns>
    private static string[] Choice(string[] strList, string addstr)
    {
        if (!(strList is null))
        {
            string[] newStr = new string[strList.Length + 1];
            for (int i = 0; i < strList.Length; i++)
                newStr[i] = strList[i];
            newStr[strList.Length] = addstr;
            return newStr;
        }
        return new string[] { addstr };
    }
    
    /// <summary>
    /// 公共写入文件夹类
    /// </summary>
    /// <param name="path"></param>
    /// <param name="accout"></param>
    private static void CommonWrite(string path, string message)
    {
        if (path == "")
            return;
        FileIsExists(path); //判断文件是否存在
        path = path + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";//生成写入地址
        File.WriteAllLines(path, Choice(ReadGeneral(path), message));

    }

    #endregion


    static object generalLock = new object();
    /// <summary>
    /// 数据抓取日志记录
    /// </summary>
    /// <param 抓取的消息="msg"></param>
    public static void WriteLine(string msg)
    {
        try
        {
            lock (generalLock)
            {
                CommonWrite(GetOrdinaryPath(), msg);
                if (DeskTorUrl != "")
                    CommonWrite(DeskTorUrl + @" \抓取日志记录\" + DateTime.Now.ToString("yyyyMM"), msg);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format("时间:{0} \r\n具体位置锁定:{1} \r\n异常信息:{2} \r\n异常位置锁定:{3}\r\n ", DateTime.Now, MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name, ex.Message + ex.InnerException, ex.StackTrace));
        }
    }

    /// <summary>
    /// 存到本地
    /// </summary>
    /// <param name="msg"></param>
    public static void SaveLine(string msg)
    {
        try
        {
            lock (generalLock)
            {
                CommonWrite(GetOrdnaryUrl(), msg);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format("时间:{0} \r\n具体位置锁定:{1} \r\n异常信息:{2} \r\n异常位置锁定:{3}\r\n ", DateTime.Now, MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name, ex.Message + ex.InnerException, ex.StackTrace));
        }
    }
}

 
 
