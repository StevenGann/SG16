
namespace EmulatorUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStep = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.textBoxPC = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxUSRF = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.textBoxUSRE = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxUSRD = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxUSRC = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxUSRB = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxUSRA = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxUSR9 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxUSR8 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxUSR7 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxUSR6 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxUSR5 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxUSR4 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxUSR3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxUSR2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxUSR1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxUSR0 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxRREF = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPEND = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPSTR = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSUBR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSTAT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.byteViewerRAM = new System.ComponentModel.Design.ByteViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 638);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(652, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(652, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openROMToolStripMenuItem
            // 
            this.openROMToolStripMenuItem.Name = "openROMToolStripMenuItem";
            this.openROMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openROMToolStripMenuItem.Text = "Open ROM";
            this.openROMToolStripMenuItem.Click += new System.EventHandler(this.openROMToolStripMenuItem_Click);
            // 
            // timerMain
            // 
            this.timerMain.Interval = 5;
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(3, 3);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 2;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(84, 3);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 3;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStep
            // 
            this.buttonStep.Location = new System.Drawing.Point(165, 3);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(75, 23);
            this.buttonStep.TabIndex = 4;
            this.buttonStep.Text = "Step";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(246, 3);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // textBoxPC
            // 
            this.textBoxPC.Location = new System.Drawing.Point(9, 6);
            this.textBoxPC.Name = "textBoxPC";
            this.textBoxPC.ReadOnly = true;
            this.textBoxPC.Size = new System.Drawing.Size(47, 20);
            this.textBoxPC.TabIndex = 6;
            this.textBoxPC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "PC";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(652, 585);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxUSRF);
            this.tabPage1.Controls.Add(this.textBoxRREF);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Controls.Add(this.textBoxUSRE);
            this.tabPage1.Controls.Add(this.label21);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.textBoxPEND);
            this.tabPage1.Controls.Add(this.textBoxUSRD);
            this.tabPage1.Controls.Add(this.textBoxPC);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxUSRC);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.textBoxPSTR);
            this.tabPage1.Controls.Add(this.textBoxUSRB);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.textBoxUSRA);
            this.tabPage1.Controls.Add(this.textBoxSTAT);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.textBoxSUBR);
            this.tabPage1.Controls.Add(this.textBoxUSR9);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.textBoxUSR0);
            this.tabPage1.Controls.Add(this.textBoxUSR8);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.textBoxUSR7);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.textBoxUSR1);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.textBoxUSR6);
            this.tabPage1.Controls.Add(this.textBoxUSR2);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.textBoxUSR5);
            this.tabPage1.Controls.Add(this.textBoxUSR3);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.textBoxUSR4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(644, 559);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Registers";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxUSRF
            // 
            this.textBoxUSRF.Location = new System.Drawing.Point(203, 188);
            this.textBoxUSRF.Name = "textBoxUSRF";
            this.textBoxUSRF.ReadOnly = true;
            this.textBoxUSRF.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRF.TabIndex = 48;
            this.textBoxUSRF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(256, 191);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(36, 13);
            this.label22.TabIndex = 49;
            this.label22.Text = "USRF";
            // 
            // textBoxUSRE
            // 
            this.textBoxUSRE.Location = new System.Drawing.Point(203, 162);
            this.textBoxUSRE.Name = "textBoxUSRE";
            this.textBoxUSRE.ReadOnly = true;
            this.textBoxUSRE.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRE.TabIndex = 46;
            this.textBoxUSRE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(256, 165);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(37, 13);
            this.label21.TabIndex = 47;
            this.label21.Text = "USRE";
            // 
            // textBoxUSRD
            // 
            this.textBoxUSRD.Location = new System.Drawing.Point(203, 136);
            this.textBoxUSRD.Name = "textBoxUSRD";
            this.textBoxUSRD.ReadOnly = true;
            this.textBoxUSRD.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRD.TabIndex = 44;
            this.textBoxUSRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(256, 139);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(38, 13);
            this.label20.TabIndex = 45;
            this.label20.Text = "USRD";
            // 
            // textBoxUSRC
            // 
            this.textBoxUSRC.Location = new System.Drawing.Point(203, 113);
            this.textBoxUSRC.Name = "textBoxUSRC";
            this.textBoxUSRC.ReadOnly = true;
            this.textBoxUSRC.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRC.TabIndex = 42;
            this.textBoxUSRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(256, 116);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(37, 13);
            this.label19.TabIndex = 43;
            this.label19.Text = "USRC";
            // 
            // textBoxUSRB
            // 
            this.textBoxUSRB.Location = new System.Drawing.Point(203, 87);
            this.textBoxUSRB.Name = "textBoxUSRB";
            this.textBoxUSRB.ReadOnly = true;
            this.textBoxUSRB.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRB.TabIndex = 40;
            this.textBoxUSRB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(256, 90);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(37, 13);
            this.label18.TabIndex = 41;
            this.label18.Text = "USRB";
            // 
            // textBoxUSRA
            // 
            this.textBoxUSRA.Location = new System.Drawing.Point(203, 58);
            this.textBoxUSRA.Name = "textBoxUSRA";
            this.textBoxUSRA.ReadOnly = true;
            this.textBoxUSRA.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSRA.TabIndex = 38;
            this.textBoxUSRA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(256, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(37, 13);
            this.label17.TabIndex = 39;
            this.label17.Text = "USRA";
            // 
            // textBoxUSR9
            // 
            this.textBoxUSR9.Location = new System.Drawing.Point(203, 32);
            this.textBoxUSR9.Name = "textBoxUSR9";
            this.textBoxUSR9.ReadOnly = true;
            this.textBoxUSR9.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR9.TabIndex = 36;
            this.textBoxUSR9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(256, 35);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "USR9";
            // 
            // textBoxUSR8
            // 
            this.textBoxUSR8.Location = new System.Drawing.Point(203, 6);
            this.textBoxUSR8.Name = "textBoxUSR8";
            this.textBoxUSR8.ReadOnly = true;
            this.textBoxUSR8.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR8.TabIndex = 34;
            this.textBoxUSR8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(256, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 35;
            this.label15.Text = "USR8";
            // 
            // textBoxUSR7
            // 
            this.textBoxUSR7.Location = new System.Drawing.Point(108, 188);
            this.textBoxUSR7.Name = "textBoxUSR7";
            this.textBoxUSR7.ReadOnly = true;
            this.textBoxUSR7.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR7.TabIndex = 32;
            this.textBoxUSR7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(161, 191);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "USR7";
            // 
            // textBoxUSR6
            // 
            this.textBoxUSR6.Location = new System.Drawing.Point(108, 162);
            this.textBoxUSR6.Name = "textBoxUSR6";
            this.textBoxUSR6.ReadOnly = true;
            this.textBoxUSR6.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR6.TabIndex = 30;
            this.textBoxUSR6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(161, 165);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "USR6";
            // 
            // textBoxUSR5
            // 
            this.textBoxUSR5.Location = new System.Drawing.Point(108, 136);
            this.textBoxUSR5.Name = "textBoxUSR5";
            this.textBoxUSR5.ReadOnly = true;
            this.textBoxUSR5.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR5.TabIndex = 28;
            this.textBoxUSR5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(161, 139);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "USR5";
            // 
            // textBoxUSR4
            // 
            this.textBoxUSR4.Location = new System.Drawing.Point(108, 110);
            this.textBoxUSR4.Name = "textBoxUSR4";
            this.textBoxUSR4.ReadOnly = true;
            this.textBoxUSR4.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR4.TabIndex = 26;
            this.textBoxUSR4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(161, 113);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "USR4";
            // 
            // textBoxUSR3
            // 
            this.textBoxUSR3.Location = new System.Drawing.Point(108, 84);
            this.textBoxUSR3.Name = "textBoxUSR3";
            this.textBoxUSR3.ReadOnly = true;
            this.textBoxUSR3.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR3.TabIndex = 24;
            this.textBoxUSR3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(161, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "USR3";
            // 
            // textBoxUSR2
            // 
            this.textBoxUSR2.Location = new System.Drawing.Point(108, 58);
            this.textBoxUSR2.Name = "textBoxUSR2";
            this.textBoxUSR2.ReadOnly = true;
            this.textBoxUSR2.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR2.TabIndex = 22;
            this.textBoxUSR2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(161, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "USR2";
            // 
            // textBoxUSR1
            // 
            this.textBoxUSR1.Location = new System.Drawing.Point(108, 32);
            this.textBoxUSR1.Name = "textBoxUSR1";
            this.textBoxUSR1.ReadOnly = true;
            this.textBoxUSR1.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR1.TabIndex = 20;
            this.textBoxUSR1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(161, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "USR1";
            // 
            // textBoxUSR0
            // 
            this.textBoxUSR0.Location = new System.Drawing.Point(108, 6);
            this.textBoxUSR0.Name = "textBoxUSR0";
            this.textBoxUSR0.ReadOnly = true;
            this.textBoxUSR0.Size = new System.Drawing.Size(47, 20);
            this.textBoxUSR0.TabIndex = 18;
            this.textBoxUSR0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(161, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "USR0";
            // 
            // textBoxRREF
            // 
            this.textBoxRREF.Location = new System.Drawing.Point(9, 136);
            this.textBoxRREF.Name = "textBoxRREF";
            this.textBoxRREF.ReadOnly = true;
            this.textBoxRREF.Size = new System.Drawing.Size(47, 20);
            this.textBoxRREF.TabIndex = 16;
            this.textBoxRREF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "RREF";
            // 
            // textBoxPEND
            // 
            this.textBoxPEND.Location = new System.Drawing.Point(9, 110);
            this.textBoxPEND.Name = "textBoxPEND";
            this.textBoxPEND.ReadOnly = true;
            this.textBoxPEND.Size = new System.Drawing.Size(47, 20);
            this.textBoxPEND.TabIndex = 14;
            this.textBoxPEND.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "PEND";
            // 
            // textBoxPSTR
            // 
            this.textBoxPSTR.Location = new System.Drawing.Point(9, 84);
            this.textBoxPSTR.Name = "textBoxPSTR";
            this.textBoxPSTR.ReadOnly = true;
            this.textBoxPSTR.Size = new System.Drawing.Size(47, 20);
            this.textBoxPSTR.TabIndex = 12;
            this.textBoxPSTR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "PSTR";
            // 
            // textBoxSUBR
            // 
            this.textBoxSUBR.Location = new System.Drawing.Point(9, 58);
            this.textBoxSUBR.Name = "textBoxSUBR";
            this.textBoxSUBR.ReadOnly = true;
            this.textBoxSUBR.Size = new System.Drawing.Size(47, 20);
            this.textBoxSUBR.TabIndex = 10;
            this.textBoxSUBR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "SUBR";
            // 
            // textBoxSTAT
            // 
            this.textBoxSTAT.Location = new System.Drawing.Point(9, 32);
            this.textBoxSTAT.Name = "textBoxSTAT";
            this.textBoxSTAT.ReadOnly = true;
            this.textBoxSTAT.Size = new System.Drawing.Size(47, 20);
            this.textBoxSTAT.TabIndex = 8;
            this.textBoxSTAT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "STAT";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.byteViewerRAM);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(644, 559);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RAM";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // byteViewerRAM
            // 
            this.byteViewerRAM.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.byteViewerRAM.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.byteViewerRAM.ColumnCount = 1;
            this.byteViewerRAM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewerRAM.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewerRAM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.byteViewerRAM.Location = new System.Drawing.Point(3, 3);
            this.byteViewerRAM.Name = "byteViewerRAM";
            this.byteViewerRAM.RowCount = 1;
            this.byteViewerRAM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewerRAM.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.byteViewerRAM.Size = new System.Drawing.Size(638, 553);
            this.byteViewerRAM.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRun);
            this.panel1.Controls.Add(this.buttonPause);
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.buttonStep);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 29);
            this.panel1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 660);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SG16 Emulator";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openROMToolStripMenuItem;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TextBox textBoxPC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxRREF;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPEND;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPSTR;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSUBR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSTAT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUSR0;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxUSRF;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxUSRE;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxUSRD;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxUSRC;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxUSRB;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBoxUSRA;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxUSR9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxUSR8;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxUSR7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxUSR6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxUSR5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxUSR4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxUSR3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxUSR2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxUSR1;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.Design.ByteViewer byteViewerRAM;
        private System.Windows.Forms.Panel panel1;
    }
}

