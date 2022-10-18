using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;

namespace ParaDriver
{
    public class Helpers
    {
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            if(sourceDirectory != targetDirectory)
            {
                CopyAll(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
            }
        }

        public static void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        public static void ForceCloseParaChrome(Guid paraId)
        {
            string pid = GetPID(paraId).ToString();
            if(pid != "-1")
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo() { FileName = "cmd.exe", Arguments = $"/c taskkill /f /pid {pid} /t" };
                process.Start();
            }
        }

        private static int GetPID(Guid paraId)
        {
            Process[] processes = Process.GetProcessesByName("chrome");

            for (int p = 0; p < processes.Length; p++)
            {
                ManagementObjectSearcher commandLineSearcher = new("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + processes[p].Id);
                string commandLine = string.Empty;
                foreach (ManagementObject commandLineObject in commandLineSearcher.Get())
                {
                    commandLine += commandLineObject["CommandLine"];
                }

                if (commandLine.Contains(paraId.ToString()))
                {
                    return processes[p].Id;
                }
            }

            return -1;
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
