using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Upgreade_Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlSave
    {

        /// <summary>
        /// 检查是否更新
        /// </summary>
        /// <param name="model"></param>
        public void RequestISUpdata(ControlReflex.PassModel model)
        {
            JObject obj = JObject.Parse(model.JsonData);
            string date = null;
            try
            {

                //程序名称
                string name = obj["SerialNumber"].ToString();
                //版本号
                string versionNumber = obj["VersionNumber"].ToString();
                //根目录
                string url = obj["RootDirectory"].ToString();
                DataTable data = ControlSQLMethod.QueryIsVMAndPro(name, versionNumber);
                date = data.Rows.Count.Equals(0) ? "true" : "false";
                new Task(() =>
                {
                    UpdateStart(data, model.Socket, url, versionNumber);
                }).Start();
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息:{1}", DateTime.Now, ex.Message));
            }
            SendMessage.SendIsUpdate(model.Socket, date is null ? "false" : date);
        }
        /// <summary>
        /// 循环遍历
        /// </summary>
        /// <param name="data"></param>
        /// <param name="socket"></param>
        /// <param name="url"></param>
        private void UpdateStart(DataTable data, Socket socket, string url, string versionNumber)
        {
            Thread.Sleep(1000);
            string[] strArr = url.Split('\\');
            //拿到首要数据
            string urlName = strArr[strArr.Length - 1];
            try
            {
                //循环遍历新的版本
                foreach (DataRow item in data.Rows)
                {
                    //取出文件夹下所有的文件
                    string[] fileArr = Directory.GetFiles(item["LocalAddress"].ToString(), "*", SearchOption.AllDirectories);
                    foreach (string ite in fileArr)
                    {
                        string path = url;
                        if (!ite.Contains(urlName))
                            continue;
                        //地址
                        int i = ite.IndexOf(urlName) + urlName.Length;
                        //回传地址
                        path += ite.Substring(i, ite.Length - i);
                        byte[] bt = File.ReadAllBytes(ite);
                        //发送更新数据
                        SendMessage.SendSaveLocal(socket, JsonConvert.SerializeObject(bt), path);
                        Thread.Sleep(500);
                    }
                    versionNumber = item["Nuber"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息:{1}", DateTime.Now, ex.Message));
            }
            SendMessage.Close_This(socket, versionNumber);
        }
    }
}
