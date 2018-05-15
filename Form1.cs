using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RootBackup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            G.form1 = this;

            //Reset crash reason
            UserSettings.setSetting("crash-reason", "None");
            new CrashHelper("RootBackup Helper", @"C:\Program Files\RootBackup\RootBackup Helper.exe").start();
             
            //Enable devmode in title
            if (G.getUsername().Equals("JBlevins") && Debugger.IsAttached)
            {
                this.Text = "DevMode";
            }

            //Notify Icon init
            notifyIcon1.Text = "RootBackup";
            notifyIcon1.Icon = Icon;
            notifyIcon1.Visible = true;

            notifyIcon1.ContextMenu = new ContextMenu();
            notifyIcon1.ContextMenu.MenuItems.Add(new MenuItem("Exit", closeDown));

            //Other startup stuff
            UserSettings.setSetting("Version", G.VERSION);

            if (!Visible)
            {
                G.messageBox("RootBackup", "RootBackup is now hidden in your toolbar. You can access it again by clicking on the RootBackup icon.", ToolTipIcon.Info);
            }

            //Auto set next and last backup times
            new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(250);
                    G.setLastBackupTime();
                    G.setNextBackupTime();
                    G.refreshStatus();
                    if (G.timeUntilNextBackup() <= 0 && UserSettings.getSetting("auto-backup").Equals("Yes") && UserSettings.getSetting("source-folders").Length > 3)
                    {
                        bool startBackupBool = true;
                        if (startBackupBool && !G.backingUp)
                        {
                            G.backupThread = new Thread(new ThreadStart(()=>{
                                startBackup();
                            }));
                            G.backupThread.Start();
                        }
                    }
                }
            })).Start();

            G.setProgressBar(0, 0);
        }

        private void closeDown(object sender, EventArgs e)
        {
            UserSettings.setSetting("crash-reason", "User Exit");
            notifyIcon1.ShowBalloonTip(10000, "RootBackup", "RootBackup has been shut down and will not be able to backup your files until it is started again.", ToolTipIcon.Info);
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Check if settings form is already open
            if (G.settingsForm == null)
            {
                new Settings().Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Check if schedule form is already open
            if (G.scheduleForm == null)
            {
                new Schedule().Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            G.setStatus("Starting backup");
            G.backupThread = new Thread(new ThreadStart(() =>
            {
                startBackup();
            }));
            G.backupThread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", UserSettings.getSetting("target-folder"));
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            notifyIcon1.ShowBalloonTip(10000, "RootBackup", "RootBackup is now hidden in your toolbar. You can access it again by clicking on the RootBackup icon.", ToolTipIcon.Info);
            e.Cancel = true;
        }

        void startBackup()
        {
            G.setProgressBar(0, 0);

            if (!G.backingUp)
            {
                PowerHelper.ForceSystemAwake();
                G.backingUp = true;
                G.setStartBackupOption(false);
                G.setStatus("Analyzing source folders");
                

                string[] sourceFolders = G.splitString(UserSettings.getSetting("source-folders"), "*");

                //Delete old tmp files
                string[] tmpFiles = Directory.GetFiles(UserSettings.getSetting("target-folder"));
                for (int i = 0; i < tmpFiles.Length && G.backingUp; i++)
                {
                    G.print("Deleteing " + tmpFiles[i]);
                    if (tmpFiles[i].EndsWith(".tmp"))
                    {
                        File.Delete(tmpFiles[i]);
                    }
                }

                //Backup files online
                Thread.Sleep(1000);
                string[] sourcePaths = G.splitString(UserSettings.getSetting("source-folders"), "*");
                //G.connection.sendMessage("beginRevision");
                for (int x = 0; x < sourcePaths.Length && G.backingUp; x++)
                {
                    G.setProgressBar(0, 0);
                    if (!G.replaceAll(sourcePaths[x], " ", "").Equals(""))
                    {
                        string[] sourceFolderNameArgs = G.splitString(sourcePaths[x], @"\");
                        string sourceFolderName = sourceFolderNameArgs[sourceFolderNameArgs.Length - 1];
                        G.setStatus("Creating backup for " + sourcePaths[x] + ". This may take a while, please be patient.");
                        G.compressDirectory(sourcePaths[x], UserSettings.getSetting("target-folder") + sourceFolderName + ".zip");
                        //G.setStatus("Uploading backup for " + sourcePaths[x] + ". This may take a while, please be patient.");
                        //G.connection.uploadFile(@"C:\RootBackup\" + sourceFolderName + ".zip");
                        //G.setStatus("Clearing space for next folder backup.");
                        //File.Delete(@"C:\RootBackup\" + sourceFolderName + ".zip");
                    }
                }
                G.handleFinalErrors();
            }
            G.backingUp = false;
            G.messageBox("Backup Finished", "RootBackup has finished backing up your files.", ToolTipIcon.Info);
            G.setStartBackupOption(true);
            G.setProgressBar(0,0);
            if (G.backupError)
            {
                G.setStatus("Backup finished with errors. Will try again in " + UserSettings.getSetting("backup-interval") + " hours.");
                G.backupError = false;
            }
            else
            {
                G.setStatus("Backup finished");
            }
            PowerHelper.ResetSystemDefault();
            UserSettings.setSetting("last-backup", G.getTimeUTC());
            UserSettings.setSetting("last-backup-millis", "" + G.getTimeMilli());
        }
    }
}
