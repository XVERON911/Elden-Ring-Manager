using Elden_Ring_Manager.Resources.Files;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Elden_Ring_Manager
{
    //this is TEST branch
    public partial class Form1 : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = FormManager.SetRandomBackground();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.MouseUp += Panel1_MouseUp;
        }
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position; // Get current mouse position
            dragFormPoint = this.Location; // Get form position
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                // Calculate the new form position
                Point newLocation = new Point(
                    dragFormPoint.X + (Cursor.Position.X - dragCursorPoint.X),
                    dragFormPoint.Y + (Cursor.Position.Y - dragCursorPoint.Y)
                );

                this.Location = newLocation;
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        public static bool imgLoaded = false;
        FormManager FM = new FormManager();


        public static bool user = false;
        public static bool admin = false;
        private async void Form1_Load(object sender, EventArgs e)
        {
            this.FormClosing += formClose;
            if (!FormManager.IsUserAdmin() || !imgLoaded)
            {
                if (!FormManager.IsUserAdmin())
                {
                    MessageBox.Show("YOU ARE NOT RUNNING AS ADMINISTRATOR !", "Admin Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FormManager.DisableAllControls(this);
                    this.Text += " ~~ADMINISTRATOR REQUIRED~~";
                    this.Font = new Font(this.Font, this.Font.Style | FontStyle.Strikeout);
                    CheckER_Result.Text = "ADMIN REQUIRED !";
                    statsReadingLabel.Text = "ADMIN REQUIRED !";
                    CheckER_Result.Enabled = true;
                    statsReadingLabel.Enabled = true;
                    CheckER_Result.BackColor = Color.Firebrick;
                    statsReadingLabel.BackColor = Color.Firebrick;
                }
                if (!imgLoaded)
                    this.Close();
            }
            else
            {
                Buttons_Panel.Enabled = false;
                buttons2Panel.Enabled = false;
                buttons3Panel.Enabled = false;
                var (savedPath1, savedPath2, pass, allow) = ConfigManager.LoadPaths();
                //string activation_C = ConfigManager.LoadActivation();
                //activationTextbox.Text = activation_C;
                eldenRingPathTextbox.Text = savedPath1;
                steamPathTextbox.Text = savedPath2;
                sessionPassTextBox.Text = pass;
                allowInvadersCheckBox.Checked = allow;
                FormManager.Challenger(challengerLabel, animationspeedLabel);
                FormManager.UserActivationCheck(Buttons_Panel, buttons2Panel, buttons3Panel, CheckEldenRingButton, adminTextBox, killERbutton, killSteamButton);
                FormManager.CheckGeneratedCodes(adminTextBox);
                labelupdate();
                ReadstatsButtonUpdate();
                CheckEldenRingDynamically();
                FormManager.GiveAdminAccess();
            }
        }

        MemoryManagement MM;
        bool instanced = false;
        private void Instancee()
        {
            MM = new MemoryManagement();
        }

        public static bool gameRunning = false;

        private async void CheckEldenRingDynamically()
        {
            string original = CheckEldenRingButton.Text;
            string originalTitle = this.Text;
            while (true)
            {
                CheckEldenRingButton.Text = "Checker State: Checking...";
                await Task.Delay(1000);
                if (ProcessManager.IsProcessRunning("steam"))
                {
                    steamRunningLabel.ForeColor = Color.ForestGreen;
                }
                else
                {
                    steamRunningLabel.ForeColor = Color.Firebrick;
                }

                if (ProcessManager.IsProcessRunning("eldenring"))
                {
                    CheckER_Result.Text = "Elden Ring";
                    this.Text = originalTitle + " DO NOT CLOSE THIS APP WHILE GAME IS RUNNING !";
                    CheckER_Result.ForeColor = Color.ForestGreen;
                    CheckEldenRingButton.Text = original;
                    gameRunning = true;
                    if ((admin && !user) || (admin && user))
                    {
                        readStats.Enabled = true;
                    }
                    else
                    {
                        readStats.Enabled = false;
                    }
                    await Task.Delay(5000);
                }
                else
                {
                    CheckEldenRingButton.Text = original;
                    CheckER_Result.Text = "Elden Ring";
                    this.Text = originalTitle;
                    CheckER_Result.ForeColor = Color.Firebrick;
                    readStats.Enabled = false;
                    gameRunning = false;
                }

                if ((!ProcessManager.IsProcessRunning("eldenring") && (user || admin)))
                {
                    CheckEldenRingButton.Text = original;
                    if (allowPathEdit)
                        pathsPanel.Enabled = true;
                }
                else if ((ProcessManager.IsProcessRunning("eldenring") && user) || (ProcessManager.IsProcessRunning("eldenring") && admin))
                {
                    CheckEldenRingButton.Text = original;
                    pathsPanel.Enabled = false;
                }
                await Task.Delay(2000);
            }
        }
        public static bool StatusReading = false;
        private bool calledFunction = false;
        private async void ReadstatsButtonUpdate()
        {


            while (true)
            {
                if (!StatusReading)
                {
                    readStats.Text = "READ";
                    statsReadingLabel.ForeColor = Color.Firebrick;
                    if ((user || admin) && ProcessManager.IsProcessRunning("eldenring"))
                    {

                        if (!user)
                        {
                            statsReadingLabel.Text = "Read State: Not Reading";
                            readStats.Enabled = true;
                        }
                        else
                        {
                            CheckEldenRingButton.Enabled = true;
                            statsReadingLabel.Text = "Read State: Not Reading";
                            //readStats.Enabled = true;
                        }
                    }
                    else if ((user || admin) && !ProcessManager.IsProcessRunning("eldenring"))
                    {
                        if (!user)
                        {
                            statsReadingLabel.Text = "Read State: Not Reading";
                            readStats.Enabled = false;
                        }
                        else
                        {
                            statsReadingLabel.Text = "Read State: Not Reading";
                            //readStats.Enabled = true;
                        }
                    }
                    else
                    {
                        statsReadingLabel.Text = "EXPIRED";
                        readStats.Enabled = false;
                    }
                    readStats.BackColor = Color.Transparent;
                    readStats.FlatAppearance.MouseDownBackColor = Color.Gray;
                    readStats.FlatAppearance.MouseOverBackColor = Color.DimGray;
                    Buttons_Panel.Enabled = false;
                    buttons2Panel.Enabled = false;
                    buttons3Panel.Enabled = false;
                    calledFunction = false;
                }
                else
                {
                    if (!calledFunction)
                    {
                        READALL();
                        calledFunction = true;
                    }
                    readStats.Text = "Stop Reading !";
                    readStats.BackColor = Color.DarkGreen;
                    readStats.FlatAppearance.MouseDownBackColor = Color.Gray;
                    readStats.FlatAppearance.MouseOverBackColor = Color.Firebrick;
                    //Buttons_Panel.Enabled = true;
                    //buttons2Panel.Enabled = true;


                }
                await Task.Delay(500);
            }
        }
        private void readStats_Click(object sender, EventArgs e)
        {
            if (!StatusReading)
            {
                StatusReading = true;
                if (!instanced)
                {
                    Instancee();
                    instanced = true;
                    hptrackBar.Minimum = 1;
                    fptrackBar.Minimum = 1;
                }
            }
            else
            {
                StatusReading = false;
                if (instanced)
                {
                    instanced = false;
                    MM.Dispose();
                }
            }
        }

        private async void READALL()
        {
            statsReadingLabel.Text = "INJECTING...";
            statsReadingLabel.ForeColor = Color.Firebrick;
            await Task.Delay(2000);
            bool sent = false;
            bool read = false;
            FM.Buttons3PanelControl(noattackButton: noAttackButton, noDeadButton: noDeadButton, noFPButton: infiniteFPbutton, noSPbutton: infiniteSPbutton, noDamageButton: noDamageButton, NoMoveButton: noMoveButton, noupdateButton: noUpdateButton);

            while (StatusReading && (admin)) // START READ ALL LOOP
            {
                Buttons_Panel.Enabled = true;
                buttons2Panel.Enabled = true;
                if (Developer)
                {
                    buttons3Panel.Enabled = true;
                    buttons3Panel.Visible = true;
                }
                else
                {
                    buttons3Panel.Enabled = false;
                }


                string characterName = MM.getCharacterName();
                string chararcterLevel = MM.getLevel().ToString();
                string hpText = $"HP : {await MM.GetHP()} / {MM.getMaxHP()}"; hptrackBar.Maximum = MM.getMaxHP(); hptrackBar.Value = await MM.GetHP();
                string fpText = $"FP : {await MM.GetFP()} / {MM.getMaxFP()}"; fptrackBar.Maximum = MM.getMaxFP(); fptrackBar.Value = await MM.GetFP();
                int staminaCurrent = MM.GetStamina() < 0 ? 0 : MM.GetStamina();
                string staminaText = $"Stamina: {staminaCurrent} / {MM.getMaxStamina()}"; staminaTrackBar.Maximum = MM.getMaxStamina(); staminaTrackBar.Value = MM.GetStamina();
                string runesTxt = $"Runes Held: {MM.GetRunes()}";
                string runeArcStatus = $"Rune Arc Status: {(MM.GetRuneArcStatus() ? "Activated" : "De-Activated")}";

                if (MM.GetAnimationSpeed() > 0)
                {
                    animationspeedTrackbar.Value = MM.GetAnimationSpeed();
                }

                if (!read)
                {
                    statsReadingLabel.Text = "Read State: Reading";
                    statsReadingLabel.ForeColor = Color.ForestGreen;
                    read = true;
                }

                if (!sent)
                {
                    MemoryManagement.hpScrollBarValue = hptrackBar.Value;
                    MemoryManagement.fpScrollBarValue = fptrackBar.Value;
                    MemoryManagement.SpScrollBarValue = staminaTrackBar.Value;
                    MemoryManagement.animSpeedScrollbarValue = animationspeedTrackbar.Value;
                    sent = true;
                }
                CharacterNameLabel.Text = $"Character Name: {characterName}";
                charLevelLabel.Text = $"Level: {chararcterLevel}";
                hpLabel.Text = hpText;
                fpLabel.Text = fpText;
                staminaLabel.Text = staminaText;
                runesLabel.Text = runesTxt;
                runeArcStatusLabel.Text = runeArcStatus;
                if (MM.GetRuneArcStatus())
                {
                    runeArcStatusCheckbox.Checked = true;
                }
                else
                {
                    runeArcStatusCheckbox.Checked = false;
                }

                vigorLabel.Text = $"Vigor : {MM.GetAllStats()[0]} / 99"; vigorNumeric.Value = MM.GetAllStats()[0];
                mindLabel.Text = $"Mind : {MM.GetAllStats()[1]} / 99"; mindNumeric.Value = MM.GetAllStats()[1];
                enduranceLabel.Text = $"Endurance : {MM.GetAllStats()[2]} / 99"; enduranceNumeric.Value = MM.GetAllStats()[2];
                strengthLabel.Text = $"Strength : {MM.GetAllStats()[3]} / 99"; strengthNumeric.Value = MM.GetAllStats()[3];
                dexterityLabel.Text = $"Dexterity : {MM.GetAllStats()[4]} / 99"; dexterityNumeric.Value = MM.GetAllStats()[4];
                intLabel.Text = $"Intelligence : {MM.GetAllStats()[5]} / 99"; intNumeric.Value = MM.GetAllStats()[5];
                faithLabel.Text = $"Faith : {MM.GetAllStats()[6]} / 99"; faithNumeric.Value = MM.GetAllStats()[6];
                arcaneLabel.Text = $"Arcane : {MM.GetAllStats()[7]} / 99"; arcaneNumeric.Value = MM.GetAllStats()[7];

                MM.NoDead(true);
                MM.NoDamage(true);
                MM.NoFpConsumption(true);
                MM.NoSpConsumption(true);
                MM.NoAttack(true);
                MM.NoMove(true);
                MM.NoUpdate(true);

                await Task.Delay(200);
            }
        }

        private async void hptrackBar_Scroll(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                if (!freezeHPcheckbox.Checked)
                {
                    MemoryManagement.hpScrollBarValue = hptrackBar.Value;
                    await MM.GetHP(true);
                }
                else
                {
                    MemoryManagement.hpScrollBarValue = hptrackBar.Value;
                }
            }
        }

        private async void fptrackBar_Scroll(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                if (!freezeFPcheckbox.Checked)
                {
                    MemoryManagement.fpScrollBarValue = fptrackBar.Value;
                    await MM.GetFP(true);
                }
                else
                {
                    MemoryManagement.fpScrollBarValue = fptrackBar.Value;
                }
            }
        }

        private void staminaTrackBar_Scroll(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MemoryManagement.SpScrollBarValue = staminaTrackBar.Value;
                MM.GetStamina(true);
            }
        }

        private void animationspeedTrackbar_Scroll(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MemoryManagement.animSpeedScrollbarValue = animationspeedTrackbar.Value;
                MM.GetAnimationSpeed(true);
            }
        }

        int runesInput = 0;
        private void runesInputTextbox_TextChanged(object sender, EventArgs e) // ensure only numbers
        {
            if (StatusReading)
            {
                string filteredText = new string(runesInputTextbox.Text.Where(char.IsDigit).ToArray());

                if (runesInputTextbox.Text != filteredText)
                {
                    int cursorPosition = runesInputTextbox.SelectionStart - 1;
                    runesInputTextbox.Text = filteredText;
                    runesInputTextbox.SelectionStart = Math.Max(cursorPosition, 0);
                }
                if (int.TryParse(runesInputTextbox.Text, out int parsedValue))
                {
                    runesInput = parsedValue;
                }
                else
                {
                    runesInput = 0;
                }
            }
        }

        private void setRunes_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.GetRunes(true, runesInput);
            }
        }

        private void freezeHPcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                if (freezeHPcheckbox.Checked)
                {
                    MemoryManagement.freezeHP = true;
                    MM.FreezeHP();
                    //await MM.GetHP();
                }
                else if (!freezeHPcheckbox.Checked)
                {
                    MemoryManagement.freezeHP = false;
                    //await MM.GetHP();
                }
            }
            else
            {
                if (freezeHPcheckbox.Checked)
                {
                    freezeHPcheckbox.Checked = false;
                }
                else
                {
                    freezeHPcheckbox.Checked = true;
                }
            }
        }

        private void freezeFPcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                if (freezeFPcheckbox.Checked)
                {
                    MemoryManagement.freezeFP = true;
                    MM.FreezeFP();
                    //await MM.GetFP();
                }
                else if (!freezeFPcheckbox.Checked)
                {
                    MemoryManagement.freezeFP = false;
                    //await MM.GetFP();
                }
            }
            else
            {
                if (freezeFPcheckbox.Checked)
                {
                    freezeFPcheckbox.Checked = false;
                }
                else
                {
                    freezeFPcheckbox.Checked = true;
                }
            }
        }

        private void runeArcStatusCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                if (!MM.GetRuneArcStatus() && runeArcStatusCheckbox.Checked == true)
                {
                    MM.GetRuneArcStatus(on: true);
                }
                else if (MM.GetRuneArcStatus() && runeArcStatusCheckbox.Checked == false)
                {
                    MM.GetRuneArcStatus(off: true);
                }
            }
        }

        private void vigorNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x3C, (int)vigorNumeric.Value);
            }
        }

        private void mindNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x40, (int)mindNumeric.Value);
            }
        }

        private void enduranceNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x44, (int)enduranceNumeric.Value);
            }
        }

        private void strengthNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x48, (int)strengthNumeric.Value);
            }
        }

        private void dexterityNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x4C, (int)dexterityNumeric.Value);
            }
        }

        private void intNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x50, (int)intNumeric.Value);
            }
        }

        private void faithNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x54, (int)faithNumeric.Value);
            }
        }

        private void arcaneNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.SetStatus(0x58, (int)arcaneNumeric.Value);
            }
        }

        public static bool allowPathEdit = true;
        private async void LaunchSeamlessButton_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            string path1 = eldenRingPathTextbox.Text;
            string path2 = steamPathTextbox.Text;
            string pass = "246800";
            string activationCode = activationTextbox.Text;
            allowPathEdit = false;
            launchSeamlessButton.BackColor = Color.Firebrick;
            launchSeamlessButton.Text = "Assembling Bootstrap";
            pathsPanel.Enabled = false;
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.Text = "Generating XL C++";
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.Text = "Injecting Elden Ring..";
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.Text = "Generating Binaries";
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.Text = "Injecting Seamless Coop";
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.Text = "Executing Rules...";
            await Task.Delay(r.Next(1000, 5000));
            launchSeamlessButton.BackColor = Color.Transparent;
            if ((string.IsNullOrWhiteSpace(path1) || string.IsNullOrWhiteSpace(path2)))
            {
                MessageBox.Show("Enter Game Path and Steam Path Correctly !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (allowInvadersCheckBox.Checked || !String.IsNullOrWhiteSpace(sessionPassTextBox.Text))
                {
                    pass = sessionPassTextBox.Text;
                    ProcessManager.UpdateInfFile((Directory.GetCurrentDirectory()) + "\\source\\SeamlessCoop\\ersc_settings.ini", allowInvadersCheckBox, pass);
                }

                string ERpath = eldenRingPathTextbox.Text;
                string STpath = steamPathTextbox.Text;
                if (!Directory.Exists(path1) || !Directory.Exists(path2))
                {
                    MessageBox.Show("Invalid directories. Please enter correct paths.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ConfigManager.SavePaths(path1, path2, pass, allowInvadersCheckBox.Checked);
                terminalTextBox.Text += "Preferences Saved !" + Environment.NewLine;
                terminalTextBox.Enabled = true;
                terminalTextBox.Visible = true;
                //terminalTextBox.Focus();
                ProcessManager.launchSeamlessCoop(path1, path2, terminalTextBox, this, pass, pathsPanel, killERbutton, killSteamButton);
                FormManager.launchButtonControl(launchSeamlessButton, pathsPanel);
            }
        }

        private void challengerLabel_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = FormManager.SetRandomBackground();
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void noDeadButton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoDead();
            }
        }

        private void noDamageButton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoDamage();
            }
        }

        private void infiniteFPbutton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoFpConsumption();
            }
        }

        private void infiniteSPbutton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoSpConsumption();
            }
        }
        private void noAttackButton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoAttack();
            }
        }
        private void noMoveButton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoMove();
            }
        }
        private void noUpdateButton_Click(object sender, EventArgs e)
        {
            if (StatusReading)
            {
                MM.NoUpdate();
            }
        }

        public static bool Developer = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (!user)
            {
                user = true;
            }
            else
            {
                user = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!admin)
            {
                admin = true;
            }
            else
            {
                admin = false;
            }
        }

        private async void labelupdate()
        {
            while (!isShowingActivationError)
            {
                if (Developer)
                {
                    label1.Text = $"Status: {(Developer ? "DEVELOPER ACCESS" : "")}".ToUpper();
                    activateButton.Enabled = false;
                    activationTextbox.Enabled = false;
                    label1.BackColor = Color.Black;
                    label1.ForeColor = Color.White;
                }
                else if (user && !admin)
                {
                    label1.Text = $"Status: {(user ? "Seamless Coop Access" : "")}".ToUpper();
                    activateButton.Enabled = false;
                    activationTextbox.Enabled = false;
                    label1.BackColor = Color.ForestGreen;
                }
                else if (admin && !user)
                {
                    label1.Text = $"Status: {(admin ? "Features Access" : "")}".ToUpper();
                    activateButton.Enabled = false;
                    activationTextbox.Enabled = false;
                    label1.BackColor = Color.ForestGreen;
                }
                else if (admin && user)
                {
                    label1.Text = $"Status: Features Access".ToUpper();
                    activateButton.Enabled = false;
                    activationTextbox.Enabled = false;
                    label1.BackColor = Color.ForestGreen;
                }
                else
                {
                    label1.Text = "Status: Expired".ToUpper();
                    if (FormManager.IsUserAdmin())
                    {
                        activationTextbox.Enabled = true;
                        activateButton.Enabled = true;
                    }
                    label1.BackColor = Color.Firebrick;
                }

                await Task.Delay(500);
            }
        }

        private static bool isShowingActivationError = false;
        private async void activate()
        {
            if (!String.IsNullOrEmpty(activationTextbox.Text)) //remove spaces
            {
                string aC = activationTextbox.Text;
                ConfigManager.SaveActiveCode(aC);
                string code = activationTextbox.Text.Replace(" ", "");
                isShowingActivationError = true;
                label1.Text = "Status: Checking...".ToUpper();
                await Task.Delay(2000);
                isShowingActivationError = false;
                FormManager.enteredCode = code;
                await Task.Delay(1000);
                if ((!user && !admin) && !isShowingActivationError)
                {
                    isShowingActivationError = true;
                    label1.Text = "Status: Wrong Activation Code".ToUpper();
                    await Task.Delay(2000);
                    isShowingActivationError = false;
                    labelupdate();
                }
                labelupdate();
            }
            else
            {
                if (!isShowingActivationError)
                {
                    isShowingActivationError = true;
                    label1.Text = "Status: Enter Activation Code First".ToUpper();
                    await Task.Delay(2000);
                    isShowingActivationError = false;
                    labelupdate();
                }
            }
        }

        private void activateButton_Click(object sender, EventArgs e)
        {
            activate();
        }

        private void activationTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter))
            {
                activate();
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private async void activationTextbox_TextChanged(object sender, EventArgs e)
        {
            if (activationTextbox.Text == "ADMINISTRATOR")
            {
                activationTextbox.PasswordChar = '*';
                activationTextbox.Clear();
                isShowingActivationError = true;
                label1.Text = "Status: Checking...".ToUpper();
                await Task.Delay(5000);
                isShowingActivationError = false;
                labelupdate();
                string pass = activationTextbox.Text;
                if (pass == "@800AK47//")
                {
                    Developer = true;
                    FormManager.GiveAdminAccess();
                }
                else
                {
                    Developer = false;
                }
            }
        }

        private async void killERbutton_Click(object sender, EventArgs e) // killer button & form close
        {
            string orig = killERbutton.Text;
            if (ProcessManager.IsProcessRunning("eldenring"))
            {
                killERbutton.Text = "Killing...";
                ProcessManager.killProcess("eldenring");
                await Task.Delay(1000);
                killERbutton.Text = "Killed Elden Ring";
                await Task.Delay(2000);
                killERbutton.Text = orig;
            }
        }

        private void formClose(object sender, EventArgs e) // killer button & form close
        {
            if (ProcessManager.IsProcessRunning("eldenring"))
            {
                //ProcessManager.killProcess("eldenring");
            }
        }

        private async void killSteamButton_Click(object sender, EventArgs e)
        {
            string orig = killSteamButton.Text;
            if (ProcessManager.IsProcessRunning("steam"))
            {
                killSteamButton.Text = "Killing...";
                ProcessManager.ForceCloseSteam();
                await Task.Delay(1000);
                killSteamButton.Text = "Killed Steam";
                await Task.Delay(2000);
                killSteamButton.Text = orig;
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool maximized = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!maximized)
            {
                this.Size = new Size(900, 600);
                maximized = true;
            }
            else
            {
                this.Size = new Size(856, 522);
                maximized = false;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
