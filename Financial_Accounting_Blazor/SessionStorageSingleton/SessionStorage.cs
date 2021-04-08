using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.SessionStorageSingleton
{
    public class SessionStorage
    {
        public List<Transaction> SessionTransactions { get; set; }
        private SessionStorage()
        {
            SessionTransactions = new List<Transaction>();
        }
        private static SessionStorage _instance;
        public static SessionStorage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SessionStorage();
            }
            return _instance;
        }
    }
}
