using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Upgreade_Server
{
    public class SocketServer
    {
        /// <summary>
        /// 客户端队列管理
        /// </summary>
        Dictionary<string, Server_ClientModel> dictConn = new Dictionary<string, Server_ClientModel>();

        /// <summary>
        /// 主服务器的ip
        /// </summary>
        public string ip = null;

        /// <summary>
        /// 端口
        /// </summary>
        public int port;
        /// <summary>
        /// 发送接收到的byte字符串
        /// </summary>
        public Action<string,Socket> recv = null;

        /// <summary>
        /// 主线程服务器
        /// </summary>
        Socket listenSocket = null;

        /// <summary>
        /// 监听新连接的客户端
        /// </summary>
        /// <param name="AResult"></param>
        public void ScoketAccept(IAsyncResult AResult)
        {
            Socket serverSocket = (Socket)AResult.AsyncState;
            Socket clientsocket = serverSocket.EndAccept(AResult);
            Server_ClientModel connection = new Server_ClientModel(clientsocket, recv, RemoveClientConnection);
            dictConn.Add(clientsocket.RemoteEndPoint.ToString(), connection);
            //再次监听新的连接
            listenSocket.BeginAccept(new AsyncCallback(ScoketAccept), listenSocket);
            Console.WriteLine("客户端:" + clientsocket.RemoteEndPoint.ToString() + "已连接上服务器");
        }


        /// <summary>
        /// 移除与指定客户端的连接
        /// </summary>
        /// <param name="key">指定客户端的IP和端口</param>
        public void RemoveClientConnection(string key)
        {
            if (dictConn.ContainsKey(key))
            {
                dictConn.Remove(key);
                Console.WriteLine(key + "已和服务器断开连接");
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip"></param>
        public SocketServer(Action<string,Socket> receive, string ip = null)
        {
            this.ip = ip;
            recv = receive;
        }

        /// <summary>
        /// 开启监听程序
        /// </summary>
        /// <param name="port"></param>
        public void Listenin(int port)
        {
            IPAddress serverIP = ip is null ? IPAddress.Any : IPAddress.Parse(ip);
            IPEndPoint endp = new IPEndPoint(serverIP, port);
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endp);
            listenSocket.Listen(180);
            listenSocket.BeginAccept(new AsyncCallback(ScoketAccept), listenSocket);
            Console.WriteLine(string.Format("服务器已经开始监听"));
        }

    }

}
