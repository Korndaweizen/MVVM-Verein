using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Mitgliederverwaltung.Database;
using PropertyChanged;

namespace Mitgliederverwaltung.ViewModel
{
    [ImplementPropertyChanged]
    public class AddUserViewModel : ViewModelBase
    {
        private readonly Member _memberNoInfo = new Member();

        private ICommand _addUserCommand;
        private ICommand _resetUserCommand;

        public AddUserViewModel()
        {
            Member = new Member();
        }

        public Member Member { get; set; }

        public MetroWindow MainWindow => Application.Current.MainWindow as MetroWindow;

        public ICommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new RelayCommand(async () =>
                    {
                        if (Member.KontoInh == "")
                            Member.KontoInh = Member.Ganzername;
                        if (SQLiteDB.AddMembertoDb(Member))
                            GridViewModel.UpDateMembers();
                        await
                            MainWindow.ShowMessageAsync("Success", $"'{Member.Vorname}' '{Member.Plz}' was added to DB.");
                        Member = new Member();
                    }, NeededInfo);
                }
                return _addUserCommand;
            }
        }

        public ICommand ResetUserCommand
        {
            get
            {
                return _resetUserCommand ??
                       (_resetUserCommand = new RelayCommand(() => { Member = new Member(); }, DataInserted));
            }
        }

        private bool DataInserted()
        {
            return Member != _memberNoInfo;
        }

        private bool NeededInfo()
        {
            if (!string.IsNullOrEmpty(Member.Vorname) && !string.IsNullOrEmpty(Member.Name) &&
                !string.IsNullOrEmpty(Member.Strasse) && !string.IsNullOrEmpty(Member.Ort) && Member.Eintritt != null &&
                Member.Geburtstag != null && Member.Plz != null)
                return true;
            return false;
        }
    }
}