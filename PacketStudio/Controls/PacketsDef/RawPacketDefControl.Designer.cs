namespace PacketStudio.Controls.PacketsDef
{
	partial class RawPacketDefControl
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
            this.linkLayerLabel = new System.Windows.Forms.Label();
            this.linkLayerTextBox = new System.Windows.Forms.TextBox();
            this.resProtoValueLabel = new System.Windows.Forms.Label();
            this.resProtoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkLayerLabel
            // 
            this.linkLayerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLayerLabel.AutoSize = true;
            this.linkLayerLabel.Location = new System.Drawing.Point(50, 6);
            this.linkLayerLabel.Name = "linkLayerLabel";
            this.linkLayerLabel.Size = new System.Drawing.Size(59, 13);
            this.linkLayerLabel.TabIndex = 3;
            this.linkLayerLabel.Text = "Link Layer:";
            // 
            // linkLayerTextBox
            // 
            this.linkLayerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLayerTextBox.Location = new System.Drawing.Point(113, 3);
            this.linkLayerTextBox.Name = "linkLayerTextBox";
            this.linkLayerTextBox.Size = new System.Drawing.Size(100, 20);
            this.linkLayerTextBox.TabIndex = 2;
            this.linkLayerTextBox.Text = "1";
            this.linkLayerTextBox.TextChanged += new System.EventHandler(this.linkLayerTextBox_TextChanged);
            // 
            // resProtoValueLabel
            // 
            this.resProtoValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resProtoValueLabel.AutoSize = true;
            this.resProtoValueLabel.Location = new System.Drawing.Point(113, 32);
            this.resProtoValueLabel.Name = "resProtoValueLabel";
            this.resProtoValueLabel.Size = new System.Drawing.Size(60, 13);
            this.resProtoValueLabel.TabIndex = 7;
            this.resProtoValueLabel.Text = "ProtoName";
            // 
            // resProtoLabel
            // 
            this.resProtoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resProtoLabel.AutoSize = true;
            this.resProtoLabel.Location = new System.Drawing.Point(10, 32);
            this.resProtoLabel.Name = "resProtoLabel";
            this.resProtoLabel.Size = new System.Drawing.Size(97, 13);
            this.resProtoLabel.TabIndex = 6;
            this.resProtoLabel.Text = "Resolved Protocol:";
            // 
            // RawPacketDefControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resProtoValueLabel);
            this.Controls.Add(this.resProtoLabel);
            this.Controls.Add(this.linkLayerLabel);
            this.Controls.Add(this.linkLayerTextBox);
            this.Name = "RawPacketDefControl";
            this.Size = new System.Drawing.Size(216, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Label linkLayerLabel;
        private System.Windows.Forms.TextBox linkLayerTextBox;
        private System.Windows.Forms.Label resProtoValueLabel;
        private System.Windows.Forms.Label resProtoLabel;
    }
}
