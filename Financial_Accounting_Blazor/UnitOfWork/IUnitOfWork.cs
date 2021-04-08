using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        List<Transaction> GetAll();
        Transaction Get(int? id);
        void Create(Transaction item);
        void Update(Transaction item);
        void Delete(int id);
        void RegisterNew(Transaction entity);
        void RegisterDirty(Transaction entity);
        void RegisterClean(Transaction entity);
        void RegisterDeleted(Transaction entity);
        public void Commit();
        public void Save();
    }
}
