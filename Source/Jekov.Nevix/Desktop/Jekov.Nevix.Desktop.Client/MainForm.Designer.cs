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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.playerLabel = new System.Windows.Forms.Label();
            this.changePlayerBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.connect = new System.Windows.Forms.Button();
            this.email = new System.Windows.Forms.Label();
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.syncBtn = new System.Windows.Forms.Button();
            this.mediaDirectories = new System.Windows.Forms.ListBox();
            this.playerLocation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Jekov.Nevix.Desktop.Client.Properties.Resources.ic_launcher;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 51);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.label4.Location = new System.Drawing.Point(72, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 37);
            this.label4.TabIndex = 12;
            this.label4.Text = "NeviX Client";
            // 
            // playerLabel
            // 
            this.playerLabel.AutoSize = true;
            this.playerLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.playerLabel.Location = new System.Drawing.Point(26, 114);
            this.playerLabel.Name = "playerLabel";
            this.playerLabel.Size = new System.Drawing.Size(104, 13);
            this.playerLabel.TabIndex = 14;
            this.playerLabel.Text = "BSPlayer location : ";
            // 
            // changePlayerBtn
            // 
            this.changePlayerBtn.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.changePlayerBtn.Location = new System.Drawing.Point(29, 131);
            this.changePlayerBtn.Name = "changePlayerBtn";
            this.changePlayerBtn.Size = new System.Drawing.Size(75, 23);
            this.changePlayerBtn.TabIndex = 15;
            this.changePlayerBtn.Text = "Change";
            this.changePlayerBtn.UseVisualStyleBackColor = true;
            this.changePlayerBtn.Click += new System.EventHandler(this.ChangePlayerBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Media folders";
            // 
            // connect
            // 
            this.connect.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.connect.Location = new System.Drawing.Point(496, 216);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(104, 68);
            this.connect.TabIndex = 18;
            this.connect.Text = "Connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.email.Location = new System.Drawing.Point(89, 57);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(37, 15);
            this.email.TabIndex = 20;
            this.email.Text = "email";
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(444, 216);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(27, 23);
            this.add.TabIndex = 21;
            this.add.Text = "+";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.Add_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(444, 245);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(27, 23);
            this.remove.TabIndex = 22;
            this.remove.Text = "-";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(512, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Antony Jekov 2014";
            // 
            // syncBtn
            // 
            this.syncBtn.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.syncBtn.Location = new System.Drawing.Point(29, 330);
            this.syncBtn.Name = "syncBtn";
            this.syncBtn.Size = new System.Drawing.Size(75, 23);
            this.syncBtn.TabIndex = 24;
            this.syncBtn.Text = "Sync Media";
            this.syncBtn.UseVisualStyleBackColor = true;
            this.syncBtn.Click += new System.EventHandler(this.syncBtn_Click);
            // 
            // mediaDirectories
            // 
            this.mediaDirectories.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.mediaDirectories.FormattingEnabled = true;
            this.mediaDirectories.ItemHeight = 17;
            this.mediaDirectories.Location = new System.Drawing.Point(29, 216);
            this.mediaDirectories.Name = "mediaDirectories";
            this.mediaDirectories.Size = new System.Drawing.Size(409, 106);
            this.mediaDirectories.TabIndex = 16;
            // 
            // playerLocation
            // 
            this.playerLocation.AutoSize = true;
            this.playerLocation.Location = new System.Drawing.Point(137, 114);
            this.playerLocation.Name = "playerLocation";
            this.playerLocation.Size = new System.Drawing.Size(44, 13);
            this.playerLocation.TabIndex = 25;
            this.playerLocation.Text = "location";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 389);
            this.Controls.Add(this.playerLocation);
            this.Controls.Add(this.syncBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.email);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mediaDirectories);
            this.Controls.Add(this.changePlayerBtn);
            this.Controls.Add(this.playerLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Nevix";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label playerLabel;
        private System.Windows.Forms.Button changePlayerBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button syncBtn;
        private System.Windows.Forms.ListBox mediaDirectories;
        private System.Windows.Forms.Label playerLocation;
    }
}