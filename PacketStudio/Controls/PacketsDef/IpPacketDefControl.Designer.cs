namespace PacketStudio.Controls.PacketsDef
{
	partial class IpPacketDefControl
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
			this.label2 = new System.Windows.Forms.Label();
			this.nextProtoTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// streamIdTextBox
			// 
			this.streamIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.streamIdTextBox.Location = new System.Drawing.Point(78, 3);
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
			this.label1.Location = new System.Drawing.Point(18, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Stream ID:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Protocol:";
			// 
			// nextProtoTextBox
			// 
			this.nextProtoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nextProtoTextBox.Location = new System.Drawing.Point(78, 29);
			this.nextProtoTextBox.Name = "nextProtoTextBox";
			this.nextProtoTextBox.Size = new System.Drawing.Size(100, 20);
			this.nextProtoTextBox.TabIndex = 2;
			this.nextProtoTextBox.Text = "1";
			this.nextProtoTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// IpPacketDefControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nextProtoTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.streamIdTextBox);
			this.Name = "IpPacketDefControl";
			this.Size = new System.Drawing.Size(181, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox streamIdTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox nextProtoTextBox;
	}
}
