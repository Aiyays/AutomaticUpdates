using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgreade_Server
{
    /// <summary>
    /// 控制SQLite数据库
    /// </summary>
    public static class ControlSQLMethod
    {
        #region 查询

        /// <summary>
        /// 根据id和号码取到
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string QuertTest(string msg)
        {
            //msg     ->"血常规-1.1"
            string flag = null;
            string sql = string.Format("select vm.LocalAddress from ProgramInfomation as pi LEFT join   VersionManagement as vm on pi.ID = vm.ProgreamInformation_ID where pi.Name = '{0}' and vm.Nuber = '{1}'", msg.Split('-')[0], msg.Split('-')[1]);
            DataTable ed = SQLite_Connection.QueryTab(sql).Tables[0];
            if (!ed.Rows.Count.Equals(0))
            {
                flag = ed.Rows[0][0].ToString();
            }
            return flag;
        }

        /// <summary>
        /// 查询基础程序配置
        /// </summary>
        public static DataTable QueryConfig()
        {
            string sql = "select * from ProgramInfomation";
            var c = SQLite_Connection.QueryTab(sql);
            return c.Tables[0];
        }

        /// <summary>
        /// 根据ID查询所有的子项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable QueryVM(string id)
        {
            string sql = "select * from VersionManagement where ProgreamInformation_ID='" + id + "'";
            var ed = SQLite_Connection.QueryTab(sql);
            return ed.Tables[0];
        }

        /// <summary>
        /// 查询程序名是否已经存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns>返回true代表可用 false代表不可用</returns>
        public static bool QueryProgramName(string name)
        {
            string sql = "";
            bool flag = false;
            try
            {
                var ed = SQLite_Connection.QueryTab(sql);
                flag = ed.Tables[0].Rows.Count == 0;
            }
            catch (Exception ex)
            {
                Log.WriteLine(DateTime.Now + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// 查询是否存在该程序以及版本号
        /// </summary>
        /// <param name="name">程序名称 </param>
        /// <param name="vm"> 版本号</param>
        /// <returns></returns>
        public static DataTable QueryIsVMAndPro(string name, string vm)
        {
            string sql = string.Format("select * from ProgramInfomation as pi left JOIN VersionManagement as vm on pi.ID=vm.ProgreamInformation_ID where pi.Name='{0}' and vm.Nuber>{1} order by Nuber", name, vm);
            DataTable dataTable = null;
            try
            {
                var ed = SQLite_Connection.QueryTab(sql);
                dataTable = ed.Tables[0];
            }
            catch (Exception ex)
            {
                Log.WriteLine(DateTime.Now + ex.Message);
                dataTable = dataTable is null ? new DataTable() : dataTable;
            }
            return dataTable;
        }




        #endregion

        #region 操作


        #endregion

    }
}
