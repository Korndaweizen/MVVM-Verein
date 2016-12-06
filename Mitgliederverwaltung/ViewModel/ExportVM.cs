using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
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
    public class ExportVm
    {
        private readonly IDemoService _demoservice;
        private readonly ISFDialogue _isfDialogue;
        private RelayCommand _exportBankingCommand;
        private RelayCommand _exportDBCommand;
        private RelayCommand _exportSelectedCommand;
        /*public ExportVm()
        {
            ExportList = new ObservableCollection<ExportItemList>();
            var banking = new ExportItemList { ExportCategory = "Banking" };
            banking.Add(new ExportItem { Name = "Mitgliedsname" });
            banking.Add(new ExportItem { Name = "Kontoinhaber" });
            banking.Add(new ExportItem { Name = "Iban" });
            banking.Add(new ExportItem { Name = "BIC" });
            banking.Add(new ExportItem { Name = "Bankname" });
            banking.Add(new ExportItem { Name = "Bezahlt Bis" });
            banking.Add(new ExportItem { Name = "Befreit Bis" });
            banking.Add(new ExportItem { Name = "Hat Gezahlt" });

            var common = new ExportItemList { ExportCategory = "Allgemein" };
            common.Add(new ExportItem { Name = "Nr." });
            common.Add(new ExportItem { Name = "Name" });
            common.Add(new ExportItem { Name = "Vorname" });
            common.Add(new ExportItem { Name = "Alter" });
            common.Add(new ExportItem { Name = "Mail" });
            common.Add(new ExportItem { Name = "Telefon" });
            common.Add(new ExportItem { Name = "TelefonMobil" });
            common.Add(new ExportItem { Name = "Strasse" });
            common.Add(new ExportItem { Name = "Plz" });
            common.Add(new ExportItem { Name = "Ort" });
            common.Add(new ExportItem { Name = "Geburtstag" });
            common.Add(new ExportItem { Name = "Mitgliedsjahre" });
            common.Add(new ExportItem { Name = "Eintritt" });
            common.Add(new ExportItem { Name = "Austritt" });
            common.Add(new ExportItem { Name = "Mitgliedsfunktion" });


            ExportList.Add(common);
            ExportList.Add(banking);
        }*/

        public ExportVm(ISFDialogue isfDialogue, IDemoService demoservice)
        {
            _demoservice = demoservice;
            _isfDialogue = isfDialogue;
            ExportList = new ObservableCollection<ExportItemList>();
            var banking = new ExportItemList {ExportCategory = "Banking"};
            banking.Add(new ExportItem {Name = "Mitgliedsname"});
            banking.Add(new ExportItem {Name = "Kontoinhaber"});
            banking.Add(new ExportItem {Name = "Iban"});
            banking.Add(new ExportItem {Name = "BIC"});
            banking.Add(new ExportItem {Name = "Bankname"});
            banking.Add(new ExportItem {Name = "Bezahlt Bis"});
            banking.Add(new ExportItem {Name = "Befreit Bis"});
            banking.Add(new ExportItem {Name = "Hat Gezahlt"});

            var common = new ExportItemList {ExportCategory = "Allgemein"};
            common.Add(new ExportItem {Name = "Nr."});
            common.Add(new ExportItem {Name = "Name"});
            common.Add(new ExportItem {Name = "Vorname"});
            common.Add(new ExportItem {Name = "Alter"});
            common.Add(new ExportItem {Name = "Mail"});
            common.Add(new ExportItem {Name = "Telefon"});
            common.Add(new ExportItem {Name = "TelefonMobil"});
            common.Add(new ExportItem {Name = "Strasse"});
            common.Add(new ExportItem {Name = "Plz"});
            common.Add(new ExportItem {Name = "Ort"});
            common.Add(new ExportItem {Name = "Geburtstag"});
            common.Add(new ExportItem {Name = "Mitgliedsjahre"});
            common.Add(new ExportItem {Name = "Eintritt"});
            common.Add(new ExportItem {Name = "Austritt"});
            common.Add(new ExportItem {Name = "Mitgliedsfunktion"});


            ExportList.Add(common);
            ExportList.Add(banking);
        }

        public ObservableCollection<ExportItemList> ExportList { get; set; }


        public MetroWindow MainWindow => Application.Current.MainWindow as MetroWindow;

        public ICommand ExportBankingCommand
        {
            get
            {
                if (_exportBankingCommand == null)
                {
                    _exportBankingCommand = new RelayCommand(async () =>
                    {
                        _demoservice.DefaultPath = _isfDialogue.SaveFileDialog(_demoservice.DefaultPath,
                            "excel files (*.csv)|*.csv");
                        var b = new StringBuilder();
                        SQLiteDB.InsertUpdateCollection(GridViewModel.Members);
                        if (!string.IsNullOrEmpty(_demoservice.DefaultPath))
                        {
                            b.Append(
                                "Mitgliedname;Kontoinhaber (Falls abweichend);IBAN;BIC;Bankname;Bezahlt bis; Befreit bis; Gezahlt?");

                            foreach (var member in GridViewModel.Members)
                            {
                                b.AppendLine();
                                b.Append(member.Ganzername + ";");
                                b.Append(member.KontoInh + ";");
                                b.Append(member.Iban + ";");
                                b.Append(member.BIC + ";");
                                b.Append(member.Bank + ";");
                                b.Append(member.PayedUntil?.ToString("dd.MM.yyyy",
                                    CultureInfo.InvariantCulture) + ";");
                                b.Append(member.BefreitBis?.ToString("dd.MM.yyyy",
                                    CultureInfo.InvariantCulture) + ";");
                                b.Append(member.Payed);
                            }
                            File.WriteAllText(_demoservice.DefaultPath, b.ToString(), Encoding.UTF8);

                            await MainWindow.ShowMessageAsync("Success", "Export complete");
                        }
                    });
                }
                return _exportBankingCommand;
            }
        }

        public ICommand ExportDBCommand
        {
            get
            {
                if (_exportDBCommand == null)
                {
                    _exportDBCommand = new RelayCommand(async () =>
                    {
                        _demoservice.DefaultPath = _isfDialogue.SaveFileDialog(_demoservice.DefaultPath,
                            "Json files (*.json)|*.json");
                        if (!string.IsNullOrEmpty(_demoservice.DefaultPath))
                        {
                            var members = SQLiteDB.GetAllMembers();
                            var jsonstring = JsonConvert.SerializeObject(members);
                            File.WriteAllText(_demoservice.DefaultPath, jsonstring, Encoding.UTF8);


                            await MainWindow.ShowMessageAsync("Success", "Export complete");
                        }
                    });
                }
                return _exportDBCommand;
            }
        }

        public ICommand ExportSelectedCommand
        {
            get
            {
                if (_exportSelectedCommand == null)
                {
                    _exportSelectedCommand = new RelayCommand(async () =>
                    {
                        var b = new StringBuilder();
                        var eList = new ExportItemList();
                        foreach (var explist in ExportList)
                        {
                            foreach (var eitem in explist)
                            {
                                if (eitem.Checked)
                                {
                                    eList.Add(eitem);
                                }
                            }
                        }
                        SQLiteDB.InsertUpdateCollection(GridViewModel.Members);
                        _demoservice.DefaultPath = _isfDialogue.SaveFileDialog(_demoservice.DefaultPath,
                            "excel files (*.csv)|*.csv");
                        if (!string.IsNullOrEmpty(_demoservice.DefaultPath))
                        {
                            foreach (var item in eList)
                            {
                                b.Append(item.Name + ";");
                            }
                            b.Remove(b.Length - 1, 1);
                            foreach (var member in GridViewModel.Members)
                            {
                                b.AppendLine();
                                foreach (var item in eList)
                                {
                                    if (item.Name.Equals("Mitgliedsname"))
                                    {
                                        b.Append(member.Ganzername + ";");
                                    }
                                    else if (item.Name.Equals("Kontoinhaber"))
                                    {
                                        b.Append(member.KontoInh + ";");
                                    }
                                    else if (item.Name.Equals("Iban"))
                                    {
                                        b.Append(member.Iban + ";");
                                    }
                                    else if (item.Name.Equals("BIC"))
                                    {
                                        b.Append(member.BIC + ";");
                                    }
                                    else if (item.Name.Equals("Bankname"))
                                    {
                                        b.Append(member.Bank + ";");
                                    }
                                    else if (item.Name.Equals("Bezahlt Bis"))
                                    {
                                        b.Append(member.PayedUntil?.ToString("dd.MM.yyyy",
                                            CultureInfo.InvariantCulture) + ";");
                                    }
                                    else if (item.Name.Equals("Befreit Bis"))
                                    {
                                        b.Append(member.BefreitBis?.ToString("dd.MM.yyyy",
                                            CultureInfo.InvariantCulture) + ";");
                                    }
                                    else if (item.Name.Equals("Hat Gezahlt"))
                                    {
                                        b.Append(member.Payed + ";");
                                    }
                                    else if (item.Name.Equals("Nr."))
                                    {
                                        b.Append(member.Id + ";");
                                    }
                                    else if (item.Name.Equals("Name"))
                                    {
                                        b.Append(member.Name + ";");
                                    }
                                    else if (item.Name.Equals("Vorname"))
                                    {
                                        b.Append(member.Vorname + ";");
                                    }
                                    else if (item.Name.Equals("Alter"))
                                    {
                                        b.Append(member.Alter + ";");
                                    }
                                    else if (item.Name.Equals("Mail"))
                                    {
                                        b.Append(member.Mail + ";");
                                    }
                                    else if (item.Name.Equals("Telefon"))
                                    {
                                        b.Append(member.Telefon + ";");
                                    }
                                    else if (item.Name.Equals("TelefonMobil"))
                                    {
                                        b.Append(member.TelefonMobil + ";");
                                    }
                                    else if (item.Name.Equals("Strasse"))
                                    {
                                        b.Append(member.Strasse + ";");
                                    }
                                    else if (item.Name.Equals("Plz"))
                                    {
                                        b.Append(member.Plz + ";");
                                    }
                                    else if (item.Name.Equals("Ort"))
                                    {
                                        b.Append(member.Ort + ";");
                                    }
                                    else if (item.Name.Equals("Geburtstag"))
                                    {
                                        b.Append(member.Geburtstag?.ToString("dd.MM.yyyy",
                                            CultureInfo.InvariantCulture) + ";");
                                    }
                                    else if (item.Name.Equals("Mitgliedsjahre"))
                                    {
                                        b.Append(member.Mitgliedsjahre + ";");
                                    }
                                    else if (item.Name.Equals("Eintritt"))
                                    {
                                        b.Append(member.Eintritt?.ToString("dd.MM.yyyy",
                                            CultureInfo.InvariantCulture) + ";");
                                    }
                                    else if (item.Name.Equals("Austritt"))
                                    {
                                        b.Append(member.Austritt?.ToString("dd.MM.yyyy",
                                            CultureInfo.InvariantCulture) + ";");
                                    }
                                    else if (item.Name.Equals("Mitgliedsfunktion"))
                                    {
                                        b.Append(member.Additional_Information + ";");
                                    }
                                }
                                b.Remove(b.Length - 1, 1);
                            }
                            File.WriteAllText(_demoservice.DefaultPath, b.ToString(), Encoding.UTF8);

                            await MainWindow.ShowMessageAsync("Success", "Export complete");
                        }
                    }, IsAnythingSelected);
                }
                return _exportSelectedCommand;
            }
        }

        private bool IsAnythingSelected()
        {
            var retVal = false;
            foreach (var itemList in ExportList)
            {
                foreach (var exportItem in itemList)
                {
                    retVal = exportItem.Checked || retVal;
                }
            }
            return retVal;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }


    [ImplementPropertyChanged]
    public class ExportItemList : List<ExportItem>
    {
        private bool _exportCategoryChecked;

        public string ExportCategory { get; set; }

        public bool ExportCategoryChecked
        {
            get { return _exportCategoryChecked; }
            set
            {
                _exportCategoryChecked = value;
                foreach (var item in this)
                {
                    item.Checked = value;
                }
            }
        }
    }


    [ImplementPropertyChanged]
    public class ExportItem
    {
        public bool Checked { get; set; }
        public string Name { get; set; }
    }
}