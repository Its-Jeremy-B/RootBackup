using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RootBackup
{
    public class CrashHelper
    {
        //This class will restart your helper app whenever it crashes
        //By Jeremy Blevins
        string exe;
        string path;
        public CrashHelper(string helperExecutableName, string helperExecutablePath){
            exe = helperExecutableName;
            path = helperExecutablePath;
        }

        public void start()
        {
            new Thread(new ThreadStart(() => {
                while (true)
                {
                    Thread.Sleep(10000);
                    if (!IsProcessOpen(exe) && !System.Diagnostics.Debugger.IsAttached)
                    {
                        Console.WriteLine("Helper has stopped running. Restarting now.");
                        Process.Start(path);
                    }
                }
            })).Start();
        }

        public static bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
