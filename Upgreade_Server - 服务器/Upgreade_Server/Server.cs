using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgreade_Server
{
    public partial class Server : Form
    {
        /// <summary>
        /// 数据库的程序基本配置表
        /// </summary>
        public static DataTable dataTable = null;

        public Server()
        {
            InitializeComponent();
            ServerOpen();
        }
        SocketServer socketServer = null;

        public void ServerOpen()
        {
            //服务器实例
            socketServer = new SocketServer(Recv);
            //端口
            socketServer.Listenin(10000);



        }
        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        #region 数据处理

        string Key = "\u0002";
        string Value = "\u0003";
        StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 接到消息
        /// </summary>
        /// <param name="msg"></param>
        public void Recv(string msg, Socket socket)
        {
            if (sb.Length.Equals(0))
                sb.Append(msg.Contains(Key) ? msg.Substring(msg.IndexOf(Key), msg.Length - msg.IndexOf(Key)) : "");
            else
                sb.Append(msg);

            if (msg.Contains("\u0003"))
                Handlel(socket);

        }
        /// <summary>
        /// 筛选出一条完整的数据
        /// </summary>
        /// <param name="msg"></param>
        public void Handlel(Socket socket)
        {
            ///循环等待数据上传完成
            while (sb.ToString().Contains(Value))
            {
                string upData = sb.ToString().Substring(sb.ToString().IndexOf(Key), sb.ToString().IndexOf(Value) + 1);
                sb.Remove(sb.ToString().IndexOf(Key), sb.ToString().IndexOf(Value) + 1);
                ///清除坏字符串
                if (!sb.ToString().Contains(Key))
                    sb = new StringBuilder();
                ///处理信息
                ControlMsg(upData, socket);
            }
        }

        #endregion


        /// <summary>
        /// 控制数据
        /// </summary>
        /// <param name="msg"></param>
        public void ControlMsg(string msg, Socket socket)
        {
            msg = msg.Replace("\u0002", "").Replace("\u0003", "");
            try
            {
                JObject obj = JObject.Parse(msg);
                string methodName = obj["Name"].ToString();
                ControlReflex.Reflex(new ControlReflex.ReflexModel() {
                    ClassName= "Upgreade_Server.ControlSave",
                    DomainName= @"./Upgreade_Server.exe",
                    JsonData=new ControlReflex.PassModel() {
                        JsonData=msg,
                        Socket=socket
                    },
                    MethodName=methodName
                });

            }
            catch (Exception ex)
            {

            }
        }





        private void Server_Load(object sender, EventArgs e)
        {
            //string[] ad = File.ReadAllLines("Config.txt", Encoding.GetEncoding("GB2312"));

            //创建一个ToolStripMenuItem菜单，可以文本和图片一并添加
            ToolStripMenuItem tsmi = new ToolStripMenuItem("管理程序");

            foreach (DataRow item in ControlSQLMethod.QueryConfig().Rows)
            {
                ToolStripMenuItem tsmi2 = new ToolStripMenuItem(item["Name"].ToString());
                tsmi2.MouseUp += (object s, MouseEventArgs d) =>
                {

                    listBox1.Items.Clear();
                    ToolStripMenuItem test = new ToolStripMenuItem();
                    foreach (DataRow ite in ControlSQLMethod.QueryVM(item["ID"].ToString()).Rows)
                    {
                        //添加子项
                        listBox1.Items.Add(item["Name"].ToString() + "-" + ite["Nuber"].ToString());
                    }
                };
                //listBox1.Items.Add(tsmi2);
                tsmi.DropDownItems.Add(tsmi2);
            }
            ToolStripMenuItem t = new ToolStripMenuItem("管理程序");
            t.MouseUp += (object s, MouseEventArgs d) =>
            {
                //点击按钮发生的事件
            };
            //listBox1.Items.Add(tsmi2);
            tsmi.DropDownItems.Add(t);
            menuStrip1.Items.Add(tsmi);
        }




        /// <summary>
        /// 此刻选中的检查项目
        /// </summary>
        string listBoxItem = "";

        /// <summary>
        /// 选中双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var c = ((System.Windows.Forms.ListBox)sender).SelectedItem;
            if (!(c is null))//判断是否选中 然后打开相应文件夹
            {
                System.Diagnostics.Process.Start("explorer.exe", ControlSQLMethod.QuertTest(listBoxItem));
            }
        }

        /// <summary>
        /// 路径设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 路径设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(listBoxItem.Equals("")))
            {
                MessageBox.Show("现在地址" + ControlSQLMethod.QuertTest(listBoxItem));
            }
            //var c = ((System.Windows.Forms.ListBox)sender).SelectedItem;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var c = ((System.Windows.Forms.ListBox)sender).SelectedItem;
            if (!(c is null))//
                listBoxItem = c.ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!(listBoxItem.Equals("")))
            {
                System.Diagnostics.Process.Start("explorer.exe", ControlSQLMethod.QuertTest(listBoxItem));
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(listBoxItem.Equals("")))
            {
                System.Diagnostics.Process.Start("explorer.exe", ControlSQLMethod.QuertTest(listBoxItem));
            }
        }

        private void 发布新版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
