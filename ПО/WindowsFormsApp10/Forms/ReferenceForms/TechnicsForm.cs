using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.ReferenceForms
{
    public partial class TechnicsForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        public Technic SelectedTechnic { get; private set; }

        private readonly bool _selectMode;

        private List<TechnicType> _technicTypes;

        public TechnicsForm(bool selectMode)
        {
            _selectMode = selectMode;
            InitializeComponent();
            UpdateDatagrid();
        }

        public TechnicsForm()
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
                technicBindingSource.DataSource = null;
                technicBindingSource.DataSource = _repository.GetEntityes<Technic>();
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
                var item = dataGridView.SelectedRows[0].DataBoundItem as Technic;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditTechnicForm(item).ShowDialog();
                UpdateDatagrid();
            }
            if (e.ColumnIndex == dataGridView.Columns["ParametersColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Technic;
                // открываем форму просмотра параметров техники
                new TechnicParametersForm(item).ShowDialog();
            }
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Technic;
                var result = MessageBox.Show($"Удалить технику с ID = {item.Technic_ID}? ", "",
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
            var result = new AddEditTechnicForm().ShowDialog();
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
                SelectedTechnic = dataGridView.SelectedRows[0].DataBoundItem as Technic;
                DialogResult = DialogResult.OK;
            }
        }

        private void TechnicsForm_Load(object sender, EventArgs e)
        {
            
            _technicTypes = new List<TechnicType> { new TechnicType() { TechnicTypeName = "Все виды техники" } };
            _technicTypes.AddRange(_repository.GetEntityes<TechnicType>());
            comboBoxTechnicType.DataSource = _technicTypes;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            comboBoxTechnicType.SelectedItem = _technicTypes.First();
            textBoxManufacturer.Clear();
            textBoxModel.Clear();
            UpdateDatagrid();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var technicType = comboBoxTechnicType.SelectedItem as TechnicType;
            technicBindingSource.DataSource = null;
            technicBindingSource.DataSource =
                _repository.SearchTechnics(technicType, textBoxManufacturer.Text, textBoxModel.Text);
        }
    }
}