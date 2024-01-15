using System;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.MainForms
{
    public partial class ClientsForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private readonly bool _selectMode;

        /// <summary>
        /// Выбранный объект клиента с таблицы
        /// </summary>
        public Client SelectedClient { get; private set; }

        public ClientsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструкторф формы для режима выбора клиента
        /// 
        /// </summary>
        public ClientsForm(bool selectMode)
        {
            _selectMode = selectMode;
            InitializeComponent();
        }

        /// <summary>
        ///     Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
        {
            try
            {
                clientBindingSource.DataSource = null;
                clientBindingSource.DataSource = _repository.GetEntityes<Client>();
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
                var item = dataGridView.SelectedRows[0].DataBoundItem as Client;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditClientForm(item).ShowDialog();
                UpdateDatagrid();
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Client;
                var result = MessageBox.Show($"Удалить клиента с ID = {item.Client_ID}? ", "",
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
            var result = new AddEditClientForm().ShowDialog();
            if (result == DialogResult.OK)
            {
                //если были добавлены данные обновляем таблицу
                UpdateDatagrid();
            }
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            if (_selectMode)
            {
                // скрываем кнопки удаления и редактирования
                dataGridView.Columns["EditColumn"].Visible = false;
                dataGridView.Columns["DeleteColumn"].Visible = false;
                // скрываем кнопку добавления клиента
                buttonAdd.Visible = false;
                label1.Visible = true;
            }
            UpdateDatagrid();
            
        }

        /// <summary>
        /// Обработка события двойного клика по ячейкам
        /// </summary>
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // если есть выбранная строка
            if (dataGridView.SelectedRows.Count > 0)
            {
                // получаем объект клиента со строки
                SelectedClient = dataGridView.SelectedRows[0].DataBoundItem as Client;
                // оповещаем, что клиент успешно выбран с талицы данных
                DialogResult = DialogResult.OK;
            }
        }
    }
}