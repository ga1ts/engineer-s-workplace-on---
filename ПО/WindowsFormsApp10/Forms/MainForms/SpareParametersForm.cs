using System;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.MainForms
{
    public partial class SpareParametersForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private readonly Spare _spare;
        public SpareParametersForm(Spare spare)
        {
            _spare = spare;
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
                spareParameterBindingSource.DataSource = null;
                spareParameterBindingSource.DataSource = _repository.GetEntityes<SpareParameter>(sp=>sp.Spare_ID==_spare.Spare_ID);
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
                var item = dataGridView.SelectedRows[0].DataBoundItem as SpareParameter;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditSpareParameterForm(item).ShowDialog();
                UpdateDatagrid();
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Post;
                var result = MessageBox.Show($"Удалить должность с ID = {item.Post_ID}? ", "",
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
            var result = new AddEditSpareParameterForm(_spare).ShowDialog();
            if (result == DialogResult.OK)
            {
                //если были добавлены данные обновляем таблицу
                UpdateDatagrid();
            }
        }

        private void SpareParametersForm_Load(object sender, EventArgs e)
        {
            label1.Text = _spare.ToString();
        }
    }
}