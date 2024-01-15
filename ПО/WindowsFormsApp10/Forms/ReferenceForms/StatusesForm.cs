using System;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.ReferenceForms
{
    public partial class StatusesForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        public Status SelectedStatus { get; private set; }

        private readonly bool _selectMode;

        public StatusesForm()
        {
            InitializeComponent();
            UpdateDatagrid();
        }

        public StatusesForm(bool selectMode)
        {
            _selectMode = selectMode;
            InitializeComponent();
            UpdateDatagrid();
        }

        /// <summary>
        ///     Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
        {
            try
            {
                statusBindingSource.DataSource = null;
                statusBindingSource.DataSource = _repository.GetEntityes<Status>();
                dataGridView.ClearSelection();
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        /// <summary>
        /// Обработчик события нажатия ячеек с икноками редактирования и удаления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView.NewRowIndex || e.RowIndex < 0)
                return;
            // если нажали на ячейку с иконкой редактирования
            if (e.ColumnIndex == dataGridView.Columns["EditColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Status;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditStatusForm(item).ShowDialog();
                UpdateDatagrid();
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Status;
                var result = MessageBox.Show($"Удалить статус с ID = {item.Status_ID}? ", "",
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
        /// Обработчик события нажатия кнопки Добавить
        /// </summary>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // открываем форму в режиме добавления (пустой конструктор
            var result = new AddEditStatusForm().ShowDialog();
            if (result == DialogResult.OK)
            {
                //если были добавлены данные обновляем таблицу
                UpdateDatagrid();
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_selectMode && dataGridView.SelectedRows.Count > 0)
            {
                SelectedStatus = dataGridView.SelectedRows[0].DataBoundItem as Status;
                DialogResult = DialogResult.OK;
            }
        }
    }
}