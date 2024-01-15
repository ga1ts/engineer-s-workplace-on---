using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditEmployeeForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Employee _item;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditEmployeeForm(Employee item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditEmployeeForm()
        {
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (comboBoxPosts.SelectedItem == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxFName.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label2.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label3.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxLName.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label4.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label7.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label8.Text}!");
            }
            if (sb.Length != 0)
            {
                MessageBox.Show(sb.ToString(), "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void buttonAddEdit_Click(object sender, EventArgs e)
        {
            if (!Check()) return;
            try
            {
                if (_edit)
                {
                    _item.Post_ID = (comboBoxPosts.SelectedItem as Post).Post_ID;
                    _item.FName = textBoxFName.Text;
                    _item.EmpName = textBoxName.Text;
                    _item.LName = textBoxLName.Text;
                    // если поле телефон полностью заполнено - добавляем, в противном случае нулл
                    _item.Phone = maskedTextBoxPhone.MaskFull?maskedTextBoxPhone.Text:null;
                    // если в поле емейл пусто вставляем нулл
                    _item.Email = string.IsNullOrWhiteSpace(textBoxEMail.Text) ? null : textBoxEMail.Text;
                    _item.Login = textBoxLogin.Text;
                    _item.Password = textBoxPassword.Text;

                    _item.Post = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new Employee()
                    {
                        Post_ID = (comboBoxPosts.SelectedItem as Post).Post_ID,
                        FName = textBoxFName.Text,
                        EmpName = textBoxName.Text,
                        LName = textBoxLName.Text,
                        Phone = maskedTextBoxPhone.MaskFull ? maskedTextBoxPhone.Text : null,
                        Email = string.IsNullOrWhiteSpace(textBoxEMail.Text) ? null : textBoxEMail.Text,
                        Login = textBoxLogin.Text,
                        Password = textBoxPassword.Text
                    };
                    _repository.Add(_item);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }


        private void Form_Load(object sender, EventArgs e)
        {
            comboBoxPosts.DataSource = _repository.GetEntityes<Post>();
            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование";
                comboBoxPosts.SelectedItem = _item.Post;
                textBoxFName.Text = _item.FName;
                textBoxName.Text = _item.EmpName;
                textBoxLName.Text = _item.LName;
                maskedTextBoxPhone.Text = _item.Phone;
                textBoxEMail.Text = _item.Email;
                textBoxLogin.Text = _item.Login;
                textBoxPassword.Text = _item.Password;
                buttonAddEdit.Image = Resources.save;
            }
            else
            {
                buttonAddEdit.Text = "Добавить";
                Text = "Добавление";
                buttonAddEdit.Image = Resources.add_btn;
            }
        }
    }
}
