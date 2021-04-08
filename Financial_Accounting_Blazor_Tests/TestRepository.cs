using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace inancial_Accounting_Blazor_Tests
{
    class TestRepository : IRepository<Transaction>
    {
        List<Transaction> transactions;
        bool isSave = false;
        public TestRepository(TestDB testContext)
        {
            transactions = testContext.Transactions;
        }
        public IEnumerable<Transaction> GetAll()
        {
            return this.transactions;
        }
        public void Create(Transaction item)
        {
            transactions.Add(item);
        }

        public void Delete(int id)
        {
            var item = transactions.Find(x => x.Id == id);
            if (item != null)
                transactions.Remove(item);
        }

        public Transaction Get(int? id)
        {
            return transactions.Find(x => x.Id == id);
        }

        public void Update(Transaction item)
        {
            transactions.Find(x => x.Id == item.Id).sum = item.sum;
            transactions.Find(x => x.Id == item.Id).DateTime = item.DateTime;
        }

        public void Save()
        {
            this.isSave = true;
        }
    }
}
