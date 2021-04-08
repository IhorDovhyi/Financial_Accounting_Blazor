using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.SessionStorageSingleton;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWorkOFT<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private bool disposed = false;

        SessionStorage sessionStorage;
        IRepository<Transaction> transactionWorkRepo;
        List<Transaction> isNew;
        List<Transaction> isDirty;
        List<Transaction> isDeleted;

        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            sessionStorage = SessionStorage.GetInstance();
            transactionWorkRepo = new Repository(_context);
            isNew = new List<Transaction>();
            isDirty = new List<Transaction>();
            isDeleted = new List<Transaction>();
        }
        public TContext DbContext => _context;
        public List<Transaction> GetAll()
        {
            if(!this.sessionStorage.SessionTransactions.Any())
            {
                this.sessionStorage.SessionTransactions = transactionWorkRepo.GetAll().ToList();
                foreach(var item in transactionWorkRepo.GetAll())
                {
                    this.RegisterNew(item);
                }
            }
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
                if (transactionWorkRepo.Get(id) != null)
                {
                    this.RegisterNew(transactionWorkRepo.Get(id));
                    this.Commit();
                    toReturn = this.Get(id);
                }
                else toReturn = null;
            }
            this.RegisterDirty(toReturn);
            return toReturn;
        }
        public void Create(Transaction item)
        {
            this.RegisterNew(item);
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
                if (transactionWorkRepo.Get(item.Id) != null)
                {
                    this.RegisterDirty(transactionWorkRepo.Get(item.Id));
                    this.Update(item);
                }
                else transactionWorkRepo.Update(item);
            }
        }
        public void Delete(int id)
        {
            if (this.AnyIn(id, this.sessionStorage.SessionTransactions))
            {
                this.RegisterDeleted(this.FindIn(id, this.sessionStorage.SessionTransactions));
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
            if (this.isNew.Any())
            {
                foreach (var isnew in this.isNew)
                {
                    if (!this.AnyIn(isnew, this.transactionWorkRepo.GetAll().ToList()))
                    {
                        this.transactionWorkRepo.Create(isnew);
                        this.sessionStorage.SessionTransactions.Add(isnew);
                    }
                }
                this.isNew.Clear();
            }
        }
        internal void CommitDirty()
        {
            if (this.isDirty.Any())
            {
                foreach (var isdirty in this.isDirty)
                {
                    if (this.AnyIn(isdirty, this.sessionStorage.SessionTransactions))
                    {
                        if (this.AnyIn(isdirty, this.transactionWorkRepo.GetAll().ToList()))
                        {
                            transactionWorkRepo.Update(isdirty);
                            this.sessionStorage.SessionTransactions.Remove(this.FindIn(isdirty, this.sessionStorage.SessionTransactions));
                            this.sessionStorage.SessionTransactions.Add(isdirty);
                        }
                        else
                        {
                            this.RegisterNew(isdirty);
                        }
                    }
                    else
                    {
                        this.RegisterNew(isdirty);
                    }
                }
                this.isDirty.Clear();
            }
        }
        internal void CommitDeleted()
        { 
            if (this.isDeleted.Any())
            {
                foreach (var isdeleted in this.isDeleted)
                {
                    this.transactionWorkRepo.Delete(isdeleted.Id);
                    this.sessionStorage.SessionTransactions.Remove(isdeleted);
                }
                this.isDeleted.Clear();
            }
        }
        public void Save()
        {
            Commit();
            transactionWorkRepo.Save();
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
        internal bool AnyIn(int? inputId,List<Transaction> inputCollection)
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
                    if (transactionWorkRepo != null)
                    {
                        this.isDeleted.Clear();
                        this.isDirty.Clear();
                        this.isNew.Clear();
                    }
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
