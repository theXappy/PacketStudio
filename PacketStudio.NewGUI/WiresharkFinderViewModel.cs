using System;
using System.Collections.Generic;
using System.Linq;
using PacketStudio.Core;
using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI
{
    public class WiresharkFinderViewModel : NotificationObject
    {
        private readonly Lazy<List<WiresharkDirectory>> _sharksFinderDirs = new Lazy<List<WiresharkDirectory>>(SharksFinder.GetDirectories);
        private readonly List<WiresharkDirectory> _customDirectories = new List<WiresharkDirectory>();

        public List<WiresharkDirectory> WiresharkDirs => _sharksFinderDirs.Value.Concat(_customDirectories).ToList();

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
            _customDirectories.Add(wd);
            base.RaisePropertyChanged("WiresharkDirs");
        }
    }
}