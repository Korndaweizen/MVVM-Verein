using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Anotar.Log4Net;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Mitgliederverwaltung.Database;
using Mitgliederverwaltung.Services;
using Newtonsoft.Json;
using PropertyChanged;

namespace Mitgliederverwaltung.ViewModel
{
    [ImplementPropertyChanged]
    public class GridViewModel : ViewModelBase
    {
        private static ObservableCollection<Member> _members=new ObservableCollection<Member>();

        private readonly IDemoService _demoService;

        private readonly IOFDialogue _ioService;


        private RelayCommand _pickFilePathCommand;
        private ICommand _saveChangesCommand;


        public GridViewModel(IDemoService demoService, IOFDialogue ioService)
        {
            _demoService = demoService;
            _ioService = ioService;


            //try
            //{
            //    SQLiteDB.Init();
            //}
            //catch (Exception e)
            //{
            //    LogTo.ErrorException("Error initializing DB",e);
            //    throw;
            //}
            if (IsInDesignMode)
            {
            }
            else
            {
                if (Members.Count==0)
                {
                    Members = SQLiteDB.GetAllMembers();
                    LogTo.Info("Getting Members INIT");
                }
                
                LogTo.Info("GridViewModel initialized");
            }
        }

        public static ObservableCollection<Member> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs("Members"));
            }
        }

        public MetroWindow MainWindow => Application.Current.MainWindow as MetroWindow;

        public string SelectedPath { get; set; }

        public ICommand SaveChangesCommand
        {
            get
            {
                if (_saveChangesCommand == null)
                {
                    LogTo.Info("Init SaveChangesCommand");
                    _saveChangesCommand = new RelayCommand(() =>
                    {
                        LogTo.Info("Exec SaveChangesCommand");
                        SQLiteDB.InsertUpdateCollection(Members);
                    });
                }
                return _saveChangesCommand;
            }
        }



        public ICommand PickFilePathCommand
        {
            get
            {

                if (_pickFilePathCommand == null)
                {
                    LogTo.Info("Init PickFilePathCommand");
                    _pickFilePathCommand = new RelayCommand(async () =>
                    {
                        LogTo.Info("Exec PickFilePathCommand");
                        _demoService.DefaultPath = _ioService.OpenFileDialog(_demoService.DefaultPath);
                        if (string.IsNullOrEmpty(_demoService.DefaultPath))
                        {
                            _demoService.DefaultPath = string.Empty;
                            return;
                        }

                        List<Member> membersJson;
                        try
                        {
                            var jsonstring = File.ReadAllText(_demoService.DefaultPath, Encoding.UTF8);
                            membersJson = JsonConvert.DeserializeObject<List<Member>>(jsonstring);
                        }
                        catch (Exception)
                        {
                            LogTo.Error("File not supported");
                            await MainWindow.ShowMessageAsync("Failure", "File not supported");
                            return;
                        }

                        foreach (var m in membersJson)
                        {
                            SQLiteDB.AddMembertoDb(m);
                        }
                        Members = SQLiteDB.GetAllMembers();
                    });
                }
                return _pickFilePathCommand;
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public static void UpDateMembers()
        {
            LogTo.Info("Reload Members");
            Members = SQLiteDB.GetAllMembers();
        }
    }
}