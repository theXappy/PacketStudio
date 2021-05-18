using PacketStudio.NewGUI.Properties;

namespace PacketStudio.NewGUI
{
    public class SessionSaveState
    {
        public bool HasUnsavedChanges { get; set; }
        [CanBeNull] public string AssociatedFilePath { get; set; }
        public bool HasAssociatedFile => !string.IsNullOrEmpty(AssociatedFilePath);

        public SessionSaveState()
        {
            HasUnsavedChanges = false;
            AssociatedFilePath = null;
        }

        public SessionSaveState(bool hasUnsavedChanges, [CanBeNull] string associatedFilePath)
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