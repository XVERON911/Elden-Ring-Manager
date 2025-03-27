using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.Win32;
using System.Diagnostics;

namespace Elden_Ring_Manager.Resources.Files
{
    internal class FormManager
    {

        public FormManager()
        {

        }

        public static int lastIndex;
        private static readonly string[] ImageFiles = { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "9.jpg", "10.jpg", "11.jpg" };
        public static Image SetRandomBackground()
        {
            if (ImageFiles.Length == 0) return null;

            Random random = new Random();
            int index;

            do
            {
                index = random.Next(ImageFiles.Length);
            }
            while (index == lastIndex);

            string imagePath = Path.Combine(Application.StartupPath, "dx", ImageFiles[index]);

            if (File.Exists(imagePath))
            {
                Form1.imgLoaded = true;
                lastIndex = index;

                try
                {
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        return Image.FromStream(fs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "SYSTEM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else
            {
                Form1.imgLoaded = false;
                MessageBox.Show($"Program was Manipulated. Reinstall Program!", "SYSTEM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private static Random random = new Random();
        public static async void Challenger(Control challengerLabel, Control animationspeedLabel)
        {
            while (true)
            {
                int delay = random.Next(200, 1001);
                challengerLabel.ForeColor = Color.Firebrick;
                animationspeedLabel.ForeColor = Color.Lime;
                await Task.Delay(delay);
                challengerLabel.ForeColor = Color.Lime;
                animationspeedLabel.ForeColor = Color.DarkCyan;
                await Task.Delay(delay);
                challengerLabel.ForeColor = Color.DarkCyan;
                animationspeedLabel.ForeColor = Color.Firebrick;
                await Task.Delay(delay);
            }
        }

        public static bool IsUserAdmin() // true / false
        {
            using (WindowsIdentity Identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(Identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void DisableAllControls(Control parent) // disable all
        {
            foreach (Control control in parent.Controls)
            {
                control.Enabled = false;
                if (control.HasChildren)
                {
                    DisableAllControls(control);
                }
            }

        }

        public static void EnableAllControls(Control parent) // disable all
        {
            foreach (Control control in parent.Controls)
            {
                control.Enabled = false;
                if (control.HasChildren)
                {
                    EnableAllControls(control);
                }
            }
        }


        public static async void launchButtonControl(Control launchbutton, Control pathsPanel)
        {
            while (true)
            {
                if (ProcessManager.IsProcessRunning("eldenring"))
                {
                    launchbutton.Text = "Running Seamless Coop";
                    launchbutton.BackColor = Color.Transparent;
                    pathsPanel.Enabled = false;
                    if(!Form1.allowPathEdit)
                        Form1.allowPathEdit = true;
                }
                else
                {
                    launchbutton.Text = "Launch Seamless Coop";
                    if(Form1.allowPathEdit)
                        pathsPanel.Enabled = true;
                }
                await Task.Delay(2000);
            }
        }


        public async void Buttons3PanelControl(Button noattackButton, Button noDeadButton, Button noFPButton, Button noSPbutton, Button noDamageButton, Button NoMoveButton, Button noupdateButton)
        {
            while (true)
            {
                if (MemoryManagement.noDead)
                {
                    noDeadButton.BackColor = Color.ForestGreen;
                    noDeadButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noDeadButton.BackColor = Color.Transparent;
                    noDeadButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noDamage)
                {
                    noDamageButton.BackColor = Color.ForestGreen;
                    noDamageButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noDamageButton.BackColor = Color.Transparent;
                    noDamageButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noFPcons)
                {
                    noFPButton.BackColor = Color.ForestGreen;
                    noFPButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noFPButton.BackColor = Color.Transparent;
                    noFPButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noSPcons)
                {
                    noSPbutton.BackColor = Color.ForestGreen;
                    noSPbutton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noSPbutton.BackColor = Color.Transparent;
                    noSPbutton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noAttack)
                {
                    noattackButton.BackColor = Color.ForestGreen;
                    noattackButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noattackButton.BackColor = Color.Transparent;
                    noattackButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noMove)
                {
                    NoMoveButton.BackColor = Color.ForestGreen;
                    NoMoveButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    NoMoveButton.BackColor = Color.Transparent;
                    NoMoveButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }

                if (MemoryManagement.noUpdate)
                {
                    noupdateButton.BackColor = Color.ForestGreen;
                    noupdateButton.FlatAppearance.MouseOverBackColor = Color.ForestGreen;
                }
                else
                {
                    noupdateButton.BackColor = Color.Transparent;
                    noupdateButton.FlatAppearance.MouseOverBackColor = Color.DimGray;
                }
                await Task.Delay(500);
            }
        }


        public static string dailyTemp = "", hourlyTemp = "", minuteTemp = "", dailyTempAD = "", hourlyTempAD = "", minuteTempAD = "";
        public static string enteredCode = "";

        public static void updateLabelADMIN(TextBox adminTextBox)
        {
            string daily = dailyTemp;
            string hourly = hourlyTemp;
            string minute = minuteTemp;

            string dailyAD = dailyTempAD;
            string hourlyAD = hourlyTempAD;
            string minuteAD = minuteTempAD;

            adminTextBox.Text = $"ADMIN PANEL:{Environment.NewLine}Daily USER Code: {daily}{Environment.NewLine}Hourly USER Code: {hourly}{Environment.NewLine}Minute USER Code: {minute}{Environment.NewLine}{Environment.NewLine}";
            adminTextBox.Text += $"Daily ADMIN Code: {dailyAD}{Environment.NewLine}Hourly ADMIN Code: {hourlyAD}{Environment.NewLine}Minute ADMIN Code: {minuteAD}";
        }
        public async static void CheckGeneratedCodes(TextBox adminTextbox)
        {
            while (true)
            {
                string DailyCode = GnRodes.GenerateDailyCode("BMWS200ASF");
                string HourlyCode = GnRodes.GenerateHourlyCode("BMWS200ASF");
                string MinuteCode = GnRodes.GenerateMinuteCode("BMWS200ASF");

                string DailyCodeAD = GnRodes.GenerateDailyCode("Password600@KK");
                string HourlyCodeAD = GnRodes.GenerateHourlyCode("Password600@KK");
                string MinuteCodeAD = GnRodes.GenerateMinuteCode("Password600@KK");

                if (dailyTemp != DailyCode)
                {
                    dailyTemp = DailyCode;
                    updateLabelADMIN(adminTextbox);
                }
                if (hourlyTemp != HourlyCode)
                {
                    hourlyTemp = HourlyCode;
                    updateLabelADMIN(adminTextbox);
                }
                if (minuteTemp != MinuteCode) 
                {
                    minuteTemp = MinuteCode;
                    updateLabelADMIN(adminTextbox);
                }

                if (dailyTempAD != DailyCodeAD)
                {
                    dailyTempAD = DailyCodeAD;
                    updateLabelADMIN(adminTextbox);
                }
                if (hourlyTempAD != HourlyCodeAD)
                {
                    hourlyTempAD = HourlyCodeAD;
                    updateLabelADMIN(adminTextbox);
                }
                if (minuteTempAD != MinuteCodeAD)
                {
                    minuteTempAD = MinuteCodeAD;
                    updateLabelADMIN(adminTextbox);
                }

                if (!String.IsNullOrEmpty(enteredCode))
                {
                    if((enteredCode == DailyCode) || (enteredCode == HourlyCode) || (enteredCode == MinuteCode))
                    {
                        Form1.user = true;
                    }
                    else if ((enteredCode == DailyCodeAD) || (enteredCode == HourlyCodeAD) || (enteredCode == MinuteCodeAD))
                    {
                        Form1.admin = true;
                    }
                    else
                    {
                        Form1.user = false;
                        Form1.admin = false;
                    }
                }
                await Task.Delay(500);
            }
        }

        public async static void GiveAdminAccess()
        {
            while (true)
            {
                string DailyCodeAD = GnRodes.GenerateDailyCode("Password600@KK");
                enteredCode = DailyCodeAD;
                await Task.Delay(60*60000);
            }
        }


        public static bool allowKillEdit = true;
        public async static void UserActivationCheck(Panel buttonsPanel, Panel buttons2Panel, Panel buttons3Panel, Button checkeldenRingButton, TextBox adminTextbox, Button killER, Button steamKiller)
        {
            while (true)
            {
                bool isUser = Form1.user;
                bool isAdmin = Form1.admin;
                bool developer = Form1.Developer;
                bool gameRun = ProcessManager.IsProcessRunning("eldenring");
                bool steamRun = ProcessManager.IsProcessRunning("steam");
                bool reading = Form1.StatusReading;

                if (Form1.Developer)
                {
                    adminTextbox.Enabled = true;
                    adminTextbox.Visible = true;
                }
                else
                {
                    adminTextbox.Enabled = false;
                    adminTextbox.Visible = false;
                }

                if ((isUser && !isAdmin) || (!isUser && !isAdmin)) // user
                {
                    Form1.StatusReading = false;
                }
                else if ((isAdmin && !isUser) || (isAdmin && isUser)) //admin or admin & user
                {
                    if (steamRun && allowKillEdit)
                    {
                        steamKiller.Enabled = true;
                    }
                    else
                    {
                        steamKiller.Enabled = false;
                    }
                    if (gameRun)
                    {
                        if (allowKillEdit)
                        {
                            killER.Enabled = true;
                        }
                    }
                    else
                    {
                        killER.Enabled = false;
                        //pathsPanel.Enabled = true;
                        buttonsPanel.Enabled = false;
                        buttons2Panel.Enabled = false;
                    }
                }

                await Task.Delay(500);
            }
        }

    }
}
