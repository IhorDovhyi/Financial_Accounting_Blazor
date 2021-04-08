using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UnitOfWork
{
    public interface IUnitOfWorkOFT<TContext> : IUnitOfWork where TContext : class
    {
        TContext DbContext { get; }
    }
}
