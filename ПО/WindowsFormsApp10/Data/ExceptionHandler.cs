using System;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Windows.Forms;

namespace RemontKt.Data
{
    public class ExceptionHandler
    {
        public static void HandleException(Exception exception)
        {
            switch (exception.GetType().Name)
            {
                case nameof(DbUpdateException):
                    HandleDbException(exception);
                    break;
                default:
                    MessageBox.Show($"Тип исключения: {exception.GetType()} \n Сообщение: {exception.Message}");
                    break;
            }
        }



        /// <summary>
        /// Метод обработки исключений
        /// </summary>
        private static void HandleDbException(Exception exception)
        {
            var sb = new StringBuilder();
            
            var innerEx = exception.InnerException ?? exception;
            var test = innerEx.GetType();
            while (true)
            {
                if (innerEx.InnerException == null)
                {
                    break;
                }
                innerEx = innerEx.InnerException;
            }
            MessageBox.Show($"{innerEx.Message} \n {sb} \n Исключение получено в методе: {exception.TargetSite}",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
