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
    public partial class HeurDissectorsDialog : Form
    {
        public List<string> Available { get; set; }
        public List<string> Enabled { get; set; }

        public HeurDissectorsDialog(List<string> availableList,List<string> enabledList,bool isDefaultLists)
        {
            InitializeComponent();

            Available = availableList;
            Enabled = enabledList;

            this.availableList.Items.AddRange(availableList.ToArray());
            this.enabledList.Items.AddRange(enabledList.ToArray());

            if (isDefaultLists)
            {
                // Not showing the reload button if this is the default lists
                reloadButton.Hide();
            }
        }

        private void MoveSingleDissector(object sender, EventArgs e)
        {
            ListBox fromBox;
            ListBox toBox;
            if (sender == enableDissectorButton)
            {
                fromBox = availableList;
                toBox = enabledList;
            }
            else
            {
                fromBox = enabledList;
                toBox = availableList;
            }

            if (fromBox.SelectedItem != null)
            {
                object item = fromBox.SelectedItem;
                fromBox.Items.Remove(item);
                toBox.Items.Add(item);
            }
        }

        private void MoveAllDissectors(object sender, EventArgs e)
        {
            ListBox fromBox;
            ListBox toBox;
            if (sender == enableAllDissectorsButton)
            {
                fromBox = availableList;
                toBox = enabledList;
            }
            else
            {
                fromBox = enabledList;
                toBox = availableList;
            }

            toBox.Items.AddRange(fromBox.Items);
            fromBox.Items.Clear();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Available = availableList.Items.Cast<string>().ToList();
            this.Enabled = enabledList.Items.Cast<string>().ToList();

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you'd like to reload TShark's default enabled/disabled lists?\r\n" +
                            "All changes will be reset.","PacketStudio",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                // This is our signal to the dialog's owner that we need a relaunch with default lists
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }
    }
}
