using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.SessionStorageSingleton;
using Financial_Accounting_Blazor.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inancial_Accounting_Blazor_Tests
{
    public class TestUnitOfWork<TContext> : IUnitOfWorkOFT<TContext> where TContext : TestDB
    {
        private readonly TContext _context;
        private bool disposed = false;

        bool testSave = false;

        SessionStorage sessionStorage;
        IRepository<Transaction> workRepo;
        List<Transaction> isNew;
        List<Transaction> isDirty;
        List<Transaction> isDeleted;
        public TestUnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            sessionStorage = SessionStorage.GetInstance();
            workRepo = new TestRepository(_context);
            isNew = new List<Transaction>();
            isDirty = new List<Transaction>();
            isDeleted = new List<Transaction>();
        }
        public TContext DbContext => _context;
        public List<Transaction> GetAll()
        {
            foreach (var entity in workRepo.GetAll())
            {
                this.RegisterNew(entity);
            }
            this.CommitNew();
            return this.sessionStorage.SessionTransactions;
        }
        public Transaction Get(int? id)
        {
            Transaction toReturn;
            if (this.AnyIn(id, this.sessionStorage.SessionTransactions))
            {
                toReturn = this.FindIn(id, sessionStorage.SessionTransactions);
            }
            else
            {
                if (workRepo.Get(id) != null)
                {
                    this.RegisterNew(workRepo.Get(id));
                    toReturn = this.Get(id);
                }
                else toReturn = null;
            }
            this.RegisterDirty(toReturn);
            return toReturn;
        }
        public void Create(Transaction item)
        {
            if (!this.AnyIn(item, this.sessionStorage.SessionTransactions))
            {
                this.RegisterNew(item);
            }
        }
        public void Update(Transaction item)
        {
            if (this.AnyIn(item, isDirty))
            {
                this.isDirty.Remove(this.FindIn(item, this.isDirty));
                this.RegisterDirty(item);
            }
            else
            {
                if (workRepo.Get(item.Id) != null)
                {
                    this.RegisterDirty(workRepo.Get(item.Id));
                    this.Update(item);
                }
                else workRepo.Update(item);
            }
        }
        public void Delete(int id)
        {
            if (this.AnyIn(id, this.sessionStorage.SessionTransactions))
            {
                this.RegisterDeleted(this.FindIn(id, this.sessionStorage.SessionTransactions));
            }
            else
            {
                if (workRepo.Get(id) != null)
                {
                    this.RegisterDeleted(workRepo.Get(id));
                    this.Delete(id);
                }
                else workRepo.Delete(id);
                {
                    workRepo.Delete(id);
                }
            }
        }
        public void RegisterNew(Transaction entity)
        {
            this.isNew.Add(entity);
        }
        public void RegisterDirty(Transaction entity)
        {
            this.isDirty.Add(entity);
        }
        public void RegisterClean(Transaction entity)
        {
            sessionStorage.SessionTransactions.Add(entity);
        }
        public void RegisterDeleted(Transaction entity)
        {
            this.isDeleted.Add(entity);
        }
        public void Commit()
        {
            CommitNew();
            CommitDirty();
            CommitDeleted();
        }
        internal void CommitNew()
        {
            foreach (var isnew in this.isNew)
            {
                if (!this.AnyIn(isnew, this.sessionStorage.SessionTransactions))
                {
                    workRepo.Create(isnew);
                    this.sessionStorage.SessionTransactions.Add(isnew);
                }
                else
                {
                    this.RegisterDeleted(isnew);
                }
            }
        }
        internal void CommitDirty()
        {
            foreach (var isdirty in this.isDirty)
            {
                if (this.AnyIn(isdirty, this.sessionStorage.SessionTransactions))
                {
                    workRepo.Update(isdirty);
                    this.sessionStorage.SessionTransactions.Remove(this.FindIn(isdirty, this.sessionStorage.SessionTransactions));
                    this.sessionStorage.SessionTransactions.Add(isdirty);
                }
                else
                {
                    RegisterDeleted(isdirty);
                }
            }
        }
        internal void CommitDeleted()
        {
            foreach (var isdeleted in this.isDeleted)
            {
                if (this.AnyIn(isdeleted, this.sessionStorage.SessionTransactions))
                {
                    workRepo.Delete(isdeleted.Id);
                    this.sessionStorage.SessionTransactions.Remove(isdeleted);
                }
            }
            this.isDeleted.Clear();
        }
        public void Save()
        {
            this.testSave = true;
        }
        internal Transaction FindIn(Transaction inputTransaction, List<Transaction> inputCollection)
        {
            return inputCollection.Find(x => x.Id == inputTransaction.Id);
        }
        internal Transaction FindIn(int? inputId, List<Transaction> inputCollection)
        {
            return inputCollection.Find(x => x.Id == inputId);
        }
        internal bool AnyIn(Transaction inputTransaction, List<Transaction> inputCollection)
        {
            return inputCollection.Any(x => x.Id == inputTransaction.Id);
        }
        internal bool AnyIn(int? inputId, List<Transaction> inputCollection)
        {
            return inputCollection.Any(x => x.Id == inputId);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        ~TestUnitOfWork()
        {
            Dispose(false);
        }
    }
}
