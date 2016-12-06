using Microsoft.Win32;

namespace Mitgliederverwaltung.Services
{
    internal class OFDialogue : IOFDialogue
    {
        public string OpenFileDialog(string _defaultPath)
        {
            var dialog = new OpenFileDialog {InitialDirectory = _defaultPath};
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.ShowDialog();

            var selectedPath = dialog.FileName;
            return selectedPath;
        }
    }
}