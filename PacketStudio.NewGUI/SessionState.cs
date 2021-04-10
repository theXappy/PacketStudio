using PacketStudio.NewGUI.Properties;

namespace PacketStudio.NewGUI
{
    public class SessionState
    {
        public bool HasUnsavedChanges { get; set; }
        [CanBeNull] public string AssociatedFilePath { get; set; }
        public bool HasAssociatedFile => !string.IsNullOrEmpty(AssociatedFilePath);

        public SessionState()
        {
            HasUnsavedChanges = false;
            AssociatedFilePath = null;
        }

        public SessionState(bool hasUnsavedChanges, [CanBeNull] string associatedFilePath)
        {
            HasUnsavedChanges = hasUnsavedChanges;
            AssociatedFilePath = associatedFilePath;
        }

        public void Reset()
        {
            HasUnsavedChanges = false;
            AssociatedFilePath = null;
        }
    }
}