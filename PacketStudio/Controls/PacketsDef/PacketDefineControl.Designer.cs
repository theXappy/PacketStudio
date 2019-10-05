using System;

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
            this.packetTypeListBox = new System.Windows.Forms.ListBox();
            this.packetDefPanel = new System.Windows.Forms.Panel();
            this.rawPacketDefControl1 = new PacketStudio.Controls.PacketsDef.RawPacketDefControl();
            this.scintillaHexBox = new ScintillaNET.Scintilla();
            this.packetDefPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // packetTypeListBox
            // 
            this.packetTypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packetTypeListBox.FormattingEnabled = true;
            this.packetTypeListBox.Location = new System.Drawing.Point(3, 171);
            this.packetTypeListBox.Name = "packetTypeListBox";
            this.packetTypeListBox.Size = new System.Drawing.Size(139, 82);
            this.packetTypeListBox.TabIndex = 14;
            this.packetTypeListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // packetDefPanel
            // 
            this.packetDefPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.packetDefPanel.Controls.Add(this.rawPacketDefControl1);
            this.packetDefPanel.Location = new System.Drawing.Point(148, 171);
            this.packetDefPanel.Name = "packetDefPanel";
            this.packetDefPanel.Size = new System.Drawing.Size(247, 82);
            this.packetDefPanel.TabIndex = 15;
            // 
            // rawPacketDefControl1
            // 
            this.rawPacketDefControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rawPacketDefControl1.Location = new System.Drawing.Point(0, 0);
            this.rawPacketDefControl1.Name = "rawPacketDefControl1";
            this.rawPacketDefControl1.Size = new System.Drawing.Size(247, 82);
            this.rawPacketDefControl1.TabIndex = 0;
            // 
            // scintillaHexBox
            // 
            this.scintillaHexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scintillaHexBox.AutoCMaxHeight = 9;
            this.scintillaHexBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintillaHexBox.Location = new System.Drawing.Point(3, 3);
            this.scintillaHexBox.Name = "scintillaHexBox";
            this.scintillaHexBox.PasteConvertEndings = false;
            this.scintillaHexBox.Size = new System.Drawing.Size(392, 166);
            this.scintillaHexBox.TabIndex = 16;
            this.scintillaHexBox.WrapIndentMode = ScintillaNET.WrapIndentMode.Same;
            this.scintillaHexBox.WrapMode = ScintillaNET.WrapMode.Char;
            this.scintillaHexBox.TextChanged += new System.EventHandler(this.scintillaHexBox_TextChanged);
            // 
            // PacketDefineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scintillaHexBox);
            this.Controls.Add(this.packetDefPanel);
            this.Controls.Add(this.packetTypeListBox);
            this.Name = "PacketDefineControl";
            this.Size = new System.Drawing.Size(398, 256);
            this.Load += new System.EventHandler(this.PacketDefineControl_Load);
            this.packetDefPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.ListBox packetTypeListBox;
		private System.Windows.Forms.Panel packetDefPanel;
		private RawPacketDefControl rawPacketDefControl1;
        private ScintillaNET.Scintilla scintillaHexBox;
    }
}
