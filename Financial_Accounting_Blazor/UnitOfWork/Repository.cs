using Financial_Accounting_Blazor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UnitOfWork
{
    public class Repository: IRepository<Transaction>
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<Transaction> dbSet;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.dbSet = this.dbContext.Set<Transaction>();
        }

        public IEnumerable<Transaction> GetAll()
        {
            return this.dbSet;
        }
        public Transaction Get(int? id)
        {
            return this.dbSet.Find(id);
        }
        public void Create(Transaction item)
        {
            var toCreate = this.dbSet.Find(item.Id);
            if (toCreate == null)
            { 
                this.dbSet.Add(item);
            }
        }
        public void Delete(int id)
        {
            var item = this.dbSet.Find(id);
            if (item != null)
            {
                this.dbSet.Remove(item);
            }
           
        }
        public void Update(Transaction entity)
        {
            this.dbSet.Update(entity);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
