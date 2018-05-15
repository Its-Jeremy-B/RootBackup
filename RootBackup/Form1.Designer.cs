namespace RootBackup
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button3 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            label3 = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button1.Location = new System.Drawing.Point(12, 46);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(200, 54);
            button1.TabIndex = 0;
            button1.Text = "Backup Settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(12, 191);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(406, 81);
            label1.TabIndex = 2;
            label1.Text = "Status: Waiting for queue";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(162, 17);
            label2.TabIndex = 3;
            label2.Text = "Last backup finished on ";
            // 
            // button3
            // 
            button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button3.Location = new System.Drawing.Point(218, 46);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(200, 54);
            button3.TabIndex = 4;
            button3.Text = "Start Backup";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(button3_Click);
            // 
            // button2
            // 
            button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button2.Location = new System.Drawing.Point(218, 105);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(200, 54);
            button2.TabIndex = 5;
            button2.Text = "Browse Backups";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // button4
            // 
            button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button4.Location = new System.Drawing.Point(12, 105);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(200, 54);
            button4.TabIndex = 6;
            button4.Text = "Schedule Settings";
            button4.UseVisualStyleBackColor = true;
            button4.Click += new System.EventHandler(button4_Click);
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon1_MouseDoubleClick);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 26);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(159, 17);
            label3.TabIndex = 7;
            label3.Text = "Next backup will start in ";
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(12, 165);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(406, 23);
            progressBar1.TabIndex = 8;
            progressBar1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(430, 282);
            Controls.Add(progressBar1);
            Controls.Add(label3);
            Controls.Add(button4);
            Controls.Add(button2);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "RootBackup";
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
            FormClosed += new System.Windows.Forms.FormClosedEventHandler(Form1_FormClosed);
            Load += new System.EventHandler(Form1_Load);
            ResumeLayout(false);
            PerformLayout();

        }






        #endregion

        public static System.Windows.Forms.ProgressBar progressBar1;
        public static System.Windows.Forms.Button button1;
        public static System.Windows.Forms.Label label1;
        public static System.Windows.Forms.Label label2;
        public static System.Windows.Forms.Button button3;
        public static System.Windows.Forms.Button button2;
        public static System.Windows.Forms.Button button4;
        public static System.Windows.Forms.NotifyIcon notifyIcon1;
        public static System.Windows.Forms.Label label3;
    }
}

