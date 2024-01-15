using System;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.ReferenceForms
{
    public partial class WorksForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        public WorksForm()
        {
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
                workBindingSource.DataSource = null;
                workBindingSource.DataSource = _repository.GetEntityes<Work>();
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
                var item = dataGridView.SelectedRows[0].DataBoundItem as Work;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditWorkForm(item).ShowDialog();
                UpdateDatagrid();
            }
            if (e.ColumnIndex == dataGridView.Columns["DescriptionColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Work;
                MessageBox.Show(item.Description, "Описание", MessageBoxButtons.OK);
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Work;
                var result = MessageBox.Show($"Удалить вид ремонтных работ с ID = {item.Work_ID}? ", "",
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
            var result = new AddEditWorkForm().ShowDialog();
            if (result == DialogResult.OK)
            {
                //если были добавлены данные обновляем таблицу
                UpdateDatagrid();
            }
        }

    }
}