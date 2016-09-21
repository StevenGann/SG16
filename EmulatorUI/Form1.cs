﻿using SG16;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmulatorUI
{
    public partial class Form1 : Form
    {
        private string romPath = "";
        private Core core = new Core();
        private Terminal terminal = new Terminal();

        public Form1()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length >= 2)
            {
                if (File.Exists(args[1]) && Path.GetExtension(args[1]) == ".rom")
                {
                    romPath = args[1];
                    tabControl1.SelectedIndex = 1;
                }
            }
            resetCPU();
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            timerMain.Enabled = false;
            tickCPU();
            timerMain.Enabled = true;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            timerMain.Enabled = true;
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            timerMain.Enabled = false;
        }

        public void resetCPU()
        {
            timerMain.Enabled = false;
            core = new Core();
            if (File.Exists(romPath))
            {
                core.LoadROM(0, romPath);
            }

            //Attach terminal to 0xFF00, 0xFF02, and 0xFF04
            terminal = new Terminal();
            terminal.Attach(65280, 65282, 65284, core.RAM);
            core.AttachPeripheral(terminal);

            updateUI();
        }

        private void tickCPU()
        {
            if (core.PC.ToInt() < core.RAM.Data.Length)
            {
                long time = core.Tick();
                StatusLabel.Text = core.Message;
                if (time >= 1)
                {
                    StatusLabel.Text += " - Took " + Convert.ToString(time) + "ms";
                }
            }
            else
            {
                MessageBox.Show("Program Counter has reached the end of RAM");
            }
            updateUI();
        }

        private void updateUI()
        {
            if (tabControl1.SelectedIndex == 0)
            {
                textBoxPC.Text = core.PC.ToString();
                textBoxSTAT.Text = core.STAT.ToString();
                textBoxSUBR.Text = core.SUBR.ToString();
                textBoxPSTR.Text = core.PSTR.ToString();
                textBoxPEND.Text = core.PEND.ToString();
                textBoxRREF.Text = core.RREF.ToString();

                textBoxUSR0.Text = core.USR0.ToString();
                textBoxUSR1.Text = core.USR1.ToString();
                textBoxUSR2.Text = core.USR2.ToString();
                textBoxUSR3.Text = core.USR3.ToString();
                textBoxUSR4.Text = core.USR4.ToString();
                textBoxUSR5.Text = core.USR5.ToString();
                textBoxUSR6.Text = core.USR6.ToString();
                textBoxUSR7.Text = core.USR7.ToString();
                textBoxUSR8.Text = core.USR8.ToString();
                textBoxUSR9.Text = core.USR9.ToString();
                textBoxUSRA.Text = core.USRA.ToString();
                textBoxUSRB.Text = core.USRB.ToString();
                textBoxUSRC.Text = core.USRC.ToString();
                textBoxUSRD.Text = core.USRD.ToString();
                textBoxUSRE.Text = core.USRE.ToString();
                textBoxUSRF.Text = core.USRF.ToString();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                byteViewerRAM.SetBytes(core.RAM.Data);
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                textBoxTerminalOutput.Text = terminal.Text;
            }
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            timerMain.Enabled = false;
            tickCPU();
        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "SG16 Executable Files|*.rom|All Files|*.*";
            openFileDialog1.FilterIndex = 1;

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Open the selected file to read.
                romPath = openFileDialog1.FileName;
                tabControl1.SelectedIndex = 1;
                resetCPU();
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            resetCPU();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateUI();
        }

        private void buttonSerialSend_Click(object sender, EventArgs e)
        {
            if (textBoxTerminalInput.Text.Length > 0)
            {
                terminal.Feed(textBoxTerminalInput.Text);
                textBoxTerminalInput.Text = "";
            }
        }
    }
}