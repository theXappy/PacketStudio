namespace PacketStudio
{
    partial class HeurDissectorsDialog
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
            this.availableList = new System.Windows.Forms.ListBox();
            this.enabledList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.enableDissectorButton = new System.Windows.Forms.Button();
            this.enableAllDissectorsButton = new System.Windows.Forms.Button();
            this.disableAllDissectorsButton = new System.Windows.Forms.Button();
            this.disableDissectorButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reloadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // availableList
            // 
            this.availableList.FormattingEnabled = true;
            this.availableList.Location = new System.Drawing.Point(12, 38);
            this.availableList.Name = "availableList";
            this.availableList.Size = new System.Drawing.Size(203, 303);
            this.availableList.Sorted = true;
            this.availableList.TabIndex = 0;
            // 
            // enabledList
            // 
            this.enabledList.FormattingEnabled = true;
            this.enabledList.Location = new System.Drawing.Point(355, 38);
            this.enabledList.Name = "enabledList";
            this.enabledList.Size = new System.Drawing.Size(203, 303);
            this.enabledList.Sorted = true;
            this.enabledList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available Heuristic Dissectors";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(352, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enabled Heuristic Dissectors";
            // 
            // enableDissectorButton
            // 
            this.enableDissectorButton.Location = new System.Drawing.Point(248, 75);
            this.enableDissectorButton.Name = "enableDissectorButton";
            this.enableDissectorButton.Size = new System.Drawing.Size(75, 34);
            this.enableDissectorButton.TabIndex = 4;
            this.enableDissectorButton.Text = "→";
            this.enableDissectorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.enableDissectorButton.UseVisualStyleBackColor = true;
            this.enableDissectorButton.Click += new System.EventHandler(this.MoveSingleDissector);
            // 
            // enableAllDissectorsButton
            // 
            this.enableAllDissectorsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableAllDissectorsButton.Location = new System.Drawing.Point(249, 115);
            this.enableAllDissectorsButton.Name = "enableAllDissectorsButton";
            this.enableAllDissectorsButton.Size = new System.Drawing.Size(75, 34);
            this.enableAllDissectorsButton.TabIndex = 5;
            this.enableAllDissectorsButton.Text = "⮆";
            this.enableAllDissectorsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.enableAllDissectorsButton.UseVisualStyleBackColor = true;
            this.enableAllDissectorsButton.Click += new System.EventHandler(this.MoveAllDissectors);
            // 
            // disableAllDissectorsButton
            // 
            this.disableAllDissectorsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disableAllDissectorsButton.Location = new System.Drawing.Point(248, 214);
            this.disableAllDissectorsButton.Name = "disableAllDissectorsButton";
            this.disableAllDissectorsButton.Size = new System.Drawing.Size(75, 34);
            this.disableAllDissectorsButton.TabIndex = 7;
            this.disableAllDissectorsButton.Text = "⮄";
            this.disableAllDissectorsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.disableAllDissectorsButton.UseVisualStyleBackColor = true;
            this.disableAllDissectorsButton.Click += new System.EventHandler(this.MoveAllDissectors);
            // 
            // disableDissectorButton
            // 
            this.disableDissectorButton.Location = new System.Drawing.Point(249, 254);
            this.disableDissectorButton.Name = "disableDissectorButton";
            this.disableDissectorButton.Size = new System.Drawing.Size(75, 34);
            this.disableDissectorButton.TabIndex = 6;
            this.disableDissectorButton.Text = "←";
            this.disableDissectorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.disableDissectorButton.UseVisualStyleBackColor = true;
            this.disableDissectorButton.Click += new System.EventHandler(this.MoveSingleDissector);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(483, 352);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(402, 352);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // reloadButton
            // 
            this.reloadButton.Location = new System.Drawing.Point(12, 352);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 10;
            this.reloadButton.Text = "⭮ Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click);
            // 
            // HeurDissectorsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 387);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.disableAllDissectorsButton);
            this.Controls.Add(this.disableDissectorButton);
            this.Controls.Add(this.enableAllDissectorsButton);
            this.Controls.Add(this.enableDissectorButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.enabledList);
            this.Controls.Add(this.availableList);
            this.Name = "HeurDissectorsDialog";
            this.Text = "HeurDissectorsDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox availableList;
        private System.Windows.Forms.ListBox enabledList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button enableDissectorButton;
        private System.Windows.Forms.Button enableAllDissectorsButton;
        private System.Windows.Forms.Button disableAllDissectorsButton;
        private System.Windows.Forms.Button disableDissectorButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button reloadButton;
    }
}