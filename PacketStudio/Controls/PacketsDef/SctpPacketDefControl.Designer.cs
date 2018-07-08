namespace PacketStudio.Controls.PacketsDef
{
	partial class SctpPacketDefControl
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
            this.streamIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ppidLabel = new System.Windows.Forms.Label();
            this.ppidTextBox = new System.Windows.Forms.TextBox();
            this.resProtoLabel = new System.Windows.Forms.Label();
            this.resProtoValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // streamIdTextBox
            // 
            this.streamIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.streamIdTextBox.Location = new System.Drawing.Point(113, 3);
            this.streamIdTextBox.Name = "streamIdTextBox";
            this.streamIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.streamIdTextBox.TabIndex = 0;
            this.streamIdTextBox.Text = "1";
            this.streamIdTextBox.TextChanged += new System.EventHandler(this.streamIdTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Stream ID:";
            // 
            // ppidLabel
            // 
            this.ppidLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ppidLabel.AutoSize = true;
            this.ppidLabel.Location = new System.Drawing.Point(72, 32);
            this.ppidLabel.Name = "ppidLabel";
            this.ppidLabel.Size = new System.Drawing.Size(35, 13);
            this.ppidLabel.TabIndex = 3;
            this.ppidLabel.Text = "PPID:";
            // 
            // resProtoLabel
            // 
            this.resProtoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resProtoLabel.AutoSize = true;
            this.resProtoLabel.Location = new System.Drawing.Point(10, 56);
            this.resProtoLabel.Name = "resProtoLabel";
            this.resProtoLabel.Size = new System.Drawing.Size(97, 13);
            this.resProtoLabel.TabIndex = 4;
            this.resProtoLabel.Text = "Resolved Protocol:";
            // 
            // resProtoValueLabel
            // 
            this.resProtoValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resProtoValueLabel.AutoSize = true;
            this.resProtoValueLabel.Location = new System.Drawing.Point(113, 56);
            this.resProtoValueLabel.Name = "resProtoValueLabel";
            this.resProtoValueLabel.Size = new System.Drawing.Size(60, 13);
            this.resProtoValueLabel.TabIndex = 5;
            this.resProtoValueLabel.Text = "ProtoName";
		    // 
		    // ppidTextBox
		    // 
		    this.ppidTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		    this.ppidTextBox.Location = new System.Drawing.Point(113, 29);
		    this.ppidTextBox.Name = "ppidTextBox";
		    this.ppidTextBox.Size = new System.Drawing.Size(100, 20);
		    this.ppidTextBox.TabIndex = 2;
		    this.ppidTextBox.TextChanged += new System.EventHandler(this.ppidTextBox_TextChanged);
		    this.ppidTextBox.Text = "1";
            // 
            // SctpPacketDefControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resProtoValueLabel);
            this.Controls.Add(this.resProtoLabel);
            this.Controls.Add(this.ppidLabel);
            this.Controls.Add(this.ppidTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.streamIdTextBox);
            this.Name = "SctpPacketDefControl";
            this.Size = new System.Drawing.Size(216, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox streamIdTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label ppidLabel;
		private System.Windows.Forms.TextBox ppidTextBox;
        private System.Windows.Forms.Label resProtoLabel;
        private System.Windows.Forms.Label resProtoValueLabel;
    }
}
