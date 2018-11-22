using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UpgradeClient
{
    public partial class BeingUpdated : UserControl
    {
        public BeingUpdated()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="count"></param>
        /// <param name="total"></param>
        public void UpdateRatio(string count,string total)
        {
            this.Invoke(new Action<string>((delegate {
                //现在的次数
                labCount.Text = count;
                //总数
                labTaiol.Text = total;

            })),count);
        }
    }
}
