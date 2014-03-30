﻿using System.Windows.Forms;
namespace Jekov.Nevix.Desktop.Client
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mediaFoldersLabel = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.Label();
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.syncBtn = new System.Windows.Forms.Button();
            this.playerSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logoff = new System.Windows.Forms.LinkLabel();
            this.startWithWindows = new System.Windows.Forms.CheckBox();
            this.mediaDirectories = new System.Windows.Forms.ListBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Jekov.Nevix.Desktop.Client.Properties.Resources.ic_launcher;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 52);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(72, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 37);
            this.label4.TabIndex = 12;
            this.label4.Text = "NeviX Client";
            // 
            // mediaFoldersLabel
            // 
            this.mediaFoldersLabel.AutoSize = true;
            this.mediaFoldersLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mediaFoldersLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.mediaFoldersLabel.Location = new System.Drawing.Point(26, 115);
            this.mediaFoldersLabel.Name = "mediaFoldersLabel";
            this.mediaFoldersLabel.Size = new System.Drawing.Size(78, 13);
            this.mediaFoldersLabel.TabIndex = 17;
            this.mediaFoldersLabel.Text = "Media folders";
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.email.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.email.Location = new System.Drawing.Point(89, 49);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(37, 15);
            this.email.TabIndex = 20;
            this.email.Text = "email";
            // 
            // add
            // 
            this.add.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add.Location = new System.Drawing.Point(444, 133);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(27, 23);
            this.add.TabIndex = 21;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.Add_Click);
            // 
            // remove
            // 
            this.remove.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remove.Location = new System.Drawing.Point(444, 162);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(27, 23);
            this.remove.TabIndex = 22;
            this.remove.Text = "-";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // syncBtn
            // 
            this.syncBtn.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.syncBtn.Location = new System.Drawing.Point(29, 254);
            this.syncBtn.Name = "syncBtn";
            this.syncBtn.Size = new System.Drawing.Size(75, 23);
            this.syncBtn.TabIndex = 24;
            this.syncBtn.Text = "Sync Media";
            this.syncBtn.UseVisualStyleBackColor = true;
            this.syncBtn.Click += new System.EventHandler(this.syncBtn_Click);
            // 
            // playerSelect
            // 
            this.playerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerSelect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerSelect.FormattingEnabled = true;
            this.playerSelect.Location = new System.Drawing.Point(261, 77);
            this.playerSelect.Name = "playerSelect";
            this.playerSelect.Size = new System.Drawing.Size(210, 21);
            this.playerSelect.TabIndex = 25;
            this.playerSelect.SelectedIndexChanged += new System.EventHandler(this.playerSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(258, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Chose media player";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(357, 279);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Antony Jekov © 2014";
            // 
            // logoff
            // 
            this.logoff.AutoSize = true;
            this.logoff.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.logoff.Location = new System.Drawing.Point(426, 12);
            this.logoff.Name = "logoff";
            this.logoff.Size = new System.Drawing.Size(49, 13);
            this.logoff.TabIndex = 28;
            this.logoff.TabStop = true;
            this.logoff.Text = "Log Out";
            this.logoff.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.logoff_LinkClicked);
            // 
            // startWithWindows
            // 
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startWithWindows.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.startWithWindows.Location = new System.Drawing.Point(360, 254);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(128, 17);
            this.startWithWindows.TabIndex = 29;
            this.startWithWindows.Text = "Start with Windows";
            this.startWithWindows.UseVisualStyleBackColor = true;
            this.startWithWindows.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // mediaDirectories
            // 
            this.mediaDirectories.FormattingEnabled = true;
            this.mediaDirectories.Location = new System.Drawing.Point(29, 133);
            this.mediaDirectories.Name = "mediaDirectories";
            this.mediaDirectories.Size = new System.Drawing.Size(409, 95);
            this.mediaDirectories.TabIndex = 30;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Nevix Desktop Client";
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(492, 304);
            this.Controls.Add(this.mediaDirectories);
            this.Controls.Add(this.startWithWindows);
            this.Controls.Add(this.logoff);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playerSelect);
            this.Controls.Add(this.syncBtn);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.email);
            this.Controls.Add(this.mediaFoldersLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Nevix";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing_1);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label mediaFoldersLabel;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button syncBtn;
        private System.Windows.Forms.ComboBox playerSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel logoff;
        private System.Windows.Forms.CheckBox startWithWindows;
        private System.Windows.Forms.ListBox mediaDirectories;
        private NotifyIcon notifyIcon1;
    }
}