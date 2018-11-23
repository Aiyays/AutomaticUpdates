using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Upgreade_Server
{
    /// <summary>
    /// 向客户端发送消息
    /// </summary>
    public static class SendMessage
    {
        #region 公用隐藏方法

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        private static void SendMsg(Socket socket, string result)
        {
            byte[] bt = Encoding.UTF8.GetBytes(result);
            try
            {
                socket.Send(bt, bt.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息:{1}\r\n", DateTime.Now, ex.Message));
            }
        }
        /// <summary>
        /// 统一更新套装
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="url"></param>
        private static void UnifiedSend(Socket socket, string name, string data, string url = "")
        {
            JObject obj = new JObject() {
                { "Name",name},
                { "Data",data},
                { "Url",url}
            };
            SendMsg(socket, string.Format("\u0002{0}{1}", obj.ToString(), "\u0003"));
           

        }

        #endregion

        /// <summary>
        /// 是否可以更新
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"> true  false   {"Name": "","Data": {},"Url": "地址"}</param>
        public static void SendIsUpdate(Socket socket, string data) => UnifiedSend(socket, "ISUpdate", data);

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">数据</param>
        /// <param name="url">地址</param>
        public static void SendSaveLocal(Socket socket, string data, string url) => UnifiedSend(socket, "SaveLocal", data, url);

        /// <summary>
        /// 更新完成
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">当前版本号</param>
        public static void Close_This(Socket socket, string data) => UnifiedSend(socket, "Close_This", data);



    }
}
