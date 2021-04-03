using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PacketStudio.Core;
using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI
{
    public class EncodeTextViewModel : NotificationObject
    {
        public string Text { get; set; }
        public int SelectedEncIndex { get; set; }

        public ObservableCollection<Encoding> AvailableEncodings { get; set; }

        public EncodeTextViewModel()
        {
            AvailableEncodings = new ObservableCollection<Encoding>(new[] { Encoding.ASCII, Encoding.Unicode, Encoding.BigEndianUnicode, Encoding.UTF8, Encoding.UTF7, Encoding.UTF32 });
        }
    }
}