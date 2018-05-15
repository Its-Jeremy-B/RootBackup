using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RootBackup
{
    public partial class Schedule : Form
    {
        public Schedule()
        {
            InitializeComponent();

            //Set schedule form
            G.scheduleForm = this;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            label2.Show();
            numericUpDown1.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Hide();
            label2.Hide();
            numericUpDown1.Hide();
        }

        private void Schedule_FormClosing(object sender, FormClosingEventArgs e)
        {
            string autoBackup = "Yes";
            if(radioButton2.Checked){
                autoBackup = "No";
            }
            UserSettings.setSetting("auto-backup", autoBackup);
            UserSettings.setSetting("backup-interval", numericUpDown1.Value.ToString());

            G.scheduleForm = null;
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            if(UserSettings.getSetting("auto-backup").Equals("No")){
                radioButton2.Checked = true;
            }
            numericUpDown1.Value = int.Parse(UserSettings.getSetting("backup-interval"));
        }
    }
}
