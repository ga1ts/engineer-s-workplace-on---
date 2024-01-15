using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Export;


namespace RemontKt.Forms.Reports
{
    public partial class RequestByFaultTypeReportForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;
        
        public RequestByFaultTypeReportForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
        {
            var dateFrom = dateTimePickerFrom.Checked ? dateTimePickerFrom.Value.Date : (DateTime?)null;
            var dateTo = dateTimePickerTo.Checked ? dateTimePickerTo.Value.Date : (DateTime?)null;
            requestByFaultTypeBindingSource.DataSource = null;
            requestByFaultTypeBindingSource.DataSource =
                _repository.GetRequestGroupBy<Request, FaultType>(request => request.FaultType, dateFrom, dateTo) // выбираем по датам и группируем по типу несиправности
            // выбираем из группированной коллекции тип неисправности и количество заявок
                    .Select(requests => new RequestByFaultType { FaultType = requests.Key, Count = requests.Count() })
            .ToList();
        }


        /// <summary>
        /// Метод сброса параметров поиска заявок
        /// </summary>
        private void ResetSearchData()
        {
            dateTimePickerFrom.Checked = false;
            dateTimePickerTo.Checked = false;
            requestByFaultTypeBindingSource.DataSource = null;
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

            var data = requestByFaultTypeBindingSource.DataSource as List<RequestByFaultType>;
            if (data != null && data.Count > 0)
            {
                using (var export = new ExportToWord())
                {
                    export.CreateRequestsByFaultTypesReport(data, dateFrom, dateTo);
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