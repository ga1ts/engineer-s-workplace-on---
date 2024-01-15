using System;
using System.Text;
using System.Windows.Forms;
using RemontKt.Data;
using RemontKt.Properties;


namespace RemontKt.Forms.AddEditForms
{
    public partial class AddEditSpareForm : Form
    {
        private readonly Repository _repository = Repository.RepositoryInstance;

        private Spare _item;

        private readonly bool _edit;
        /// <summary>
        /// Конструктор редактирования
        /// </summary>
        public AddEditSpareForm(Spare item)
        {
            _item = item;
            _edit = true;
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор добавления
        /// </summary>
        public AddEditSpareForm()
        {
            InitializeComponent();
        }

        private bool Check()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textBoxSpareName.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label1.Text}!");
            }
            if (comboBoxSpareType.SelectedItem==null)
            {
                sb.AppendLine($"Не верно заполнено поле {label2.Text}!");
            }
            if (string.IsNullOrWhiteSpace(textBoxArticle.Text))
            {
                sb.AppendLine($"Не верно заполнено поле {label3.Text}!");
            }
            if (!decimal.TryParse(textBoxPrice.Text, out _))
            {
                sb.AppendLine($"Не верно заполнено поле {label4.Text}!");
            }

            if (sb.Length != 0)
            {
                MessageBox.Show(sb.ToString(), "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void buttonAddEdit_Click(object sender, EventArgs e)
        {
            if (!Check()) return;
            try
            {
                if (_edit)
                {
                    _item.SpareName = textBoxSpareName.Text;
                    _item.SpareType_ID = (comboBoxSpareType.SelectedItem as SpareType).SpareType_ID;
                    _item.Article = textBoxArticle.Text;
                    _item.Price = decimal.Parse(textBoxPrice.Text);

                    _item.SpareType = null;
                    _repository.Update(_item);
                }
                else
                {
                    _item = new Spare()
                    {
                        SpareName = textBoxSpareName.Text,
                        SpareType_ID = (comboBoxSpareType.SelectedItem as SpareType).SpareType_ID,
                        Article = textBoxArticle.Text,
                        Price = decimal.Parse(textBoxPrice.Text)
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


        private void Form_Load(object sender, EventArgs e)
        {
            comboBoxTechnicType.DataSource = _repository.GetEntityes<TechnicType>();
            if (_item != null)
            {
                buttonAddEdit.Text = "Сохранить";
                Text = "Редактирование";
                textBoxSpareName.Text= _item.SpareName;
                comboBoxSpareType.SelectedItem= _item.SpareType;
               textBoxArticle.Text = _item.Article;
                textBoxPrice.Text = _item.Price.ToString("f2");
                buttonAddEdit.Image = Resources.save;
            }
            else
            {
                buttonAddEdit.Text = "Добавить";
                Text = "Добавление";
                buttonAddEdit.Image = Resources.add_btn;
            }
        }

        /// <summary>
        /// Обработка события изменения выбора типа техники
        /// </summary>
        private void comboBoxTechnicType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // на основании выбранного типа техники загружаем типы запчастей в ComboBox
            if (comboBoxTechnicType.SelectedItem is TechnicType techType)
            {
                comboBoxSpareType.DataSource =
                    _repository.GetEntityes<SpareType>(sp => sp.TechnicType_ID == techType.TechnicType_ID);
            }
        }
    }
}
