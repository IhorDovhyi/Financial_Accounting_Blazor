using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace inancial_Accounting_Blazor_Tests
{
    public class TestDB : IDisposable
    {
        public List<Transaction> Transactions { get; set; }

        public TestDB()
        {
            Transactions = new List<Transaction>
            {
                new Transaction { Id = 1, sum = 5000, DateTime = new DateTime(2015, 7, 20, 18, 30, 25).ToLocalTime() },
                new Transaction { Id = 2, sum = 5000, DateTime = new DateTime(2016, 8, 21, 18, 30, 25).ToLocalTime() },
                new Transaction { Id = 3, sum = 5000, DateTime = new DateTime(2021, 7, 10, 11, 30, 25).ToLocalTime() },
                new Transaction { Id = 4, sum = -5000, DateTime = new DateTime(2020, 7, 10, 11, 30, 25).ToLocalTime() }
            };
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
