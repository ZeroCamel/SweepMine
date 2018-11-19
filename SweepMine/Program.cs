using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SweepMine
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //控件可视化
            Application.EnableVisualStyles();
            //.NET2.0 新增
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}