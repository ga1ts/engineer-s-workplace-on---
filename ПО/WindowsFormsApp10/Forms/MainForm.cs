using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Export;
using RemontKt.Forms.AddEditForms;
using RemontKt.Forms.MainForms;
using RemontKt.Forms.ReferenceForms;
using RemontKt.Forms.Reports;
using RequestWorksForm = RemontKt.Forms.MainForms.RequestWorksForm;

namespace RemontKt.Forms
{
    public partial class MainForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        #region Коллекции для заполнения ComboBox в панели поиска

        private List<FaultType> _faultTypes;

        private List<Status> _statuses;

        private List<TechnicType> _technicTypes;

        private List<Manufacturer> _manufacturers;

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод обновления таблицы данных
        /// </summary>
        private void UpdateDatagrid()
        {
            requestBindingSource.DataSource = null;
            requestBindingSource.DataSource = _repository.GetEntityes<Request>();
            dataGridView.ClearSelection();
        }

       

        #region Обработка событий пунктов меню
        
        private void должностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PostsForm().ShowDialog();
        }

        private void видыНеисправностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FaultTypesForm().ShowDialog();
            InitComboBoxes();
            UpdateDatagrid();
        }

        private void статусыЗаявокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StatusesForm().ShowDialog();
            InitComboBoxes();
            UpdateDatagrid();
        }

        private void работыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WorksForm().ShowDialog();
        }

        private void производителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManufacturersForm().ShowDialog();
            InitComboBoxes();
        }

        private void характеристикиТехникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ParametersForm().ShowDialog();
        }

        private void видыТехникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TechnicTypesForm().ShowDialog();
            InitComboBoxes();
        }

        private void бытоваяТехникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TechnicsForm().ShowDialog();
        }

        private void типыЗапчастейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SpareTypesForm().ShowDialog();
            UpdateDatagrid();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EmployeesForm().ShowDialog();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClientsForm().ShowDialog();
            UpdateDatagrid();
        }

        private void запчастиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SparesForm().ShowDialog();
        }
        #endregion

        /// <summary>
        /// Обработка события нажатия кнопки добавления новой заявки
        /// </summary>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var addRequestForm = new AddEditRequestForm();
            if (addRequestForm.ShowDialog()==DialogResult.OK)
            {
                UpdateDatagrid();
            }
        }

        /// <summary>
        /// Обработчик события нажатия ячеек с икноками редактирования и удаления
        /// </summary>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView.NewRowIndex || e.RowIndex < 0)
                return;
            // если нажали на ячейку с иконкой экспорта квитанции в Word
            if (e.ColumnIndex == dataGridView.Columns["ExportToWordColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Request;
                using (var export=new ExportToWord())
                {
                    export.CreateRequestTicket(item);
                }
            }
            // если нажали на ячейку с иконкой редактирования
            if (e.ColumnIndex == dataGridView.Columns["EditColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Request;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditRequestForm(item).ShowDialog();
                UpdateDatagrid();
            }
            if (e.ColumnIndex == dataGridView.Columns["DetailColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Request;
                // открываем форму просмотра заявки
                new RequestWorksForm(item).ShowDialog();
                UpdateDatagrid();
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Request;
                var result = MessageBox.Show($"Удалить заявку с ID = {item.Request_ID}? ", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result != DialogResult.OK) return;
                try
                {
                    // удаляем 
                    _repository.Delete(item);
                    // обновляем таблицу с данными
                    UpdateDatagrid();
                }
                catch (Exception exception)
                {
                    ExceptionHandler.HandleException(exception);
                }
            }
        }

        /// <summary>
        /// Обработка события загрузки главной формы
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Repository.FirstStart())
            {
                toolStripStatusLabel1.Text = "Первый запуск программы! Добавьте сотрудника и перезагрузите программу!";
                toolStripStatusLabel1.ForeColor=Color.Red;
            }
            else
            {
                toolStripStatusLabel1.Text = $"Вы вошли как: {Repository.LoginedEmployee}";
            }
            UpdateDatagrid();
            InitComboBoxes();
        }

        /// <summary>
        /// Метод инициализации данных ComboBox
        /// </summary>
        private void InitComboBoxes()
        {
            _faultTypes=new List<FaultType>(){new FaultType(){FaultTypeName = "Все типы поломок"}};
            _faultTypes.AddRange(_repository.GetEntityes<FaultType>());
            comboBoxFaultTypes.DataSource = _faultTypes;

            _statuses=new List<Status>(){new Status(){StatusName = "Все статусы заявок"}};
            _statuses.AddRange(_repository.GetEntityes<Status>());
            comboBoxStatuses.DataSource = _statuses;

            _technicTypes=new List<TechnicType>(){new TechnicType(){TechnicTypeName = "Все типы техники"}};
            _technicTypes.AddRange(_repository.GetEntityes<TechnicType>());
            comboBoxTechnicType.DataSource = _technicTypes;

            _manufacturers=new List<Manufacturer>(){new Manufacturer(){ManufacturerName = "Все производители"}};
            _manufacturers.AddRange(_repository.GetEntityes<Manufacturer>());
            comboBoxManufacturer.DataSource = _manufacturers;
        }

        /// <summary>
        /// Метод сброса параметров поиска заявок
        /// </summary>
        private void ResetSearchData()
        {
            dateTimePickerFrom.Checked = false;
            dateTimePickerTo.Checked = false;
            textBoxClientFName.Clear();
            textBoxRequestId.Clear();
            comboBoxStatuses.SelectedItem = _statuses.FirstOrDefault();
            comboBoxFaultTypes.SelectedItem = _faultTypes.FirstOrDefault();
            comboBoxTechnicType.SelectedItem = _technicTypes.FirstOrDefault();
            comboBoxManufacturer.SelectedItem = _manufacturers.FirstOrDefault();
            UpdateDatagrid();
        }

        /// <summary>
        /// Обработка события нажатия кнопки поиска заявок
        /// </summary>
        private void buttonSearch_Click(object sender, EventArgs e)
        {

            // если в dateTimePicker не стоит галочка (Checked) , тогда значение null
            var dateFrom = dateTimePickerFrom.Checked ? dateTimePickerFrom.Value.Date : (DateTime?)null;
            var dateTo = dateTimePickerTo.Checked ? dateTimePickerTo.Value.Date : (DateTime?)null;
            var status = comboBoxStatuses.SelectedItem as Status;
            var faultType = comboBoxFaultTypes.SelectedItem as FaultType;
            var techType = comboBoxTechnicType.SelectedItem as TechnicType;
            var manufacturer = comboBoxManufacturer.SelectedItem as Manufacturer;
            
            var requestId = 0; // номер заявки по умолчанию
            if (!string.IsNullOrWhiteSpace(textBoxRequestId.Text))
            {
                if (!int.TryParse(textBoxRequestId.Text, out requestId))
                {
                    // если ввели слишком большое или слишком маленькое значение для типа int
                    MessageBox.Show("Не верно введен номер заявки!", "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            requestBindingSource.DataSource = null;
            requestBindingSource.DataSource = _repository.SearchRequests(dateFrom, dateTo, textBoxClientFName.Text,
                faultType, requestId, status,techType,manufacturer);
        }

        /// <summary>
        /// Обработка события нажатия кнопки сброса параметров поиска
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
           ResetSearchData(); 
        }

        /// <summary>
        /// Обработка события нажатия клавиш в textBox поиска с номером заявки
        /// </summary>
        private void textBoxRequestId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // разрешаемввод только цифр от 0 до 9
            if ((e.KeyChar<=47||e.KeyChar>=58)&&e.KeyChar!=8)
            {
                e.Handled = true;
            }
        }

        private void реестрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Reports.RequestWorksForm().ShowDialog();
        }

        private void отчётПоЗаявкамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RequestReportForm().ShowDialog();
        }

        private void отчётОРасходеЗапчастейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SpareReportForm().ShowDialog();
        }

        private void статистикаЗаявокПоВидамНеисправностейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RequestByFaultTypeReportForm().ShowDialog();
        }

        private void статистикаЗаявокПоТипуТехникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RequestByTechnicTypeReportForm().ShowDialog();
        }
    }
}
