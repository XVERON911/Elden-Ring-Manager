using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elden_Ring_Manager.Resources.Files
{
    internal class FirewallManager
    {
        private const string RuleNamePrefix = "BlockRule_";

        private static void ExecuteCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(output)) Console.WriteLine(output);
                if (!string.IsNullOrEmpty(error)) Console.WriteLine($"Error: {error}");
            }
        }



        public static void BlockNetworkAccess(string directoryPath, TextBox terminalBox)
        {
            if (!Directory.Exists(directoryPath))
            {
                //Console.WriteLine("Directory does not exist.");
                terminalBox.Text += "Directory does not exist.";
                terminalBox.SelectionStart = terminalBox.Text.Length;
                terminalBox.ScrollToCaret();
                return;
            }

            foreach (string exeFile in Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories))
            {
                string ruleName = RuleNamePrefix + Path.GetFileName(exeFile); // Unique rule per exe
                string command = $"advfirewall firewall add rule name=\"{ruleName}\" dir=out action=block program=\"{exeFile}\" enable=yes";

                ExecuteCommand(command);
            }
            terminalBox.Text += $"RULES ADDED: {directoryPath} {Environment.NewLine}";
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
        }

        public static void UnblockNetworkAccess(string directoryPath, TextBox terminalBox)
        {
            foreach (string exeFile in Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories))
            {
                string ruleName = RuleNamePrefix + Path.GetFileName(exeFile);
                string command = $"advfirewall firewall delete rule name=\"{ruleName}\"";

                ExecuteCommand(command);
            }
            terminalBox.Text += $"RULES REVERSED: {directoryPath}{Environment.NewLine}";
            terminalBox.SelectionStart = terminalBox.Text.Length;
            terminalBox.ScrollToCaret();
        }
    }
}
