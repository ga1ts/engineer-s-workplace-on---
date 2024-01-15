using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.MainForms;
using RemontKt.Forms.ReferenceForms;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditRequestForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Client _client;

        private Technic _technic;

        private Request _item;

        private FaultType _faultType;

        private Status _status;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditRequestForm(Request item)
        {
            _item = item;
            _edit = true;
            _client = _item.Client;
            _technic = _item.Technic;
            _faultType = _item.FaultType;
            _status = _item.Status;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditRequestForm()
        {
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (_client == null) 
            {
                sb.AppendLine($"Не верно заполнено поле {label3.Text}!");
            }
            if (_technic == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label2.Text}!");
            }
            if (_faultType == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (_status == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label8.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxDescription.Text))
            {
                sb.AppendLine($"Не заполнено поле {label5.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxSerial.Text))
            {
                sb.AppendLine($"Не заполнено поле {label6.Text}!");
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
                    _item.Status_ID = _status.Status_ID;
                    _item.FaultType_ID = _faultType.FaultType_ID;
                    _item.Technic_ID = _technic.Technic_ID;
                    _item.Client_ID = _client.Client_ID;
                    _item.Employee_ID = Repository.LoginedEmployee.Employee_ID;
                    _item.RequestDt = dateTimePickerCreate.Value.Date;
                    _item.ExecutionDt = dateTimePickerExecution.Value.Date;
                    _item.SerialNumber = textBoxSerial.Text;
                    _item.Description = textBoxDescription.Text;

                    _item.Employee = null;
                    _item.Client = null;
                    _item.Technic = null;
                    _item.FaultType = null;
                    _item.Status = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new Request()
                    {
                        Status_ID = _status.Status_ID,
                        FaultType_ID = _faultType.FaultType_ID,
                        Technic_ID = _technic.Technic_ID,
                        Client_ID = _client.Client_ID,
                        Employee_ID = Repository.LoginedEmployee.Employee_ID,
                        RequestDt = dateTimePickerCreate.Value.Date,
                        ExecutionDt = dateTimePickerExecution.Value.Date,
                        SerialNumber = textBoxSerial.Text,
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
                Text = $"Редактирование заявки №{_item.Request_ID}";
                buttonAddEdit.Text = "Сохранить";


                textBoxStatus.Text= _item.Status.ToString();
                textBoxFaultType.Text= _item.FaultType.ToString();
                textBoxClient.Text = _client.ToString();
                textBoxTechnik.Text = _technic.ToString();
               
                
                dateTimePickerCreate.Value= _item.RequestDt;
                dateTimePickerExecution.Value= _item.ExecutionDt;
                textBoxSerial.Text= _item.SerialNumber;
                textBoxDescription.Text= _item.Description;
                buttonAddEdit.Image = Resources.save;
            }
            else
            {
                dateTimePickerCreate.MaxDate = DateTime.Now;
                dateTimePickerExecution.MinDate = DateTime.Now.AddDays(1);

                buttonAddEdit.Text = "Добавить";
                Text = "Добавление новой заявки";
                buttonAddEdit.Image = Resources.add_btn;
            }
        }

        private void linkLabelSearchClient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            var clientForm=new ClientsForm(true);
            if (clientForm.ShowDialog()==DialogResult.OK)
            {
                _client = clientForm.SelectedClient;
                textBoxClient.Text = _client.ToString();
            }
        }

        private void linkLabelSearchTechnik_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var technikForm = new TechnicsForm(true);
            if (technikForm.ShowDialog()==DialogResult.OK)
            {
                _technic = technikForm.SelectedTechnic;
                textBoxTechnik.Text = _technic.ToString();
            }
        }

        private void linkLabelFaultType_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var faultTypeForm=new FaultTypesForm(true);
            if (faultTypeForm.ShowDialog() == DialogResult.OK)
            {
                _faultType = faultTypeForm.SelectedFaultType;
                textBoxFaultType.Text = _faultType.ToString();
            }
        }

        private void linkLabelStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var statusForm = new StatusesForm(true);
            if (statusForm.ShowDialog() == DialogResult.OK)
            {
                _status = statusForm.SelectedStatus;
                textBoxStatus.Text = _status.ToString();
            }
        }
    }
}
