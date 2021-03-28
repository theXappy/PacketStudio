using System;
using System.Collections.Generic;
using System.Linq;
using PacketStudio.Core;
using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI
{
    public class WiresharkFinderViewModel : NotificationObject
    {
        private Lazy<List<WiresharkDirectory>> _sharksFinderDirs = new Lazy<List<WiresharkDirectory>>(SharksFinder.GetDirectories);
        private List<WiresharkDirectory> CustomDirectories = new List<WiresharkDirectory>();

        public List<WiresharkDirectory> WiresharkDirs => _sharksFinderDirs.Value.Concat(CustomDirectories).ToList();

        private WiresharkDirectory _selectedItem;
        public WiresharkDirectory SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                base.RaisePropertyChanged("SelectedItem");
            }
        }


        public void AddCustomDirectory(WiresharkDirectory wd)
        {
            if (wd == null) return;
            CustomDirectories.Add(wd);
            base.RaisePropertyChanged("WiresharkDirs");
        }
    }
}