using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Elden_Ring_Manager.Resources.Files
{
    internal class ProcessManager
    {
        public static string processName = "eldenring";
        public ProcessManager() 
        {
            
        }

        public static bool IsProcessRunning(String ProcessName)
        {
            return Process.GetProcessesByName(ProcessName).Length > 0;
        }

        public static void killProcess(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length == 0)
                {
                    Console.WriteLine($"No process found with name: {processName}");
                    return;
                }

                foreach (Process process in processes)
                {
                    try
                    {
                        Console.WriteLine($"Killing {process.ProcessName} (PID: {process.Id})");
                        process.Kill();
                        process.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to kill {process.ProcessName}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting processes: {ex.Message}");
            }
        }
        public static void ForceCloseSteam()
        {
            killProcess("steam");
            killProcess("steamservice");
            killProcess("gameoverlayui");
        }

        public async static Task<bool> Execute(string path, string processName, TextBox terminalBox)
        {
            bool executed = false;
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                foreach (Process process in processes)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit();
                        terminalBox.Text += $"Killed Process {processName.ToUpper()}{Environment.NewLine}";
                        terminalBox.SelectionStart = terminalBox.Text.Length;
                        terminalBox.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        terminalBox.Text += $"Error killing process: {ex.Message}{Environment.NewLine}";
                    }
                }
            }

            await Task.Delay(2000);
            terminalBox.Text += $"{Environment.NewLine}Started Executing {processName.ToUpper()}...{Environment.NewLine}";
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
            string exeDirectory = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(exeDirectory))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = path,
                    WorkingDirectory = exeDirectory,
                    UseShellExecute = true
                };

                try
                {
                    Process process = new Process { StartInfo = startInfo };
                    process.Start();
                    int attempts = 0;
                    while (attempts < 10)
                    {
                        await Task.Delay(1000);
                        if (IsProcessRunning(processName))
                        {
                            executed = true;
                            await Task.Delay(5000);
                            break;
                        }
                        attempts++;
                    }
                }
                catch (Exception ex)
                {
                    terminalBox.Text += $"Error starting process: {ex.Message}{Environment.NewLine}";
                    terminalBox.SelectionStart = terminalBox.Text.Length;
                    terminalBox.ScrollToCaret();
                }
            }
            return executed;
        }

        static void CopyDirectory(string sourceDir, string destinationDir, TextBox terminalBox = null)
        {
            try
            {
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }

                foreach (string subDir in Directory.GetDirectories(sourceDir))
                {
                    string destSubDir = Path.Combine(destinationDir, Path.GetFileName(subDir));
                    if (!Directory.Exists(destSubDir))
                    {
                        Directory.CreateDirectory(destSubDir);
                    }

                    try
                    {
                        CopyDirectory(subDir, destSubDir);
                        //terminalBox.Text += $"Copied Folder: {subDir}\n";
                    }
                    catch (Exception e)
                    {
                        terminalBox.Text += $"Reseting Binaries: {subDir}: {e.Message}\n";
                        terminalBox.SelectionStart = terminalBox.Text.Length;
                        terminalBox.ScrollToCaret();
                    }
                }
            }
            catch (Exception ex)
            {
                terminalBox.Text += $"Reseting Binaries: {sourceDir}: {ex.Message}\n";
            }
        }


        public async static void launchSeamlessCoop(string eldenRingPath, string steamPath, TextBox terminalBox, Form mainForm, string pass, Panel pathspanel, Button KillER, Button Killsteam)
        {
            string originalkillERText = KillER.Text;
            string originalKillSteamtext = Killsteam.Text;
            KillER.Text = "DISABLED";
            Killsteam.Text = "DISABLED";
            KillER.Enabled = false;
            Killsteam.Enabled = false;
            FormManager.allowKillEdit = false;
            mainForm.TopMost = true;
            FirewallManager.BlockNetworkAccess(eldenRingPath, terminalBox);
            FirewallManager.BlockNetworkAccess(steamPath, terminalBox);

            CopyDirectory((Directory.GetCurrentDirectory())+"\\source", eldenRingPath, terminalBox);
            terminalBox.Text += "INJECTED ELDEN RING"+Environment.NewLine;
            terminalBox.Text += "INJECTED STEAM"+Environment.NewLine+"WAITING FOR DLL LOAD BINARIES..."+Environment.NewLine;
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
            await Task.Delay(3000);
            terminalBox.Text += "CACHED HANDSHAKE DATA" + Environment.NewLine;
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
            await Task.Delay(2000);

            bool steam_EX = await Execute($"{steamPath}\\steam.exe", "steam", terminalBox);
            await Task.Delay(10000);
            if (steam_EX)
            {
                terminalBox.Text += $"Steam Launched !{Environment.NewLine}";
                terminalBox.SelectionStart = terminalBox.Text.Length;
                terminalBox.ScrollToCaret();
            }
            else terminalBox.Text += $"Failed To launch Steam !{Environment.NewLine}"; terminalBox.SelectionStart = terminalBox.Text.Length; terminalBox.ScrollToCaret();

            bool elden_EX = await Execute($"{eldenRingPath}\\ersc_launcher.exe", "eldenring", terminalBox);
            if (elden_EX)
            {
                terminalBox.Text += $"Elden Ring Launched !{Environment.NewLine}";
                terminalBox.SelectionStart = terminalBox.Text.Length;
                terminalBox.ScrollToCaret();
            }
            else terminalBox.Text += $"Failed To launch Elden Ring !{Environment.NewLine}"; terminalBox.SelectionStart = terminalBox.Text.Length; terminalBox.ScrollToCaret();

            if (IsProcessRunning("eldenring") && IsProcessRunning("steam"))
            {
                pathspanel.Enabled = false;
                Form1.gameRunning = true;
                FirewallManager.UnblockNetworkAccess(eldenRingPath, terminalBox);
                FirewallManager.UnblockNetworkAccess(steamPath, terminalBox);
            }
            FirewallManager.UnblockNetworkAccess(eldenRingPath, terminalBox);
            FirewallManager.UnblockNetworkAccess(steamPath, terminalBox);
            terminalBox.Text += $"{Environment.NewLine}SESSION PASSWORD: {pass}{Environment.NewLine}[T.E.R.M.I.N.A.L] DO NOT CLOSE THIS APP WHILE GAME IS RUNNING !";
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
            mainForm.TopMost = false;
            KillER.Text = originalkillERText;
            Killsteam.Text = originalKillSteamtext;
            FormManager.allowKillEdit = true;
        }

        public static void UpdateInfFile(string filePath, CheckBox checkBoxAllowInvaders, string pass)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"The configuration file was not found! {Environment.NewLine}Reinstall Program !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("allow_invaders"))
                {
                    lines[i] = $"allow_invaders = {(checkBoxAllowInvaders.Checked ? "1" : "0")}";
                }

                if (lines[i].StartsWith("cooppassword"))
                {
                    lines[i] = $"cooppassword = {pass}";
                }
            }

            // Write back to the file
            File.WriteAllLines(filePath, lines);

            //MessageBox.Show("Settings updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
