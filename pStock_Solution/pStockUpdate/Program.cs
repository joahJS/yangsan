using System;
using System.Windows.Forms;

namespace pStockUpdate
{
    internal static class Program
    {
        /// <summary>
        ///  프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Version_Check_Form());
        }
    }
}
