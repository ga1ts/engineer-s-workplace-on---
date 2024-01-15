using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditTechnicParameterForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private TechnicParameter _item;

        private int _technic_ID;

        private readonly bool _edit;

        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditTechnicParameterForm(TechnicParameter item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditTechnicParameterForm(int technic_ID)
        {
            // при добавлении новых параметров техники в конструкторе передаём ID этой техники
            _technic_ID = technic_ID;
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textBoxParameter.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
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
                    _item.ParameterValue = textBoxParameter.Text;
                    _item.TechnicParameter_ID = (comboBoxParameters.SelectedItem as Parameter).Parameter_ID;

                    _item.Technic = null;
                    _item.Parameter = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new TechnicParameter()
                    {
                        Technic_ID = _technic_ID,
                        ParameterValue = textBoxParameter.Text,
                        Parameter_ID = (comboBoxParameters.SelectedItem as Parameter).Parameter_ID
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
            comboBoxParameters.DataSource = _repository.GetEntityes<Parameter>();
            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование";
                textBoxParameter.Text = _item.ParameterValue;
                comboBoxParameters.SelectedItem = _item.Parameter;
                _technic_ID = _item.Technic_ID;
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
