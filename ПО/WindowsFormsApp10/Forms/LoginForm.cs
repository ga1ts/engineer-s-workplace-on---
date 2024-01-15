using System;
using System.Windows.Forms;
using RemontKt.Data;


namespace RemontKt.Forms
{
    public partial class LoginForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login()
        {
            try
            {
                _repository.Login(textBoxLogin.Text, textBoxPassword.Text);
                if (Repository.LoginedEmployee!=null)
                {
                    Close();
                }
                else
                {
                    MessageBox.Show(" Пользователь с таким логином и паролем не найден!", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    textBoxPassword.Clear();
                }
            }
            catch (Exception e)
            {
               ExceptionHandler.HandleException(e);
            }
            
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
           Login();
        }

       private void LoginForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
               Login(); 
            }
        }
    }
}
