using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI.ViewModels
{
    public class RelocatePacketViewModel : NotificationObject
    {
        private int _newPosition;
        private bool _notParsable = false;

        // Position where the packet will be inserted.
        // The preceding packet will be the one CURRENTLY at (NewPosition - 1)
        // The following packet will be the one CURRENTLY at (NewPosition), it'll move to (NewPosition + 1)
        public string NewPosition
        {
            get => _newPosition.ToString();
            set
            {
                int parsedVal;
                if (!int.TryParse(value, out parsedVal))
                {
                    _notParsable = true;
                    return;
                }

                _notParsable = false;
                _newPosition = parsedVal;
                base.RaisePropertyChanged(nameof(NewPosition));
                base.RaisePropertyChanged(nameof(PrevPos));
                base.RaisePropertyChanged(nameof(PrevPos));
                base.RaisePropertyChanged(nameof(IsValidPosition));
                base.RaisePropertyChanged(nameof(IsNotAtEnd));
                base.RaisePropertyChanged(nameof(IsNotAtStart));
            }
        }

        public string PrevPos
        {
            get => (_newPosition - 1).ToString();
            set
            {
                int parsedVal;
                if (int.TryParse(value, out parsedVal))
                {
                    NewPosition = (parsedVal + 1).ToString();
                }
                else
                {
                    NewPosition = "xxx"; // Trigger parse error
                }
            }
        }

        public bool IsValidPosition => !_notParsable && _newPosition <= (MaxPacketPosition + 1) || _newPosition >= 0;

        public bool IsNotAtStart => _newPosition!= 0;
        public bool IsNotAtEnd => _newPosition != (MaxPacketPosition + 1);

        public int MaxPacketPosition { get; set; } = -1;
    }
}