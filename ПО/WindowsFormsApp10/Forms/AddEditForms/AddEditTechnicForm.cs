using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditTechnicForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Technic _item;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditTechnicForm(Technic item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditTechnicForm()
        {
            InitializeComponent();
        }


        private bool Check()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textBoxModel.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (comboBoxTechnicType.SelectedItem == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label2.Text}!");
            }
            if (comboBoxManufacturer.SelectedItem == null)
            {
                sb.AppendLine($"Не верно заполнено поле {label3.Text}!");
            }
            if (sb.Length != 0)
            {
                MessageBox.Show(sb.ToString(), "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Обработка события нажатия кнопки добавить
        /// </summary>
        private void buttonAddEdit_Click(object sender, EventArgs e)
        {
            if (!Check()) return;
            try
            {
                if (_edit)
                {
                    _item.Model = textBoxModel.Text;
                    _item.TechnicType_ID = (comboBoxTechnicType.SelectedItem as TechnicType).TechnicType_ID;
                    _item.Manufacturer_ID = (comboBoxManufacturer.SelectedItem as Manufacturer).Manufacturer_ID;

                    // обнуляем связанный объект
                    // без этого будет ошибка, так как будет
                    // пытаться добавить TechnicType который уже есть в БД
                    _item.TechnicType = null;
                    _item.Manufacturer = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new Technic()
                    {
                        Model = textBoxModel.Text,
                        TechnicType_ID = (comboBoxTechnicType.SelectedItem as TechnicType).TechnicType_ID,
                        Manufacturer_ID = (comboBoxManufacturer.SelectedItem as Manufacturer).Manufacturer_ID
                    };
                    _repository.Add(_item);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                ExceptionHandler.HandleException(exception);
            }
        }

        /// <summary>
        /// Обработка события загрузки формы
        /// </summary>
        private void Form_Load(object sender, EventArgs e)
        {
            comboBoxTechnicType.DataSource = _repository.GetEntityes<TechnicType>();
            comboBoxManufacturer.DataSource = _repository.GetEntityes<Manufacturer>();
            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование";
                textBoxModel.Text = _item.Model;
                comboBoxTechnicType.SelectedItem = _item.TechnicType;
                comboBoxManufacturer.SelectedItem = _item.Manufacturer;
                buttonAddEdit.Image = Resources.save;
            }
            else
            {
                buttonAddEdit.Text = "Добавить";
                Text = "Добавление";
                buttonAddEdit.Image = Resources.add_btn;
            }
        }
    }
}
