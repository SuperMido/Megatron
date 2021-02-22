using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
namespace Megatron.Models
{
    public class AccountService
    {
        private readonly string _dataFile = @"Data\data.xml";
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<Account>));
        public HashSet<Account> Accounts { get; set; }

        public AccountService()
        {
            if (!File.Exists(_dataFile))
            {
                Accounts = new HashSet<Account>() {
                   new Account{Id = 1, Firstname = "ASP.NET Core for dummy", Lastname = "Trump D.", Falcuty = "Washington", BirthDate = new DateTime()},
                    new Account{Id = 2, Firstname = "Pro ASP.NET Core", Lastname = "Putin V.", Falcuty = "Moscow", BirthDate = new DateTime()},
                    new Account{Id = 3, Firstname = "ASP.NET Core Video course", Lastname = "Obama B.", Falcuty = "Washington", BirthDate = new DateTime()},
                    new Account{Id = 4, Firstname = "Programming ASP.NET Core MVC", Lastname = "Clinton B.", Falcuty = "Washington", BirthDate = new DateTime()},
                    new Account{Id = 5, Firstname = "ASP.NET Core Razor Pages", Lastname = "Yelstin B.", Falcuty = "Moscow", BirthDate = new DateTime()},
                };
            }
            else
            {
                using var stream = File.OpenRead(_dataFile);
                Accounts = _serializer.Deserialize(stream) as HashSet<Account>;
            }
        }

        public Account[] Get() => Accounts.ToArray();

        public Account Get(int id) => Accounts.FirstOrDefault(b => b.Id == id);

        public bool Add(Account account) => Accounts.Add(account);

        public Account Create()
        {
            var max = Accounts.Max(b => b.Id);
            var b = new Account()
            {
                Id = max + 1,
                BirthDate = DateTime.Now
            };
            return b;
        }

        public bool Update(Account account)
        {
            var b = Get(account.Id);
            return b != null && Accounts.Remove(b) && Accounts.Add(account);
        }

        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && Accounts.Remove(b);
        }

        public void SaveChanges()
        {
            using var stream = File.Create(_dataFile);
            _serializer.Serialize(stream, Accounts);
        }
    }
}