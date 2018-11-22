using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UpgradeClient
{
    public partial class Client : Form
    {
        /// <summary>
        /// Socket客户端
        /// </summary>
        public SocketClient SocketClient = null;
        /// <summary>
        /// 原始版本
        /// </summary>
        public string VersionNumber = "";

        /// <summary>
        /// 构造
        /// </summary>
        public Client()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_Load(object sender, EventArgs e)
        {
            beingUpdated.Visible = false;
            requestLatest.Visible = true;
            //版本号赋值
            VersionNumber = ConfigurationManager.AppSettings["VersionNumber"];
            this.FormClosed += (object sen, FormClosedEventArgs d) =>
            {
                try
                {
                    //判断版本号是否改变
                    if (VersionNumber.Equals(ConfigurationManager.AppSettings["VersionNumber"]))
                    {
                        //数据回滚
                       CommonTool.DataRollback();
                    }
                    //判断是否更新成功  更新失败则回滚原来数据   
                    string url = ConfigurationManager.AppSettings["OpenProgramUrl"].Trim('\\') + @"\" + ConfigurationManager.AppSettings["ProgramName"];
                    //判断程序是否存在
                    if (File.Exists(url))
                        System.Diagnostics.Process.Start("explorer.exe", url);
                    else
                        MessageBox.Show(null,"配置文件中程序路径有误","紧急提示");
                }
                catch (Exception ex)
                {
                    Log.WriteLine(string.Format("时间：{0}\r\n异常信息{1}", DateTime.Now, ex.Message));
                    MessageBox.Show(ex.Message);
                }
            };
            OpenSocketClient();
        }

        /// <summary>
        /// 判断是否更新
        /// </summary>
        public void ControlGetIs()
        {

            string sendMsg = new JObject() {
                { "Name","IsUpgrade"},
                //程序
                { "SerialNumber",ConfigurationManager.AppSettings["ProgramInfo"]},
                //版本号
                { "VersionNumber",ConfigurationManager.AppSettings["VersionNumber"]},
                //根目录
                { "RootDirectory",ConfigurationManager.AppSettings["OpenProgramUrl"]},
            }.ToString();
            try
            {
                ///发送请求
                SocketClient.SendMessage("\u0002" + sendMsg + "\u0003");
            }
            catch (Exception ex)
            {
                Thread.Sleep(5000);
                MessageBox.Show(null,"更新失败,远程服务器未开启\r\n" + ex.Message,"提示");
                Close_This();
            }
        }

        /// <summary>
        /// 关闭本程序
        /// </summary>
        public void Close_This()
        {
            this.Invoke(new Action<string>((delegate
            {
                //关闭
                this.Close();
            })), "");

        }

        /// <summary>
        /// 正在更新
        /// </summary>
        public void InUpdate()
        {
            this.Invoke(new Action<string>((delegate
            {
                beingUpdated.Visible = true;
                requestLatest.Visible = false;
            })), "");
        }

        /// <summary>
        /// 开启Socket客户端
        /// </summary>
        public void OpenSocketClient()
        {
            ///初始化程序
            SocketClient = new SocketClient(ConfigurationManager.AppSettings["IP"], int.Parse(ConfigurationManager.AppSettings["PORT"]), RecvStr)
            {
                ///数据头
                Head = "\u0002",
                ///数据尾
                Tail = "\u0003"
            };
            Thread A = new Thread(ControlGetIs)
            {
                IsBackground = true
            };
            A.Start();
        }

 

        /// <summary>
        /// 接受到一条完整的数据
        /// </summary>
        /// <param name="msg"></param>
        public void RecvStr(string msg)
        {
            try
            {
                //去掉数据头数据尾
                msg = msg.TrimStart('\u0002').TrimEnd('\u0003');
                //数据面向对象
                JObject obj = JObject.Parse(msg);
                //反射控制Modle
                ControlReflex.ReflexModel model = new ControlReflex.ReflexModel()
                {
                    ClassName = "UpgradeClient.ControlSave",
                    DomainName = @"./UpgradeClient.exe",
                    MethodName = obj["Name"].ToString(),
                    JsonData = new ControlReflex.PassModel()
                    {
                        JosnData = msg,
                        MainForm = this
                    }
                };
                //控制到相应的类
                ControlReflex.Reflex(model);
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息:{1}\r\n", DateTime.Now, ex.Message));
            }
        }

        /// <summary>
        /// 点击关闭所有升级程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e) => Close_This();
    }
}
