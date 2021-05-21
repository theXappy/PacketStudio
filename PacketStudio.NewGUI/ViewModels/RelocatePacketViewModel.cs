using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI.ViewModels
{
    public class RelocatePacketViewModel : NotificationObject
    {
        private string _newPosStr;
        private int _newPosition => int.Parse(_newPosStr);
        private bool _notParsable = false;
        private int _maxPacketPosition = -1;

        // Position where the packet will be inserted.
        // The preceding packet will be the one CURRENTLY at (NewPosition - 1)
        // The following packet will be the one CURRENTLY at (NewPosition), it'll move to (NewPosition + 1)
        public string NewPosition
        {
            get => _newPosStr;
            set
            {
                _newPosStr = value;
                _notParsable = (!int.TryParse(value, out _));
                base.RaisePropertyChanged(nameof(NewPosition));
                base.RaisePropertyChanged(nameof(IsValidPosition));
            }
        }

        public bool IsValidPosition => !_notParsable && _newPosition <= (MaxPacketPosition + 1) && _newPosition >= 0;

        public int MaxPacketPosition
        {
            get => _maxPacketPosition;
            set
            {
                _maxPacketPosition = value;
                base.RaisePropertyChanged(nameof(MaxPacketPosition));
                base.RaisePropertyChanged(nameof(IsValidPosition));
            }
        }
    }
}