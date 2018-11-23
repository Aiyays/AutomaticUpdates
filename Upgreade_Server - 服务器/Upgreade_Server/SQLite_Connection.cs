using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgreade_Server
{

    public class SQLite_Connection
    {
        public static string SqlCon = @"Data Source= " + AppDomain.CurrentDomain.BaseDirectory.ToString()+ @"\data\sjConfig.db" + ";Version=3;password=";
        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static DataSet QueryTab(string sqlStr)
        {

            using (SQLiteConnection con = new SQLiteConnection(SqlCon))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sqlStr, con))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        con.Open();
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(ds);
                        return ds;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

    }

}
