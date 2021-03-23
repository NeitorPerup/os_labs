
namespace os_lab1
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.pictureBoxPause = new System.Windows.Forms.PictureBox();
            this.labelPause = new System.Windows.Forms.Label();
            this.pictureBoxStop = new System.Windows.Forms.PictureBox();
            this.labelStop = new System.Windows.Forms.Label();
            this.pictureBoxRepeat = new System.Windows.Forms.PictureBox();
            this.labelRepeat = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(863, 490);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(881, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 48);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Старт";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // pictureBoxPause
            // 
            this.pictureBoxPause.Location = new System.Drawing.Point(894, 82);
            this.pictureBoxPause.Name = "pictureBoxPause";
            this.pictureBoxPause.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxPause.TabIndex = 2;
            this.pictureBoxPause.TabStop = false;
            // 
            // labelPause
            // 
            this.labelPause.Location = new System.Drawing.Point(878, 139);
            this.labelPause.Name = "labelPause";
            this.labelPause.Size = new System.Drawing.Size(117, 45);
            this.labelPause.TabIndex = 3;
            this.labelPause.Text = "Процесс временно остановлен";
            // 
            // pictureBoxStop
            // 
            this.pictureBoxStop.Location = new System.Drawing.Point(894, 201);
            this.pictureBoxStop.Name = "pictureBoxStop";
            this.pictureBoxStop.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxStop.TabIndex = 4;
            this.pictureBoxStop.TabStop = false;
            // 
            // labelStop
            // 
            this.labelStop.Location = new System.Drawing.Point(878, 256);
            this.labelStop.Name = "labelStop";
            this.labelStop.Size = new System.Drawing.Size(117, 34);
            this.labelStop.TabIndex = 5;
            this.labelStop.Text = "Процесс завершён";
            // 
            // pictureBoxRepeat
            // 
            this.pictureBoxRepeat.Location = new System.Drawing.Point(894, 303);
            this.pictureBoxRepeat.Name = "pictureBoxRepeat";
            this.pictureBoxRepeat.Size = new System.Drawing.Size(45, 45);
            this.pictureBoxRepeat.TabIndex = 6;
            this.pictureBoxRepeat.TabStop = false;
            // 
            // labelRepeat
            // 
            this.labelRepeat.Location = new System.Drawing.Point(891, 361);
            this.labelRepeat.Name = "labelRepeat";
            this.labelRepeat.Size = new System.Drawing.Size(88, 44);
            this.labelRepeat.TabIndex = 7;
            this.labelRepeat.Text = "Процесс продолжается";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 506);
            this.Controls.Add(this.labelRepeat);
            this.Controls.Add(this.pictureBoxRepeat);
            this.Controls.Add(this.labelStop);
            this.Controls.Add(this.pictureBoxStop);
            this.Controls.Add(this.labelPause);
            this.Controls.Add(this.pictureBoxPause);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormMain";
            this.Text = "FormMain";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRepeat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.PictureBox pictureBoxPause;
        private System.Windows.Forms.Label labelPause;
        private System.Windows.Forms.PictureBox pictureBoxStop;
        private System.Windows.Forms.Label labelStop;
        private System.Windows.Forms.PictureBox pictureBoxRepeat;
        private System.Windows.Forms.Label labelRepeat;
    }
}

