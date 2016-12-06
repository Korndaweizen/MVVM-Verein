using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Anotar.Log4Net;
using Microsoft.Practices.ObjectBuilder2;
using SQLite;

namespace Mitgliederverwaltung.Database
{
    internal class SQLiteDB
    {
        private static readonly string _dataBaseName = "MemberDB.sqlite";
        private static SQLiteConnection _dbConnection;

        public static void Init()
        {
            LogTo.Info("Initializing DB");
            if (_dbConnection == null)
            {
                _dbConnection = new SQLiteConnection(_dataBaseName);
                _dbConnection.CreateTable<Member>();
            }
        }

        private static List<int> GetListOfMemberNos()
        {
            var retList = _dbConnection.Table<Member>().ToList().Select(m => m.Id).ToList();

            return retList;
        }

        private static int GetFirstFreeMemberNo(List<int> memberNos)
        {
            if (memberNos.Count == 0)
            {
                return 1;
            }

            if (memberNos.Count < memberNos.Max())
                for (var i = 1; i < memberNos.Max(); i++)
                {
                    if (!memberNos.Contains(i))
                    {
                        return i;
                    }
                }
            return memberNos.Count + 1;
        }

        public static bool AddMembertoDb(Member member)
        {
            LogTo.Info("Add Member to Db");
            var query =
                _dbConnection.Table<Member>()
                    .Where(
                        v =>
                            v.Name == member.Name &&
                            v.Vorname == member.Vorname &&
                            v.Geburtstag.Equals(member.Geburtstag));
            if (query.Any())
            {
                member.Id = query.First().Id;
            }
            else
            {
                var listOfMemberNos = GetListOfMemberNos();
                if (member.Id <= 0)
                {
                    member.Id = GetFirstFreeMemberNo(listOfMemberNos);
                }
                if (listOfMemberNos.Contains(member.Id))
                {
                    member.Id = GetFirstFreeMemberNo(listOfMemberNos);
                    //throw new Exception("MemberNumber is not Unique!");
                }
            }
            _dbConnection.InsertOrReplace(member);
            return true;
        }

        public static async Task AddMemberstoDb(List<Member> mlist)
        {
            LogTo.Info("Add Members to Db");
            var t = new Task(() =>
            {
                foreach (var m in mlist)
                {
                    AddMembertoDb(m);
                }
            });
            t.Start();
            await t;
        }

        public static ObservableCollection<Member> GetAllMembers()
        {
            LogTo.Info("Getting all members");
            var retList = _dbConnection.Table<Member>().ToList();
            var members = new ObservableCollection<Member>(retList);
            return members;
        }

        public static void InsertUpdateCollection(ObservableCollection<Member> members)
        {
            LogTo.Info("Updating DB");
            var currentDb = GetAllMembers();
            currentDb.ForEach(m =>
            {
                if (!members.Contains(m))
                {
                    _dbConnection.Delete(m);
                }
            });
            members.ForEach(m => _dbConnection.InsertOrReplace(m));
        }
    }
}