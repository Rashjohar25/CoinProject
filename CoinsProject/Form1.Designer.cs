namespace CoinsProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.text_publickey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_privatekey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.text_withdrawlto = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.exchangeto = new System.Windows.Forms.ComboBox();
            this.exchangefrom = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip_start = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip_stop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.withdrawfrom = new System.Windows.Forms.ComboBox();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // text_publickey
            // 
            this.text_publickey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.text_publickey.Location = new System.Drawing.Point(102, 12);
            this.text_publickey.Name = "text_publickey";
            this.text_publickey.Size = new System.Drawing.Size(274, 20);
            this.text_publickey.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Public Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Private Key";
            // 
            // text_privatekey
            // 
            this.text_privatekey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.text_privatekey.Location = new System.Drawing.Point(102, 39);
            this.text_privatekey.Name = "text_privatekey";
            this.text_privatekey.Size = new System.Drawing.Size(274, 20);
            this.text_privatekey.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Exchange";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Withdrawl";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "To";
            // 
            // text_withdrawlto
            // 
            this.text_withdrawlto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.text_withdrawlto.Location = new System.Drawing.Point(278, 92);
            this.text_withdrawlto.Name = "text_withdrawlto";
            this.text_withdrawlto.Size = new System.Drawing.Size(100, 20);
            this.text_withdrawlto.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // exchangeto
            // 
            this.exchangeto.Cursor = System.Windows.Forms.Cursors.Default;
            this.exchangeto.FormattingEnabled = true;
            this.exchangeto.Location = new System.Drawing.Point(278, 65);
            this.exchangeto.Name = "exchangeto";
            this.exchangeto.Size = new System.Drawing.Size(100, 21);
            this.exchangeto.Sorted = true;
            this.exchangeto.TabIndex = 3;
            this.exchangeto.Text = "Select";
            // 
            // exchangefrom
            // 
            this.exchangefrom.FormattingEnabled = true;
            this.exchangefrom.Location = new System.Drawing.Point(102, 65);
            this.exchangefrom.Name = "exchangefrom";
            this.exchangefrom.Size = new System.Drawing.Size(100, 21);
            this.exchangefrom.Sorted = true;
            this.exchangefrom.TabIndex = 2;
            this.exchangefrom.Text = "Select";
            // 
            // timer1
            // 
            this.timer1.Interval = 3600000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStrip_start,
            this.toolStrip_stop,
            this.toolStripSeparator1,
            this.toolStrip_exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(79, 98);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(78, 22);
            this.toolStripMenuItem1.Text = "Open";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStrip_start
            // 
            this.toolStrip_start.Name = "toolStrip_start";
            this.toolStrip_start.Size = new System.Drawing.Size(78, 22);
            this.toolStrip_start.Text = "Start";
            this.toolStrip_start.Click += new System.EventHandler(this.toolStrip_start_Click);
            // 
            // toolStrip_stop
            // 
            this.toolStrip_stop.Name = "toolStrip_stop";
            this.toolStrip_stop.Size = new System.Drawing.Size(78, 22);
            this.toolStrip_stop.Text = "Stop";
            this.toolStrip_stop.Click += new System.EventHandler(this.toolStrip_stop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(75, 6);
            // 
            // toolStrip_exit
            // 
            this.toolStrip_exit.Name = "toolStrip_exit";
            this.toolStrip_exit.Size = new System.Drawing.Size(78, 22);
            this.toolStrip_exit.Text = "Exit";
            this.toolStrip_exit.Click += new System.EventHandler(this.toolStrip_exit_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Coin Exchange";
            this.notifyIcon1.Visible = true;
            // 
            // withdrawfrom
            // 
            this.withdrawfrom.FormattingEnabled = true;
            this.withdrawfrom.Location = new System.Drawing.Point(102, 92);
            this.withdrawfrom.Name = "withdrawfrom";
            this.withdrawfrom.Size = new System.Drawing.Size(100, 21);
            this.withdrawfrom.Sorted = true;
            this.withdrawfrom.TabIndex = 4;
            this.withdrawfrom.Text = "Select";
            // 
            // eventLog1
            // 
            this.eventLog1.EnableRaisingEvents = true;
            this.eventLog1.Log = "Application";
            this.eventLog1.Source = "CoinProject";
            this.eventLog1.SynchronizingObject = this;
            this.eventLog1.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.eventLog1_EntryWritten);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(429, 169);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 188);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Event Logs";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(221, 119);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Hide";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 336);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.withdrawfrom);
            this.Controls.Add(this.exchangefrom);
            this.Controls.Add(this.exchangeto);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.text_withdrawlto);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.text_privatekey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_publickey);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Cryptsy Coin Exchange";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_publickey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_privatekey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox text_withdrawlto;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox exchangeto;
        private System.Windows.Forms.ComboBox exchangefrom;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStrip_start;
        private System.Windows.Forms.ToolStripMenuItem toolStrip_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStrip_exit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ComboBox withdrawfrom;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
    }
}

