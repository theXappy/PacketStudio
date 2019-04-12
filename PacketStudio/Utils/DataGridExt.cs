using System.Windows.Forms;

namespace PacketStudio.Utils
{
    public static class DataGridExt
    {
        public static void StretchLastColumn(this DataGridView dataGridView)
        {
            if(dataGridView.Columns.Count == 0)
                return;
            var lastColIndex = dataGridView.Columns.Count - 1;
            var lastCol = dataGridView.Columns[lastColIndex];
            lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}