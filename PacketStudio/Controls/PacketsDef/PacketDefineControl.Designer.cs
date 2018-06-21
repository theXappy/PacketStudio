namespace PacketStudio.Controls.PacketsDef
{
    partial class PacketDefineControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.hexBox = new System.Windows.Forms.TextBox();
			this.packetTypeListBox = new System.Windows.Forms.ListBox();
			this.packetDefPanel = new System.Windows.Forms.Panel();
			this.rawPacketDefControl1 = new PacketStudio.Controls.PacketsDef.RawPacketDefControl();
			this.packetDefPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// hexBox
			// 
			this.hexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hexBox.Font = new System.Drawing.Font("Consolas", 10F);
			this.hexBox.HideSelection = false;
			this.hexBox.Location = new System.Drawing.Point(3, 3);
			this.hexBox.Multiline = true;
			this.hexBox.Name = "hexBox";
			this.hexBox.Size = new System.Drawing.Size(344, 164);
			this.hexBox.TabIndex = 6;
			this.hexBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.hexBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			// 
			// packetTypeListBox
			// 
			this.packetTypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.packetTypeListBox.FormattingEnabled = true;
			this.packetTypeListBox.Items.AddRange(new object[] {
            "Raw Ethernet",
            "UDP Payload",
            "SCTP Payload",
            "IP Payload"});
			this.packetTypeListBox.Location = new System.Drawing.Point(3, 173);
			this.packetTypeListBox.Name = "packetTypeListBox";
			this.packetTypeListBox.Size = new System.Drawing.Size(139, 56);
			this.packetTypeListBox.TabIndex = 14;
			this.packetTypeListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// packetDefPanel
			// 
			this.packetDefPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.packetDefPanel.Controls.Add(this.rawPacketDefControl1);
			this.packetDefPanel.Location = new System.Drawing.Point(148, 173);
			this.packetDefPanel.Name = "packetDefPanel";
			this.packetDefPanel.Size = new System.Drawing.Size(199, 64);
			this.packetDefPanel.TabIndex = 15;
			// 
			// rawPacketDefControl1
			// 
			this.rawPacketDefControl1.Location = new System.Drawing.Point(3, 3);
			this.rawPacketDefControl1.Name = "rawPacketDefControl1";
			this.rawPacketDefControl1.Size = new System.Drawing.Size(193, 61);
			this.rawPacketDefControl1.TabIndex = 0;
			// 
			// PacketDefineControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.packetDefPanel);
			this.Controls.Add(this.packetTypeListBox);
			this.Controls.Add(this.hexBox);
			this.Name = "PacketDefineControl";
			this.Size = new System.Drawing.Size(350, 240);
			this.packetDefPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox hexBox;
		private System.Windows.Forms.ListBox packetTypeListBox;
		private System.Windows.Forms.Panel packetDefPanel;
		private RawPacketDefControl rawPacketDefControl1;
	}
}
