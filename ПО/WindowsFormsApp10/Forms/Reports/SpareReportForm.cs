using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Export;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.Reports
{
    public partial class SpareReportForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private List<TechnicType> _technicTypes;
        private List<SpareType> _spareTypes;

        public SpareReportForm()
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
                var technicType = comboBoxTechnickType.SelectedItem as TechnicType;
                var spareType = comboBoxSpareType.SelectedItem as SpareType;
                requestWorkBindingSource.DataSource = null;
                requestWorkBindingSource.DataSource = _repository.SearchRequestWorks(dateFrom, dateTo, technicType, spareType, textBoxSpareName.Text, textBoxSpareAricle.Text);
                dataGridView.ClearSelection();
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        private void InitComboBoxes()
        {
            _spareTypes = new List<SpareType>();

            _technicTypes = new List<TechnicType> { new TechnicType() };
            _technicTypes.AddRange(_repository.GetEntityes<TechnicType>());
            comboBoxTechnickType.DataSource = _technicTypes;

            
        }

        private void RequestWorksForm_Load(object sender, EventArgs e)
        {
            InitComboBoxes();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dateTimePickerFrom.Checked = false;
            dateTimePickerTo.Checked = false;
            comboBoxTechnickType.SelectedItem = _technicTypes.FirstOrDefault();
            textBoxSpareAricle.Clear();
            textBoxSpareName.Clear();
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
            if (data != null && data.Count > 0)
            {
                using (var export = new ExportToWord())
                {
                    export.CreateSpareReport(data, dateFrom, dateTo);
                }
            }
            else
            {
                MessageBox.Show("Таблица не содержит данных для экспорта!", "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

        }

        private void comboBoxTechnickType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var technicType = comboBoxTechnickType.SelectedItem as TechnicType;
            _spareTypes.Clear();
            if (technicType!=null&&technicType.TechnicType_ID>0)
            {
                _spareTypes.Add(new SpareType());
                _spareTypes.AddRange(_repository.GetEntityes<SpareType>(spt =>spt.TechnicType_ID==technicType.TechnicType_ID ));
                comboBoxSpareType.DataSource = null;
                comboBoxSpareType.DataSource = _spareTypes;
            }
            else
            {
                _spareTypes.Add(new SpareType());
                comboBoxSpareType.DataSource = null;
                comboBoxSpareType.DataSource = _spareTypes;
                comboBoxSpareType.SelectedItem = _spareTypes.FirstOrDefault();
            }
        }
    }
}