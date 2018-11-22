using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UpgradeClient
{
    public partial class RequestLatest : UserControl
    {
        public RequestLatest()
        {
            InitializeComponent();

        }


        public void PointMovement()
        {
            while (true)
            {
                this.Invoke(new Action<object>((delegate
                {

                    labDian.Text += ".";
                    if (labDian.Text.Length > 9)
                    {
                        labDian.Text = ".";
                    }
                    Thread.Sleep(1000);

                })), "");
            }

        }

        private void RequestLatest_Load(object sender, EventArgs e)
        {
          
            //Thread th = new Thread(PointMovement)
            //{
            //    IsBackground = true
            //};
            //th.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointMovement();
        }
 
    }
}
