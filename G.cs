using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RootBackup
{
    class G
    {
        public static int SERVER_PORT = 2001;
        public static string SERVER_REMOTE_IP = "pkdrew.bounceme.net";
        public static string SERVER_LOCAL_IP = "127.0.0.1";
        public static string SERVER_IP;
        public static string VERSION = "0.2.2";

        public static Form loginForm;
        public static Form recoveryForm;
        public static Form form1;
        public static Form scheduleForm;
        public static Form settingsForm;
        public static bool backingUp = false;
        public static bool backupError = false;
        public static string applicationLocation = @"C:\Program Files\RootBackup";
        private static string status = "Waiting for queue";
        public static Thread autoBackupThread, backupThread;
        public static string lastMessageSent;
        static string messages = "";
        public static bool loginApproved = false;
        public static bool loginFailed = false;
        public static long bytesTransferred, maxBytesTransferred;
        public static string revisionDownloadPath;

        public static void print(string str)
        {
            messages += str + "\r\n";
            Console.WriteLine(str);
        }

        public static void writeLog()
        {
            File.WriteAllText(applicationLocation+@"\Error.log", messages);
        }

        public static void createFolder(string path)
        {
            if(backingUp){
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }catch(Exception e){
                    setStatus("Stopping backup due to error");
                    backingUp = false;
                    backupError = true;
                    handleErrors(e);
                }
            }
        }

        public static string getUsername()
        {
            return Environment.UserName;
        }

        public static string[] splitString(string str, string splitter)
        {
            return str.Split(new string[] { splitter }, StringSplitOptions.None);
        }

        public static string replaceAll(string str, string replace, string replacement)
        {
            Regex rgx = new Regex(replace);
            str = rgx.Replace(str, replacement);
            return str;
        }

        public static void copyDirectory(string sourceFolder, string targetFolder)
        {
            if (backingUp)
            {
                createFolder(targetFolder);
                setStatus("Backing up " + sourceFolder);

                string[] files = Directory.GetFiles(sourceFolder);
                for (int i = 0; i < files.Length; i++)
                {
                    string[] targetArgs = G.splitString(files[i], "\\");
                    copyFile(files[i], targetFolder + "\\" + targetArgs[targetArgs.Length - 1]);
                }

                try
                {
                    string[] folders = Directory.GetDirectories(sourceFolder);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        string[] targetArgs = G.splitString(folders[i], "\\");
                        copyDirectory(folders[i], targetFolder + "\\" + targetArgs[targetArgs.Length - 1]);
                    }
                }
                catch (Exception e)
                {
                    G.print("Unable to get directories from " + targetFolder);
                }
            }
        }

        public static void uploadDirectory(string sourceFolder)
        {
            if (backingUp)
            {
                setStatus("Backing up " + sourceFolder+" online");

                string[] files = Directory.GetFiles(sourceFolder);
                for (int i = 0; i < files.Length; i++)
                {
                    uploadFile(files[i]);
                }

                try
                {
                    string[] folders = Directory.GetDirectories(sourceFolder);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        uploadDirectory(folders[i]);
                    }
                }
                catch (Exception e)
                {
                    G.print(e.ToString());
                }
            }
        }

        public static void copyFile(string sourceFile, string targetFile)
        {
            if (backingUp)
            {
                try
                {
                    if (File.Exists(targetFile))
                    {
                        G.print("Deleteing " + targetFile);
                        File.Delete(targetFile);
                    }
                    G.print("Copying " + sourceFile + " to " + targetFile);
                    File.Copy(sourceFile, targetFile);
                }
                catch (Exception e)
                {
                    setStatus("Stopping backup due to error");
                    backingUp = false;
                    backupError = true;
                    handleErrors(e);
                }
            }
        }

        public static void uploadFile(string sourceFile)
        {
            if (backingUp)
            {
                try
                {
                    getFileAsString(sourceFile);
                    //Upload file
                }
                catch (Exception e)
                {
                    setStatus("Stopping backup due to error");
                    backingUp = false;
                    backupError = true;
                    handleErrors(e);
                }
            }
        }

        public static string getFileAsString(string path)
        {
            string myString;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] bin = br.ReadBytes(Convert.ToInt32(fs.Length));
                myString = Convert.ToBase64String(bin);
            }
            return myString;
        }

        public static void getStringAsFile(string fileString, string path)
        {
            byte[] rebin = Convert.FromBase64String(fileString);
            using (FileStream fs2 = new FileStream(path, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs2))
                bw.Write(rebin);
        }

        public static void refreshStatus()
        {
            if (form1.Visible)
            {
                Form1.label1.Invoke(new MethodInvoker(delegate
                {
                    Form1.label1.Text = "Status: " + status;

                    //Set "Start Backup" option
                    if (backingUp)
                    {
                        setStartBackupOption(false);
                    }
                    else
                    {
                        setStartBackupOption(true);
                    }
                }));
            }
        }

        public static void setStatus(string text)
        {
            status = text;
            if (form1.Visible)
            {
                Form1.label1.Invoke(new MethodInvoker(delegate
                {
                    Form1.label1.Text = "Status: " + text;
                }));
            }
        }

        public static void setProgressBar(long val1, long val2)
        {
            try
            {
                Form1.progressBar1.Invoke(new MethodInvoker(delegate
                { 
                    if(val1 == 0 && val2 == 0){
                        Form1.progressBar1.Hide();
                    }else{
                        Form1.progressBar1.Show();
                    }
                    float scale = 1f;
                    if(val2 > 25000){
                        scale = 25000f / val2;
                        val1 = (long)(val1 * scale);
                        val2 = (long)(val2 * scale);
                    }
                    Form1.progressBar1.Maximum = (int)val2;
                    Form1.progressBar1.Value = (int)val1;
                }));
            }
            catch (Exception e)
            {
                //G.messageBox("", e.ToString());
            }
        }

        public static void setLastBackupTime()
        {
            if(form1.Visible){
                Form1.label2.Invoke(new MethodInvoker(delegate
                {
                    if (!UserSettings.getSetting("last-backup").Equals("N/A"))
                    {
                        DateTime datetime = DateTime.Parse(UserSettings.getSetting("last-backup"));
                        datetime = datetime.ToLocalTime();
                        string date = datetime.ToString("MMMM d");
                        string time = datetime.ToString("h:mm tt");
                        Form1.label2.Text = "Last backup finished on " + date + " at " + time+".";
                    }
                    else
                    {
                        Form1.label2.Text = "Launching first backup ever.";
                    }
                    
                }));
            }
        }

        public static void setNextBackupTime()
        {
            if (form1.Visible)
            {
                Form1.label3.Invoke(new MethodInvoker(delegate
                {
                    if(backingUp){
                        Form1.label3.Text = "Backup in progress...";
                    }else{
                        Form1.label3.Text = displayTimeUntilNextBackup();
                    }
                }));
            }
        }

        public static void setStartBackupOption(bool option)
        {
            if(form1.Visible){
                Form1.button3.Invoke(new MethodInvoker(delegate{
                    if(option){
                        Form1.button3.Enabled = true;
                        Form1.button2.Enabled = true;
                    }else{
                        Form1.button3.Enabled = false;
                        Form1.button2.Enabled = false;
                    }
                }));
            }
        }

        public static void removeNotificationIcon()
        {
            Form1.button3.Invoke(new MethodInvoker(delegate
            {
                Form1.notifyIcon1.Dispose();
            }));
        }

        public static string displayTimeUntilNextBackup()
        {
            if(UserSettings.getSetting("auto-backup").Equals("Yes")){
                int hours = (int)(timeUntilNextBackup() / (60 * 60 * 1000));
                int minutes = (int)(((timeUntilNextBackup() / (60f * 60f * 1000f)) - hours)*60f);
                if (hours >= 0 && minutes >= 0)
                {
                    return "Next backup will start in " + hours + " hours and " + minutes + " minutes";
                }
                else
                {
                    return "Backing up files now";
                }
            }else{
                return "N/A";
            }
        }

        public static long timeUntilNextBackup()
        {
            if(UserSettings.getSetting("last-backup-millis").Equals("0")){
                return -1;
            }
            return (long.Parse(UserSettings.getSetting("last-backup-millis")) + (1000 * 60 * 60 * int.Parse(UserSettings.getSetting("backup-interval"))) - getTimeMilli());
        }

        public static string getTimeUTC()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public static long getTimeMilli()
        {
            return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        public static void messageBox(string title, string body, ToolTipIcon icon)
        {
            try
            {
                if (!isFormVisible(G.form1))
                {
                    Form1.notifyIcon1.ShowBalloonTip(10000, title, body, icon);
                }
                else
                {
                    MessageBox.Show(body, title);
                }
            }
            catch (Exception e)
            {
                G.print(e.ToString());
            }
        }

        public static void messageBox(string title, string body)
        {
            try
            {
                 MessageBox.Show(body, title);
            }
            catch (Exception e)
            {
                G.print(e.ToString());
            }
        }

        public static bool isFormVisible(Form form)
        {
            try{
                if (form.Visible)
                {
                    return true;
                }
            }catch(Exception e){

            }
            return false;
        }

        public static void handleErrors(Exception e)
        {
            G.print(e.ToString());
            if (e.ToString().Contains("path was not found"))
            {
                messageBox("RootBackup Failure", "Sorry, but it appears that your target path could not be created or found\n\nPlease try again later, or reset your target folder in \"Backup Settings\"");
            }
            else if (e.ToString().Contains("unauthorized") || e.ToString().Contains("permission") || e.ToString().Contains("Access to the path"))
            {
                messageBox("RootBackup Failure", "Sorry, but it appears that you do not have permission to backup one or more of your source folders, or you may not have permissions to create your backup in your target folder.\n\nPlease change permissions or reset your target folder in \"Backup Settings\"");
            }
        }

        public static void handleFinalErrors()
        {
            if(backupErrors.Count > 0){
                backupError = true;
                string finalMessage = "Your backup has finished, but it has finished with some errors. Please read below to see more details.\n\n";

                for (int i = 0; i < backupErrors.Count; i++)
                {
                    G.print(backupErrors[i].ToString());

                    if (backupErrors[i].ToString().Contains("path was not found"))
                    {
                        finalMessage += "Error: It appears that your target path could not be created or found\n\nPlease try again later, or reset your target folder in \"Backup Settings\"\n\n";
                    }
                    else if (backupErrors[i].ToString().Contains("unauthorized") || backupErrors[i].ToString().ToString().Contains("permission") || backupErrors[i].ToString().Contains("Access to the path"))
                    {
                        finalMessage += "Error: It appears that you do not have permission to backup one or more of your source folders, or you may not have permissions to create your backup in your target folder.\n\nPlease change permissions or reset your target folder in \"Backup Settings\"\n\n";
                    }
                    else
                    {
                        finalMessage += "Please contact RootBackup support for more help.";
                    }
                }
            
                messageBox("RootBackup Failure", finalMessage, ToolTipIcon.Error);
                backupErrors.Clear();
            }
        }

        static List<Exception> backupErrors = new List<Exception>();
        static ZipFile zip;

        public static void compressDirectory(string sourcePath, string destPath)
        {
            //If backups directory doesn't exist already, create it
            if (!Directory.Exists(UserSettings.getSetting("target-folder")))
            {
                Directory.CreateDirectory(UserSettings.getSetting("target-folder"));
            }
            zip = new ZipFile();

            zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

            string[] dirs = Directory.GetDirectories(sourcePath);
            string[] files = Directory.GetFiles(sourcePath);

            G.print("Adding files");

            int items = 0;
            int directories = 0;

            for (int i = 0; i < files.Length; i++)
            {
                string directoryPath = "";
                try
                {
                    int startIndex = files[i].IndexOf(@"\") + 1;
                    directoryPath = files[i].Substring(startIndex, files[i].LastIndexOf(@"\") - startIndex);
                    createPathsInZip(directoryPath);
                    zip.AddFile(files[i], directoryPath);
                    items++;
                    setProgressBar(items + directories, dirs.Length + files.Length);
                    setStatus("Creating backup for " + sourcePath + ". " + items + " items, and " + directories + " directories added so far. This may take a while, please be patient.");
                    G.print("Added " + files[i] + " to files under " + directoryPath);
                }
                catch (Exception e)
                {
                    G.print("Error when adding " + files[i] + " to files under "+directoryPath);
                    G.print(e.ToString());
                    backupErrors.Add(e);
                }
            }

            G.print("Adding directories");

            for (int i = 0; i < dirs.Length; i++)
            {
                string directoryPath = "";
                try{
                    int startIndex = dirs[i].IndexOf(@"\")+1;
                    directoryPath = dirs[i].Substring(startIndex, dirs[i].LastIndexOf(@"\") - startIndex);
                    zip.UpdateDirectory(dirs[i], directoryPath);
                    directories++;
                    setProgressBar(items+directories, dirs.Length+files.Length);
                    setStatus("Creating backup for " + sourcePath + ". " + items + " items, and " + directories + " directories added so far. This may take a while, please be patient.");
                    G.print("Added "+dirs[i]+" to directories under "+directoryPath);
                }
                catch (Exception e)
                {
                    G.print("Error when adding " + dirs[i] + " to directories under " + directoryPath);
                    G.print(e.ToString());
                    backupErrors.Add(e);
                }
            }

            setProgressBar(0,0);
            setStatus("Creating backup for " + sourcePath + ". Finalizing backup file. This may take a while, please be patient.");
            G.print("Finalizing backup file");

            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
            zip.Save(destPath);
        }

        static List<String> createdDirectories = new List<String>();

        static void createPathsInZip(string dir)
        {
            string dirString = "";
            try
            {
                string[] dirArgs = splitString(dir, @"\");
                for (int i = 0; i <= dirArgs.Length; i++)
                {
                    for (int x = 0; x < i; x++)
                    {
                        if (x < i - 1)
                        {
                            dirString += dirArgs[x] + @"\";
                        }
                        else
                        {
                            dirString += dirArgs[x];
                        }
                    }
                    if (!createdDirectories.Contains(dirString) && !dirString.Equals(""))
                    {
                        G.print("Adding directory " + dir + " subdirectory " + dirString);
                        createdDirectories.Add(dirString);
                        zip.AddDirectoryByName(dirString);
                    }
                    dirString = "";
                }
            }
            catch (Exception ex)
            {
                G.print("Error when creating directory path " + dir + " subdirectory " + dirString + " in zip");
                dirString = "";
            }
        }
    }
}
