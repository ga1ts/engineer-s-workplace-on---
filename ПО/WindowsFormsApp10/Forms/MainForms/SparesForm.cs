using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Forms.AddEditForms;


namespace RemontKt.Forms.MainForms
{
    public partial class SparesForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private List<TechnicType> _technicTypes;

        private List<SpareType> _spareTypes;

        public Spare SelectedSpare { get; private set; }

        private readonly bool _selectMode;

        public SparesForm()
        {
            InitializeComponent();
        }

        public SparesForm(bool selectMode)
        {
            _selectMode = selectMode;
            InitializeComponent();
        }

        /// <summary>
        ///  Метод обновления данных таблицы
        /// </summary>
        private void UpdateDatagrid()
        {
            try
            {
                spareBindingSource.DataSource = null;
                spareBindingSource.DataSource = _repository.GetEntityes<Spare>();
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
                var item = dataGridView.SelectedRows[0].DataBoundItem as Spare;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new AddEditSpareForm(item).ShowDialog();
                UpdateDatagrid();
            }
            // если нажали на ячейку с параметрами запчасти
            if (e.ColumnIndex == dataGridView.Columns["SpareParametersColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Spare;
                // открываем форму в режиме редактирования (перегруженные конструктор)
                new SpareParametersForm(item).ShowDialog();
                UpdateDatagrid();
            }
            
            // если нажали на ячейку с иконкой удаления
            if (e.ColumnIndex == dataGridView.Columns["DeleteColumn"].Index)
            {
                var item = dataGridView.SelectedRows[0].DataBoundItem as Spare;
                var result = MessageBox.Show($"Удалить запчасть с ID = {item.Spare_ID}? ", "",
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
            var result = new AddEditSpareForm().ShowDialog();
            if (result == DialogResult.OK)
            {
                //если были добавлены данные обновляем таблицу
                UpdateDatagrid();
            }
        }

        /// <summary>
        /// Инициализация данных для заполнения ComboBox-в поиска
        /// </summary>
        private void InitSearchComboBox()
        {
            _spareTypes = new List<SpareType>();

            // в список типов техники добавляем запись "Все типы техники" у которой будет ID = 0
            _technicTypes = new List<TechnicType> { new TechnicType { TechnicTypeName = "Все типы техники" } };
            // добавляем в список типы техники с БД
            _technicTypes.AddRange(_repository.GetEntityes<TechnicType>());
            // выводим в ComboBox
            comboBoxTechnicType.DataSource = _technicTypes;

        }

        /// <summary>
        /// Обработка события загрузки формы
        /// </summary>
        private void SparesForm_Load(object sender, EventArgs e)
        {
            UpdateDatagrid();
            InitSearchComboBox();
            if (_selectMode)
            {
                dataGridView.Columns["EditColumn"].Visible = false;
                dataGridView.Columns["DeleteColumn"].Visible = false;
            }
        }

        /// <summary>
        /// Обработка события изменения выбора типа техники
        /// </summary>
        private void comboBoxTechnicType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _spareTypes.Clear();
            // на основании выбора типа техники выбираем из БД типы запастей для этого типа техники
            if (comboBoxTechnicType.SelectedItem is TechnicType selectedTechType)
            {
                _spareTypes.Add(new SpareType { SpareTypeName = "Все типы запчастей" });
                if (selectedTechType.TechnicType_ID != 0)
                {
                    _spareTypes.AddRange(_repository.GetEntityes<SpareType>(st => st.TechnicType_ID == selectedTechType.TechnicType_ID));
                }
                // выводим данные в ComboBox
                comboBoxSpareType.DataSource = null;
                comboBoxSpareType.DataSource = _spareTypes;
                comboBoxSpareType.SelectedItem = _spareTypes.First();
            }
        }

        /// <summary>
        /// Обработка события нажатия кнопки Поиска
        /// </summary>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var techType = comboBoxTechnicType.SelectedItem as TechnicType;
            var spareType = comboBoxSpareType.SelectedItem as SpareType;
            spareBindingSource.DataSource = null;
            spareBindingSource.DataSource =
                _repository.SearchSpares(techType, spareType, textBoxSpareName.Text, textBoxAricle.Text);
        }

        /// <summary>
        /// Обработка события нажатия кнопки сброса параметров поиска
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            UpdateDatagrid();
            comboBoxTechnicType.SelectedItem = _technicTypes.First();
            textBoxAricle.Clear();
            textBoxSpareName.Clear();
        }

        /// <summary>
        /// Обработка события двойного клика по ячейкам
        /// </summary>
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView.SelectedRows.Count>0)
            {
                SelectedSpare= dataGridView.SelectedRows[0].DataBoundItem as Spare;
                DialogResult = DialogResult.OK;
            }
        }
    }
}