using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Export;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.Reports
{
    public partial class RequestWorksForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private List<Work> _works;
        private List<Employee> _employees;



        public RequestWorksForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
        {
            try
            {
                var dateFrom = dateTimePickerFrom.Checked ? dateTimePickerFrom.Value.Date : (DateTime?)null;
                var dateTo = dateTimePickerTo.Checked ? dateTimePickerTo.Value.Date : (DateTime?)null;
                var work = comboBoxWorks.SelectedItem as Work;
                var employee = comboBoxEmployees.SelectedItem as Employee;

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
                requestWorkBindingSource.DataSource = null;
                requestWorkBindingSource.DataSource = _repository.SearchRequestWorks(dateFrom, dateTo, work, employee, requestId);
                dataGridView.ClearSelection();
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        private void InitComboBoxes()
        {
            _works = new List<Work> { new Work ()};
            _works.AddRange(_repository.GetEntityes<Work>());
            comboBoxWorks.DataSource = _works;

            _employees = new List<Employee> { new Employee() };
            _employees.AddRange(_repository.GetEntityes<Employee>());
            comboBoxEmployees.DataSource = _employees;
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

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dateTimePickerFrom.Checked = false;
            dateTimePickerTo.Checked = false;
            comboBoxWorks.SelectedItem = _works.FirstOrDefault();
            comboBoxEmployees.SelectedItem = _employees.FirstOrDefault();
            textBoxRequestId.Clear();
            requestWorkBindingSource.DataSource = null;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            UpdateDatagrid();
        }

        private void buttonExportToWord_Click(object sender, EventArgs e)
        {
            var dateFrom = dateTimePickerFrom.Checked ? dateTimePickerFrom.Value.Date : (DateTime?)null;
            var dateTo = dateTimePickerTo.Checked ? dateTimePickerTo.Value.Date : (DateTime?)null;

            var data = requestWorkBindingSource.DataSource as List<RequestWork>;
            if (data != null && data.Count>0)
            {
                using (var export = new ExportToWord())
                {
                    export.CreateRequestsWorksReport(data,dateFrom, dateTo);
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