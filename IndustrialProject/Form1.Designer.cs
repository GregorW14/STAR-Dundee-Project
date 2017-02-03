using System.Windows.Forms;

namespace IndustrialProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataRateOverTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorLocationsInTheTrafficToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unexpectedDataValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packetRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxConsole = new System.Windows.Forms.GroupBox();
            this.txtConsoleLog = new System.Windows.Forms.RichTextBox();
            this.backgroundWorkerLoadFile = new System.ComponentModel.BackgroundWorker();
            this.consoleSavingWorker = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.packetCountLabel = new System.Windows.Forms.Label();
            this.errorCountLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.lblAverageDataRate = new System.Windows.Forms.Label();
            this.packetListView = new System.Windows.Forms.ListView();
            this.nextErrorButton = new System.Windows.Forms.Button();
            this.buttonLoadBrowser = new System.Windows.Forms.Button();
            this.packetContentTextBox = new System.Windows.Forms.RichTextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.backgroundWorkerCreateGraph = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxTutorialImage = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new IndustrialProject.TabControlWithExit();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxOV2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxOV = new System.Windows.Forms.RichTextBox();
            this.listViewOV = new IndustrialProject.PacketListView();
            this.groupBoxPort1 = new System.Windows.Forms.GroupBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPacketRate_Result = new System.Windows.Forms.Label();
            this.lblPacketRate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.averageDataRateLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.groupBoxConsole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTutorialImage)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxOV2.SuspendLayout();
            this.groupBoxPort1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.viewToolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1370, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.openToolStripMenuItem.Text = "Open recorded traffic file(s)";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(259, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("quitToolStripMenuItem.Image")));
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataRateOverTimeToolStripMenuItem,
            this.errorLocationsInTheTrafficToolStripMenuItem,
            this.unexpectedDataValuesToolStripMenuItem,
            this.packetRateToolStripMenuItem});
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem1.Text = "View";
            // 
            // dataRateOverTimeToolStripMenuItem
            // 
            this.dataRateOverTimeToolStripMenuItem.Name = "dataRateOverTimeToolStripMenuItem";
            this.dataRateOverTimeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.dataRateOverTimeToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.dataRateOverTimeToolStripMenuItem.Text = "Data rate over time";
            this.dataRateOverTimeToolStripMenuItem.Click += new System.EventHandler(this.dataRateOverTimeToolStripMenuItem_Click);
            // 
            // errorLocationsInTheTrafficToolStripMenuItem
            // 
            this.errorLocationsInTheTrafficToolStripMenuItem.Name = "errorLocationsInTheTrafficToolStripMenuItem";
            this.errorLocationsInTheTrafficToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.errorLocationsInTheTrafficToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.errorLocationsInTheTrafficToolStripMenuItem.Text = "Error locations in the traffic";
            this.errorLocationsInTheTrafficToolStripMenuItem.Click += new System.EventHandler(this.errorLocationsInTeTrafficToolStripMenuItem_Click);
            // 
            // unexpectedDataValuesToolStripMenuItem
            // 
            this.unexpectedDataValuesToolStripMenuItem.Name = "unexpectedDataValuesToolStripMenuItem";
            this.unexpectedDataValuesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.unexpectedDataValuesToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.unexpectedDataValuesToolStripMenuItem.Text = "Unexpected data values";
            this.unexpectedDataValuesToolStripMenuItem.Click += new System.EventHandler(this.unexpectedDataValuesToolStripMenuItem_Click);
            // 
            // packetRateToolStripMenuItem
            // 
            this.packetRateToolStripMenuItem.Name = "packetRateToolStripMenuItem";
            this.packetRateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.packetRateToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.packetRateToolStripMenuItem.Text = "Packet rate";
            this.packetRateToolStripMenuItem.Click += new System.EventHandler(this.packetRateToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManualToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // userManualToolStripMenuItem
            // 
            this.userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            this.userManualToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F12)));
            this.userManualToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.userManualToolStripMenuItem.Text = "User manual";
            this.userManualToolStripMenuItem.Click += new System.EventHandler(this.userManualToolStripMenuItem_Click_1);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBoxConsole
            // 
            this.groupBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConsole.Controls.Add(this.txtConsoleLog);
            this.groupBoxConsole.Location = new System.Drawing.Point(260, 654);
            this.groupBoxConsole.Name = "groupBoxConsole";
            this.groupBoxConsole.Size = new System.Drawing.Size(1094, 84);
            this.groupBoxConsole.TabIndex = 11;
            this.groupBoxConsole.TabStop = false;
            this.groupBoxConsole.Text = "Output Console";
            // 
            // txtConsoleLog
            // 
            this.txtConsoleLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsoleLog.BackColor = System.Drawing.Color.White;
            this.txtConsoleLog.Location = new System.Drawing.Point(10, 19);
            this.txtConsoleLog.Name = "txtConsoleLog";
            this.txtConsoleLog.ReadOnly = true;
            this.txtConsoleLog.Size = new System.Drawing.Size(946, 59);
            this.txtConsoleLog.TabIndex = 6;
            this.txtConsoleLog.Text = "";
            // 
            // backgroundWorkerLoadFile
            // 
            this.backgroundWorkerLoadFile.WorkerReportsProgress = true;
            this.backgroundWorkerLoadFile.WorkerSupportsCancellation = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 1;
            // 
            // packetCountLabel
            // 
            this.packetCountLabel.AutoSize = true;
            this.packetCountLabel.Location = new System.Drawing.Point(123, 26);
            this.packetCountLabel.Name = "packetCountLabel";
            this.packetCountLabel.Size = new System.Drawing.Size(0, 13);
            this.packetCountLabel.TabIndex = 2;
            // 
            // errorCountLabel
            // 
            this.errorCountLabel.AutoSize = true;
            this.errorCountLabel.Location = new System.Drawing.Point(152, 51);
            this.errorCountLabel.Name = "errorCountLabel";
            this.errorCountLabel.Size = new System.Drawing.Size(0, 13);
            this.errorCountLabel.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 6;
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Location = new System.Drawing.Point(68, 76);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(0, 13);
            this.startTimeLabel.TabIndex = 7;
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Location = new System.Drawing.Point(68, 99);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(0, 13);
            this.endTimeLabel.TabIndex = 8;
            // 
            // lblAverageDataRate
            // 
            this.lblAverageDataRate.AutoSize = true;
            this.lblAverageDataRate.Location = new System.Drawing.Point(111, 123);
            this.lblAverageDataRate.Name = "lblAverageDataRate";
            this.lblAverageDataRate.Size = new System.Drawing.Size(0, 13);
            this.lblAverageDataRate.TabIndex = 9;
            // 
            // packetListView
            // 
            this.packetListView.FullRowSelect = true;
            this.packetListView.Location = new System.Drawing.Point(6, 19);
            this.packetListView.MultiSelect = false;
            this.packetListView.Name = "packetListView";
            this.packetListView.Size = new System.Drawing.Size(661, 186);
            this.packetListView.TabIndex = 10;
            this.packetListView.UseCompatibleStateImageBehavior = false;
            // 
            // nextErrorButton
            // 
            this.nextErrorButton.Location = new System.Drawing.Point(592, 211);
            this.nextErrorButton.Name = "nextErrorButton";
            this.nextErrorButton.Size = new System.Drawing.Size(75, 23);
            this.nextErrorButton.TabIndex = 15;
            this.nextErrorButton.Tag = "#";
            this.nextErrorButton.Text = "Next Error";
            this.nextErrorButton.UseVisualStyleBackColor = true;
            // 
            // buttonLoadBrowser
            // 
            this.buttonLoadBrowser.Location = new System.Drawing.Point(459, 203);
            this.buttonLoadBrowser.Name = "buttonLoadBrowser";
            this.buttonLoadBrowser.Size = new System.Drawing.Size(93, 23);
            this.buttonLoadBrowser.TabIndex = 15;
            this.buttonLoadBrowser.Text = "Load browser";
            this.buttonLoadBrowser.UseVisualStyleBackColor = true;
            // 
            // packetContentTextBox
            // 
            this.packetContentTextBox.Location = new System.Drawing.Point(6, 19);
            this.packetContentTextBox.Name = "packetContentTextBox";
            this.packetContentTextBox.Size = new System.Drawing.Size(689, 215);
            this.packetContentTextBox.TabIndex = 8;
            this.packetContentTextBox.Text = "";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(6, 19);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(683, 264);
            this.webBrowser1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::IndustrialProject.Properties.Resources.STAR;
            this.pictureBox1.Location = new System.Drawing.Point(12, 654);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(242, 84);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxTutorialImage
            // 
            this.pictureBoxTutorialImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxTutorialImage.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBoxTutorialImage.BackgroundImage = global::IndustrialProject.Properties.Resources.tutorial;
            this.pictureBoxTutorialImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxTutorialImage.Location = new System.Drawing.Point(0, 27);
            this.pictureBoxTutorialImage.Name = "pictureBoxTutorialImage";
            this.pictureBoxTutorialImage.Size = new System.Drawing.Size(1370, 720);
            this.pictureBoxTutorialImage.TabIndex = 27;
            this.pictureBoxTutorialImage.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(21, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1342, 590);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxOV2);
            this.tabPage1.Controls.Add(this.groupBoxPort1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1334, 564);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Overview";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBoxOV2
            // 
            this.groupBoxOV2.Controls.Add(this.richTextBoxOV);
            this.groupBoxOV2.Controls.Add(this.listViewOV);
            this.groupBoxOV2.Location = new System.Drawing.Point(874, 6);
            this.groupBoxOV2.Name = "groupBoxOV2";
            this.groupBoxOV2.Size = new System.Drawing.Size(629, 583);
            this.groupBoxOV2.TabIndex = 1;
            this.groupBoxOV2.TabStop = false;
            this.groupBoxOV2.Text = "Packet List";
            // 
            // richTextBoxOV
            // 
            this.richTextBoxOV.Location = new System.Drawing.Point(6, 363);
            this.richTextBoxOV.Name = "richTextBoxOV";
            this.richTextBoxOV.Size = new System.Drawing.Size(617, 214);
            this.richTextBoxOV.TabIndex = 1;
            this.richTextBoxOV.Text = "";
            // 
            // listViewOV
            // 
            this.listViewOV.Location = new System.Drawing.Point(7, 20);
            this.listViewOV.Name = "listViewOV";
            this.listViewOV.Size = new System.Drawing.Size(616, 337);
            this.listViewOV.TabIndex = 0;
            this.listViewOV.UseCompatibleStateImageBehavior = false;
            // 
            // groupBoxPort1
            // 
            this.groupBoxPort1.Controls.Add(this.webBrowser);
            this.groupBoxPort1.Location = new System.Drawing.Point(6, 6);
            this.groupBoxPort1.Name = "groupBoxPort1";
            this.groupBoxPort1.Size = new System.Drawing.Size(868, 579);
            this.groupBoxPort1.TabIndex = 0;
            this.groupBoxPort1.TabStop = false;
            this.groupBoxPort1.Text = "Visualisation";
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(6, 19);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(856, 554);
            this.webBrowser.TabIndex = 10;
            this.webBrowser.TabStop = false;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox13);
            this.tabPage2.Controls.Add(this.groupBox11);
            this.tabPage2.Controls.Add(this.groupBox12);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1334, 564);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Port 1";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.richTextBox1);
            this.groupBox13.Location = new System.Drawing.Point(694, 363);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(626, 195);
            this.groupBox13.TabIndex = 18;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Packet contents";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(610, 170);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox11.Controls.Add(this.button1);
            this.groupBox11.Controls.Add(this.listView1);
            this.groupBox11.Location = new System.Drawing.Point(6, 195);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(676, 363);
            this.groupBox11.TabIndex = 17;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Packet list";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(592, 334);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Tag = "#";
            this.button1.Text = "Next Error";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(6, 19);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(664, 342);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Location = new System.Drawing.Point(694, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(626, 351);
            this.groupBox12.TabIndex = 16;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Visualisation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPacketRate_Result);
            this.groupBox1.Controls.Add(this.lblPacketRate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.averageDataRateLabel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(676, 183);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // lblPacketRate_Result
            // 
            this.lblPacketRate_Result.AutoSize = true;
            this.lblPacketRate_Result.Location = new System.Drawing.Point(100, 146);
            this.lblPacketRate_Result.Name = "lblPacketRate_Result";
            this.lblPacketRate_Result.Size = new System.Drawing.Size(0, 13);
            this.lblPacketRate_Result.TabIndex = 12;
            // 
            // lblPacketRate
            // 
            this.lblPacketRate.AutoSize = true;
            this.lblPacketRate.Location = new System.Drawing.Point(18, 147);
            this.lblPacketRate.Name = "lblPacketRate";
            this.lblPacketRate.Size = new System.Drawing.Size(127, 13);
            this.lblPacketRate.TabIndex = 11;
            this.lblPacketRate.Text = "Packet rate (per second):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(111, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 10;
            // 
            // averageDataRateLabel
            // 
            this.averageDataRateLabel.AutoSize = true;
            this.averageDataRateLabel.Location = new System.Drawing.Point(111, 123);
            this.averageDataRateLabel.Name = "averageDataRateLabel";
            this.averageDataRateLabel.Size = new System.Drawing.Size(0, 13);
            this.averageDataRateLabel.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(68, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(68, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 123);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Average data rate:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "End time:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Start time:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(152, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 13);
            this.label12.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(123, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 13);
            this.label13.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(129, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Number of packets errors:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Number of packets:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.pictureBoxTutorialImage);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBoxConsole);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(961, 660);
            this.Name = "Form1";
            this.Text = "SpaceWire Recorder Analyser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Form1_Layout);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxConsole.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTutorialImage)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxOV2.ResumeLayout(false);
            this.groupBoxPort1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private TabControlWithExit tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBoxConsole;
        private System.Windows.Forms.RichTextBox txtConsoleLog;
        private System.Windows.Forms.GroupBox groupBoxPort1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataRateOverTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorLocationsInTheTrafficToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unexpectedDataValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packetRateToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadFile;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TabPage tabPage2;
        private System.ComponentModel.BackgroundWorker consoleSavingWorker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label packetCountLabel;
        private System.Windows.Forms.Label errorCountLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.Label lblAverageDataRate;
        private System.Windows.Forms.ListView packetListView;
        private System.Windows.Forms.Button nextErrorButton;
        private System.Windows.Forms.Button buttonLoadBrowser;
        private System.Windows.Forms.RichTextBox packetContentTextBox;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label averageDataRateLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblPacketRate;
        private System.Windows.Forms.Label lblPacketRate_Result;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCreateGraph;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.GroupBox groupBoxOV2;
        private PacketListView listViewOV;
        private System.Windows.Forms.RichTextBox richTextBoxOV;
        private PictureBox pictureBoxTutorialImage;

        public object chart { get; internal set; }
    }
}
