using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UpgradeClient
{
    public class SocketClient
    {
        /// <summary>
        /// 客户端Socket
        /// </summary>
        public Socket clientSocket = null;

        /// <summary>
        /// 缓冲区
        /// </summary>
        public byte[] bt = new byte[1024 * 1024];

        /// <summary>
        /// 头部
        /// </summary>
        public string Head = null;
        /// <summary>
        /// 尾部
        /// </summary>
        public string Tail = null;

        public Action<string> RecvData = null;

        /// <summary>
        /// 缓冲区
        /// </summary>
        public StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="recv"></param>
        public SocketClient(string ip, int port,Action<string> recv)
        {
            try
            {
                //初始化信息
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //连接到服务器
                clientSocket.Connect(ip, port);
                //绑定数据接受
                RecvData = recv;
            }
            catch (Exception ex)
            {
                //连接发生错误
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息:{1}\r\n",DateTime.Now,ex.Message));
                return;
            }
            //线程访问
            Queue<string> v = new Queue<string>();
            Thread thread = new Thread(Recv)
            {
                IsBackground = true
            };
            thread.Start();
        }


        /// <summary>
        /// 数据接受
        /// </summary>
        public void Recv()
        {
            //数据长度
            int lenth = clientSocket.Receive(bt);
            if (lenth > 0)
            {
                //接受到的数据
                string msg = Encoding.UTF8.GetString(bt, 0, lenth);
                //处理数据
                if (sb.Length.Equals(0))
                    sb.Append(msg.Contains(Head) ? msg.Substring(msg.IndexOf(Head), msg.Length - msg.IndexOf(Head)) : "");
                else
                    sb.Append(msg);
            }
            //判断是否包含命令尾
            if (sb.ToString().Contains(Tail))
                ProcessingData();

        }

        /// <summary>
        ///  加工处理数据
        /// </summary>
        public void ProcessingData()
        {
            //取出处理所有的数据
            while (sb.ToString().Contains(Tail))
            {
                string info = sb.ToString().Substring(0, sb.ToString().IndexOf(Tail) + Tail.Length);

                //解析数据
                //处理数据
                Thread thread = new Thread(() =>
                {
                    RecvData(info);
                })
                {
                    IsBackground = true
                };
                thread.Start();
                //处理烂尾数据
                sb.Remove(0, sb.ToString().IndexOf(Tail) + Tail.Length);
                if ((!sb.ToString().Contains(Head)) && (!sb.Length.Equals(0)))
                    sb = new StringBuilder();
                else if(!sb.Length.Equals(0))
                    sb.Remove(0, sb.ToString().IndexOf(Head) - 1);
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            byte[] bt = Encoding.UTF8.GetBytes(msg) ;

            clientSocket.Send(bt,bt.Length,SocketFlags.None);

        }



    }
}
