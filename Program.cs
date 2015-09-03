using System;
using System.Threading;
using System.Windows.Forms;
using Xnlab.SQLMon.Logic;
using Monitor = Xnlab.SQLMon.UI.Monitor;

namespace Xnlab.SQLMon
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += OnApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            Application.Run(new Monitor());
        }

        private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        private static void HandleException(Exception e)
        {
            MessageBox.Show(e.Message, Settings.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
