using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditWorkForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Work _item;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditWorkForm(Work item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditWorkForm()
        {
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textBoxWorkName.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (!decimal.TryParse(textBoxPrice.Text, out _))
            {
                sb.AppendLine($"Не верно заполнено поле {label2.Text}!");
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
                    _item.WorkName = textBoxWorkName.Text;
                    _item.Price = decimal.Parse(textBoxPrice.Text);
                    _item.Description = textBoxDescription.Text;

                    _repository.Update(_item);
                }
                else
                {
                    _item = new Work()
                    {
                        WorkName = textBoxWorkName.Text,
                        Price = decimal.Parse(textBoxPrice.Text),
                        Description = textBoxDescription.Text
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
                textBoxWorkName.Text = _item.WorkName;
                textBoxPrice.Text = _item.Price.ToString("f2");
                textBoxDescription.Text = _item.Description;
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
