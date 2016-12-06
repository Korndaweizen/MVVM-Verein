using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite;

namespace Mitgliederverwaltung.Database
{
    [Table("Members")]
    [ImplementPropertyChanged]
    public class Member
    {
        [PrimaryKey]
        public int Id { get; set; }


        [Ignore]
        public string Ganzername => Name + " " + Vorname;

        [NotNull, Required]
        public string Name { get; set; }

        [NotNull]
        public string Vorname { get; set; }

        [NotNull]
        public string Strasse { get; set; }

        [NotNull]
        public int? Plz { get; set; }

        [NotNull]
        public string Ort { get; set; }

        public string Telefon { get; set; } = "";
        public string TelefonMobil { get; set; } = "";
        public double? Beitrag { get; set; }

        [EmailAddress]
        public string Mail { get; set; }


        [Ignore]
        public int Mitgliedsjahre
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Eintritt.Value.Year;
                if (Geburtstag > today.AddYears(-age)) age--;
                return age;
            }
        }

        [NotNull]
        public DateTime? Eintritt { get; set; }

        public DateTime? Austritt { get; set; }

        [NotNull]
        public DateTime? Geburtstag { get; set; }

        [Ignore]
        public int Alter
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Geburtstag.Value.Year;
                if (Geburtstag > today.AddYears(-age)) age--;
                return age;
            }
            set { }
        }

        public string KontoInh { get; set; } = "";
        //public string Blz { get; set; } = "";
        //public string Konto { get; set; } = "";
        public string Iban { get; set; } = "";
        public string BIC { get; set; } = "";
        public string Bank { get; set; } = "";

        public bool Payed
        {
            get
            {
                var payedUntil = PayedUntil ?? DateTime.MinValue;
                if (payedUntil < DateTime.Today)
                    return false;
                return true;
            }
            set
            {
                if (value)
                {
                    var oldDateTime = PayedUntil ?? DateTime.MinValue;
                    var newPayDate = new DateTime(DateTime.Now.Year + 1, 1, 1);
                    if (newPayDate > oldDateTime)
                    {
                        PayedUntil = newPayDate;
                    }
                }
            }
        }

        public DateTime? PayedUntil { get; set; }
        public DateTime? BefreitBis { get; set; }
        public string Additional_Information { get; set; } = "";


        public string ToJSONString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}