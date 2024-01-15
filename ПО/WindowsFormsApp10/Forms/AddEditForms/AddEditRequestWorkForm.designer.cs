namespace RemontKt.Forms.AddEditForms
{
    partial class AddEditRequestWorkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddEdit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabelSearchSpare = new System.Windows.Forms.LinkLabel();
            this.textBoxSpare = new System.Windows.Forms.TextBox();
            this.comboBoxEmployees = new System.Windows.Forms.ComboBox();
            this.comboBoxWorks = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelDeleteSpare = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddEdit
            // 
            this.buttonAddEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddEdit.BackColor = System.Drawing.Color.Gainsboro;
            this.buttonAddEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonAddEdit.FlatAppearance.BorderSize = 2;
            this.buttonAddEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAddEdit.Location = new System.Drawing.Point(12, 320);
            this.buttonAddEdit.Name = "buttonAddEdit";
            this.buttonAddEdit.Size = new System.Drawing.Size(153, 34);
            this.buttonAddEdit.TabIndex = 3;
            this.buttonAddEdit.UseVisualStyleBackColor = false;
            this.buttonAddEdit.Click += new System.EventHandler(this.buttonAddEdit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.Gainsboro;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonCancel.FlatAppearance.BorderSize = 2;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = global::RemontKt.Properties.Resources.delete_16;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.Location = new System.Drawing.Point(237, 320);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(153, 34);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabelDeleteSpare);
            this.groupBox1.Controls.Add(this.linkLabelSearchSpare);
            this.groupBox1.Controls.Add(this.textBoxSpare);
            this.groupBox1.Controls.Add(this.comboBoxEmployees);
            this.groupBox1.Controls.Add(this.comboBoxWorks);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 210);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // linkLabelSearchSpare
            // 
            this.linkLabelSearchSpare.AutoSize = true;
            this.linkLabelSearchSpare.Location = new System.Drawing.Point(6, 150);
            this.linkLabelSearchSpare.Name = "linkLabelSearchSpare";
            this.linkLabelSearchSpare.Size = new System.Drawing.Size(98, 15);
            this.linkLabelSearchSpare.TabIndex = 13;
            this.linkLabelSearchSpare.TabStop = true;
            this.linkLabelSearchSpare.Text = "Поиск запчасти";
            this.linkLabelSearchSpare.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSearchSpare_LinkClicked);
            // 
            // textBoxSpare
            // 
            this.textBoxSpare.Location = new System.Drawing.Point(6, 169);
            this.textBoxSpare.Name = "textBoxSpare";
            this.textBoxSpare.ReadOnly = true;
            this.textBoxSpare.Size = new System.Drawing.Size(366, 21);
            this.textBoxSpare.TabIndex = 12;
            this.textBoxSpare.Text = "Запчасть не требуется";
            // 
            // comboBoxEmployees
            // 
            this.comboBoxEmployees.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEmployees.FormattingEnabled = true;
            this.comboBoxEmployees.Location = new System.Drawing.Point(6, 103);
            this.comboBoxEmployees.Name = "comboBoxEmployees";
            this.comboBoxEmployees.Size = new System.Drawing.Size(366, 23);
            this.comboBoxEmployees.TabIndex = 10;
            // 
            // comboBoxWorks
            // 
            this.comboBoxWorks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorks.FormattingEnabled = true;
            this.comboBoxWorks.Location = new System.Drawing.Point(6, 37);
            this.comboBoxWorks.Name = "comboBoxWorks";
            this.comboBoxWorks.Size = new System.Drawing.Size(366, 23);
            this.comboBoxWorks.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Сотрудник выполняющий работу";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "Работа по заявке";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(18, 253);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 21);
            this.dateTimePicker.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Дата выполнения работы";
            // 
            // linkLabelDeleteSpare
            // 
            this.linkLabelDeleteSpare.AutoSize = true;
            this.linkLabelDeleteSpare.Location = new System.Drawing.Point(308, 151);
            this.linkLabelDeleteSpare.Name = "linkLabelDeleteSpare";
            this.linkLabelDeleteSpare.Size = new System.Drawing.Size(57, 15);
            this.linkLabelDeleteSpare.TabIndex = 14;
            this.linkLabelDeleteSpare.TabStop = true;
            this.linkLabelDeleteSpare.Text = "Удалить";
            this.linkLabelDeleteSpare.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDeleteSpare_LinkClicked);
            // 
            // AddEditRequestWorkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(402, 366);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAddEdit);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditRequestWorkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAddEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel linkLabelSearchSpare;
        private System.Windows.Forms.TextBox textBoxSpare;
        private System.Windows.Forms.ComboBox comboBoxEmployees;
        private System.Windows.Forms.ComboBox comboBoxWorks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabelDeleteSpare;
    }
}