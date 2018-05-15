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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            //Setting settingsForm
            G.settingsForm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            listBox1.Items.Add(folderBrowserDialog1.SelectedPath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }catch(Exception ex){

            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            //Load source folders
            string[] sourceFolders = new string[1];
            string sourceFoldersString = UserSettings.getSetting("source-folders");

            if (sourceFoldersString.Contains("*"))
            {
                sourceFolders = G.splitString(UserSettings.getSetting("source-folders"), "*");
            }
            else
            {
                sourceFolders[0] = sourceFoldersString;
            }

            //Set source folders in itemBox
            for (int i = 0; i < sourceFolders.Length; i++)
            {
                if(sourceFolders[i].Length > 3){
                    listBox1.Items.Add(sourceFolders[i]);
                }
            }

            //Set target folder in textbox
            textBox1.Text = UserSettings.getSetting("target-folder");
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sourceFolders = "";
            for(int i = 0; i < listBox1.Items.Count; i++){
                if(i == 0){
                    sourceFolders += listBox1.Items[i].ToString();
                }else{
                    sourceFolders += "*"+listBox1.Items[i].ToString();
                }
            }

            UserSettings.setSetting("source-folders", sourceFolders);
            UserSettings.setSetting("target-folder", textBox1.Text);

            G.settingsForm = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath+"\\";
        }
    }
}
