namespace RootBackup
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            label1 = new System.Windows.Forms.Label();
            listBox1 = new System.Windows.Forms.ListBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            button3 = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(260, 17);
            label1.TabIndex = 1;
            label1.Text = "Which folders would you like to backup?";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 16;
            listBox1.Location = new System.Drawing.Point(13, 30);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(389, 100);
            listBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(408, 30);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(157, 31);
            button1.TabIndex = 3;
            button1.Text = "Add folder";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(408, 67);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(157, 31);
            button2.TabIndex = 4;
            button2.Text = "Remove folder";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(407, 149);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(157, 31);
            button3.TabIndex = 5;
            button3.Text = "Choose folder";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(button3_Click);
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(12, 153);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(389, 22);
            textBox1.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 133);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(319, 17);
            label2.TabIndex = 7;
            label2.Text = "Where would you like your backups to be stored?";
            // 
            // Settings
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(577, 190);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "Settings";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "RootBackup Settings";
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Settings_FormClosing);
            Load += new System.EventHandler(Settings_Load);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
    }
}