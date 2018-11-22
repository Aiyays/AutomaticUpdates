using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace UpgradeClient
{
    public class ControlSave
    {

        public void Test(string jsonData)
        {

        }

        /// <summary>
        /// 更新数据存到本地
        /// </summary>
        /// <param name="jsonData">{"Name": "","Data": {},"Url": "地址"}</param>
        public void SaveLocal(ControlReflex.PassModel model)
        {
            JObject obj = JObject.Parse(model.JosnData);
            try
            {
                //需要存储的数据
                byte[] btData = Encoding.UTF8.GetBytes(obj["Data"].ToString());

                //应该存储的地址
                string url = obj["Url"].ToString();

                //数据备份
                if (File.Exists(url))
                    CommonTool.TheBackup(url);

                #region 拆分判断是否地址存在 ->不存在则创建
                string[] str = url.Split('\\');
                str[str.Length - 1] = null;
                string path = string.Join(@"\", str);

                //判断文件夹是否存在  
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                #endregion

                //本地写入
                File.WriteAllBytes(url, btData);
               
            }
            catch (Exception ex )
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息{1}\r\n接受到的JsonData{2}",DateTime.Now,ex.Message,model.JosnData));
                model.MainForm.Close_This();
            }
        }

        /// <summary>
        /// 是否需要更新
        /// </summary>
        /// <param name="jsonData"></param>
        public void ISUpdate(ControlReflex.PassModel model)
        {
            JObject obj = JObject.Parse(model.JosnData);
            try
            {
                //不需要更新
                if (obj["Data"].ToString().Equals("true"))
                {
                    MessageBox.Show("当前版本已经是最新版本-_-");
                    model.MainForm.Close_This();
                }
                else
                {
                    //需要更新
                    model.MainForm.InUpdate();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息{1}\r\n接受到的JsonData{2}", DateTime.Now, ex.Message, model.JosnData));
                model.MainForm.Close_This();
            }
        }

        /// <summary>
        /// 更新完成 关闭本次更新程序
        /// </summary>
        /// <param name="model"></param>
        public void Close_This(ControlReflex.PassModel model)
        { 
            JObject obj = JObject.Parse(model.JosnData);
            try
            {
                ConfigurationManager.AppSettings.Set("VersionNumber", obj["Data"].ToString());
            }
            catch ( Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息{1}\r\n接受到的JsonData{2}", DateTime.Now, ex.Message, model.JosnData));
            }
            model.MainForm.Close_This();
        }
       
    }
}
