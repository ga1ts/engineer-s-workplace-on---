using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditClientForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Client _item;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditClientForm(Client item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditClientForm()
        {
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
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
                    _item.FName = textBoxFName.Text;
                    _item.ClientName = textBoxName.Text;
                    _item.LName = textBoxLName.Text;
                    // если поле телефон полностью заполнено - добавляем, в противном случае нулл
                    _item.Phone = maskedTextBoxPhone.MaskFull ? maskedTextBoxPhone.Text : null;
                    // если в поле емейл пусто вставляем нулл
                    _item.Email = string.IsNullOrWhiteSpace(textBoxEMail.Text) ? null : textBoxEMail.Text;
                    _item.Passport = string.IsNullOrWhiteSpace(textBoxPassport.Text) ? null : textBoxPassport.Text;

                    _repository.Update(_item);
                }
                else
                {
                    _item = new Client()
                    {
                        FName = textBoxFName.Text,
                        ClientName = textBoxName.Text,
                        LName = textBoxLName.Text,
                        // если поле телефон полностью заполнено - добавляем, в противном случае нулл
                        Phone = maskedTextBoxPhone.MaskFull ? maskedTextBoxPhone.Text : null,
                        // если в поле емейл пусто вставляем нулл
                        Email = string.IsNullOrWhiteSpace(textBoxEMail.Text) ? null : textBoxEMail.Text,
                        Passport = string.IsNullOrWhiteSpace(textBoxPassport.Text) ? null : textBoxPassport.Text
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
            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование";
                
                textBoxFName.Text = _item.FName;
                textBoxName.Text = _item.ClientName;
                textBoxLName.Text = _item.LName;
                maskedTextBoxPhone.Text = _item.Phone;
                textBoxEMail.Text = _item.Email;
                textBoxPassport.Text = _item.Passport;
               
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
