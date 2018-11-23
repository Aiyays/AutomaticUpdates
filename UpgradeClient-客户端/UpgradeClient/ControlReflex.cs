using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace UpgradeClient
{
    /// <summary>
    /// 控制反射
    /// </summary>
    public class ControlReflex
    {

        /// <summary>
        /// 更新程序接受数据的地址
        /// </summary>
        public class ReflexModel
        {
            /// <summary>
            /// 文件地址
            /// </summary>
            public string DomainName { get; set; }

            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            /// 方法名
            /// </summary>
            public string MethodName { get; set; }

            /// <summary>
            /// Json参数
            /// </summary>
            public PassModel JsonData { get; set; }


        }

        /// <summary>
        /// 反射过程中传递参数
        /// </summary>
        public class PassModel {

            /// <summary>
            /// 接收到的Json数据
            /// </summary>
            public string JsonData { get; set; }

            public Client MainForm { get; set; }
           
        }

        /// <summary>
        /// 反射调用方法
        /// </summary>
        /// <param 实例调用的方法="model"></param>
        public static void Reflex( ReflexModel model)
        {
            try
            {
                Assembly asmb = Assembly.LoadFrom(model.DomainName);
                Type type = asmb.GetType(model.ClassName);
                if (type != null)
                {
                    object obj = System.Activator.CreateInstance(type);
                    MethodInfo method = type.GetMethod(model.MethodName);
                    try
                    {
                        method.Invoke(obj, new object[] { model.JsonData });
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine("未找到接收的方法:" + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(string.Format("时间:{0}\r\n异常信息{1}",DateTime.Now,ex.Message));
                //记录到数据库
                //CommonTool.SqlServer_Control.UpdateErro(model.Parameter.ZNJTYS_Hid, model.Parameter.Data, ex.Message, model.Parameter.TableName);
            }
        }
    }
}
