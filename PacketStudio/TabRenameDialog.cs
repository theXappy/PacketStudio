using System;
using System.Windows.Forms;

namespace PacketStudio
{
	public partial class TabRenameDialog : Form
	{
		public string NewName { get; private set; }

		public TabRenameDialog(string oldName)
		{
			InitializeComponent();

			tabNameTextBox.Text = oldName;
			this.Text += '\"' + oldName + '\"';
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void TabRenameDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK)
			{
				this.DialogResult = DialogResult.Cancel;
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			NewName = tabNameTextBox.Text.Trim();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void tabNameTextBox_TextChanged(object sender, EventArgs e)
		{
			okButton.Enabled = !string.IsNullOrWhiteSpace(tabNameTextBox.Text);
		}
		
		private void tabNameTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Return)
			{
				if (okButton.Enabled)
				{
					okButton.PerformClick();
				}
			}
		}
	}
}
