using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditSpareParameterForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private SpareParameter _item;

        private readonly Spare _spare;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditSpareParameterForm(SpareParameter item)
        {
            _item = item;
            _spare = item.Spare;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditSpareParameterForm(Spare spare)
        {
            _spare = spare;
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textBoxParameter.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxValue.Text))
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
                    _item.Parameter = textBoxParameter.Text;
                    _item.ParameterVakue = textBoxValue.Text;

                    _item.Spare = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new SpareParameter()
                    {
                        Spare_ID = _spare.Spare_ID,
                        Parameter = textBoxParameter.Text,
                        ParameterVakue = textBoxValue.Text
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
                textBoxParameter.Text = _item.Parameter;
                textBoxValue.Text = _item.ParameterVakue;
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
