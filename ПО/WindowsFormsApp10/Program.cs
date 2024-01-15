using System;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms;

namespace RemontKt
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (Repository.FirstStart())
            {
                MessageBox.Show("Первый запуск программы. Добавьте сотрудника и перезагрузите программу!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                new MainForm().ShowDialog();
            }

            new LoginForm().ShowDialog();

            if (Repository.LoginedEmployee!=null)
            {
                Application.Run(new MainForm());
            }
        }
    }
}
