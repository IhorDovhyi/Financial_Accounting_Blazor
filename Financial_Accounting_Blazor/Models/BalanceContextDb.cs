using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.Models
{
    public class BalanceContextDb : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public BalanceContextDb(DbContextOptions<BalanceContextDb> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasKey(u => u.Id);
        }
    }
}
