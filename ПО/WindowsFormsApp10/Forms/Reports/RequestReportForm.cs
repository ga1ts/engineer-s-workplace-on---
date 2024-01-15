using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Export;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.Reports
{
    public partial class RequestReportForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        #region Коллекции для заполнения ComboBox в панели поиска

        private List<FaultType> _faultTypes;

        private List<Status> _statuses;

        private List<TechnicType> _technicTypes;

        private List<Manufacturer> _manufacturers;

        #endregion
        public RequestReportForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
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
                faultType, requestId, status, techType, manufacturer);
        }

        /// <summary>
        /// Метод инициализации данных ComboBox
        /// </summary>
        private void InitComboBoxes()
        {
            _faultTypes = new List<FaultType>() { new FaultType() { FaultTypeName = "Все типы поломок" } };
            _faultTypes.AddRange(_repository.GetEntityes<FaultType>());
            comboBoxFaultTypes.DataSource = _faultTypes;

            _statuses = new List<Status>() { new Status() { StatusName = "Все статусы заявок" } };
            _statuses.AddRange(_repository.GetEntityes<Status>());
            comboBoxStatuses.DataSource = _statuses;

            _technicTypes = new List<TechnicType>() { new TechnicType() { TechnicTypeName = "Все типы техники" } };
            _technicTypes.AddRange(_repository.GetEntityes<TechnicType>());
            comboBoxTechnicType.DataSource = _technicTypes;

            _manufacturers = new List<Manufacturer>() { new Manufacturer() { ManufacturerName = "Все производители" } };
            _manufacturers.AddRange(_repository.GetEntityes<Manufacturer>());
            comboBoxManufacturer.DataSource = _manufacturers;
        }

        private void textBoxRequestId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // разрешаемввод только цифр от 0 до 9
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void RequestWorksForm_Load(object sender, EventArgs e)
        {
            InitComboBoxes();
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
            requestBindingSource.DataSource = null;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetSearchData();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            UpdateDatagrid();
        }

        private void buttonExportToWord_Click(object sender, EventArgs e)
        {
            var dateFrom = dateTimePickerFrom.Checked ? dateTimePickerFrom.Value.Date : (DateTime?)null;
            var dateTo = dateTimePickerTo.Checked ? dateTimePickerTo.Value.Date : (DateTime?)null;

            var data = requestBindingSource.DataSource as List<Request>;
            if (data != null && data.Count > 0)
            {
                using (var export = new ExportToWord())
                {
                    export.CreateRequestsReport(data,dateFrom, dateTo);
                }
            }
            else
            {
                MessageBox.Show("Таблица не содержит данных для экспорта!", "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

    }
}