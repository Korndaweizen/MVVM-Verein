using System;
using System.Windows.Forms;

namespace Mitgliederverwaltung.Services
{
    internal class SFDialogue : ISFDialogue
    {
        public string SaveFileDialog(string defaultPath, string filter)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter =
                    string.IsNullOrEmpty(filter)
                        ? "json files (*.json)|*.json|excel files (*.csv)|*.csv|All files (*.*)|*.*"
                        : filter,
                FilterIndex = 2,
                RestoreDirectory = true
            };


            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return "";
            Console.WriteLine(saveFileDialog1.FileName);
            return saveFileDialog1.FileName;
        }
    }
}