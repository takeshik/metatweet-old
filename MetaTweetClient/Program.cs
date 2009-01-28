using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XSpect.MetaTweet.Clients
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            MetaTweet.Test.Program.Main(null);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
