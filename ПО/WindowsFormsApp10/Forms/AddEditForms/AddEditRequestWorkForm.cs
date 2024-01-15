using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.MainForms;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditRequestWorkForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private RequestWork _item;

        private Request _request;

        private readonly bool _edit;

        private Spare _selectedSpare;

        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditRequestWorkForm(RequestWork item)
        {
            _item = item;
            _request = item.Request;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditRequestWorkForm(Request request)
        {
            _request = request;
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            //if (string.IsNullOrWhiteSpace(textBoxPostName.Text))
            //{
            //    sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            //}
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
            //if (!Check()) return;
            try
            {
                if (_edit)
                {
                    _item.Work_ID = (comboBoxWorks.SelectedItem as Work).Work_ID;
                    _item.Employee_ID = (comboBoxEmployees.SelectedItem as Employee).Employee_ID;
                    _item.Spare_ID = _selectedSpare?.Spare_ID;
                    _item.RequestWorkDt = dateTimePicker.Value.Date;

                    _item.Work = null;
                    _item.Employee = null;
                    _item.Spare = null;

                    _repository.Update(_item);
                }
                else
                {
                    _item = new RequestWork()
                    {
                        Request_ID = _request.Request_ID,
                        Work_ID = (comboBoxWorks.SelectedItem as Work).Work_ID,
                        Employee_ID = (comboBoxEmployees.SelectedItem as Employee).Employee_ID,
                        Spare_ID = _selectedSpare?.Spare_ID,
                        RequestWorkDt = dateTimePicker.Value.Date
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
            // заполняем ComboBox-ы
            comboBoxEmployees.DataSource = _repository.GetEntityes<Employee>();
            comboBoxWorks.DataSource = _repository.GetEntityes<Work>();

            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование работы по заявке";
                textBoxSpare.Text = _item.Spare?.ToString();
                comboBoxWorks.SelectedItem = _item.Work;
                comboBoxEmployees.SelectedItem = _item.Employee;
                _selectedSpare = _item.Spare;
                textBoxSpare.Text = _item.Spare == null ? "Запчасть не требуется" : _item.Spare.ToString();
                buttonAddEdit.Image = Resources.save;
            }
            else
            {
                buttonAddEdit.Text = "Добавить";
                Text = "Добавление";
                buttonAddEdit.Image = Resources.add_btn;
            }
        }

        private void linkLabelSearchSpare_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var spareForm = new SparesForm(true);
            if (spareForm.ShowDialog()==DialogResult.OK)
            {
                _selectedSpare = spareForm.SelectedSpare;
                textBoxSpare.Text = _selectedSpare.ToString();
            }
        }

        private void linkLabelDeleteSpare_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _selectedSpare = null;
            textBoxSpare.Text = "Запчасть не требуется";
        }
    }
}
