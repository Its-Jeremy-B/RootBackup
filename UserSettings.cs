using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RootBackup
{
    class UserSettings
    {
        static string settingLocation = G.applicationLocation + @"\Settings.ini";
        static string[] settingNames = {"source-folders", "auto-backup", "backup-interval", "last-backup", "last-backup-millis", "first-launch", "crash-reason", "target-folder"};
        static string defaultFolders = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "*" + Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "*" + Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "*" + Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        static string[] defaultSettingValues = { defaultFolders, "Yes", "24", "N/A", "0", "Yes", "None", @"C:\Backups\"};
        static string[] settingValues = defaultSettingValues;

        public static void resetSettings()
        {
            for (int i = 0; i < settingNames.Length; i++)
            {
                setSetting(settingNames[i], settingValues[i]);
            }
        }

        public static string getDefaultSettingValue(string settingName)
        {
            for (int i = 0; i < settingNames.Length; i++)
            {
                if(settingNames[i].Equals(settingName)){
                    return defaultSettingValues[i];
                }
            }
            return "";
        }

        public static string getSetting(string settingName)
        {
            try
            {
                return Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\RootBackup", settingName, null).ToString();
            }
            catch (Exception e)
            {
                G.print(settingName);
                G.messageBox("RootBackup Corruption", "It appears that your settings for RootBackup have been corrupted or may have been inaccessable for an unknown reason.\nSome of your settings have been reset to the default settings.", ToolTipIcon.Error);
                G.handleErrors(e);
                setSetting(settingName, getDefaultSettingValue(settingName));
            }
            return getDefaultSettingValue(settingName);
        }

        public static void setSetting(string settingName, string settingValue)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\RootBackup", settingName, settingValue);
        }
    }
}
