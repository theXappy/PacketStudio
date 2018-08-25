using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketStudio
{
    public partial class AsciiDialog : Form
    {
        public string TextToInsert { get; private set; }

        public AsciiDialog()
        {
            TextToInsert = String.Empty;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            TextToInsert = _asciiBox.Text;
            // Check if the user clicked OK/Pressed return with an empty text box
            DialogResult = string.IsNullOrWhiteSpace(TextToInsert) ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _asciiBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
