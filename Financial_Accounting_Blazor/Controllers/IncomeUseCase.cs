using Financial_Accounting_Blazor.Models;
using Financial_Accounting_Blazor.UnitOfWork;
using Financial_Accounting_Blazor.UseCaseResultBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.Controllers
{
    public class IncomeUseCase
    {
        public IUnitOfWork unitOfWork;

        public IncomeUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string Execute()
        {
            List<Transaction> dbResult = this.unitOfWork.GetAll().Where(x => x.sum >= 0).ToList();
            this.unitOfWork.Save();
            return JsonSerializer.Serialize<UseCaseResult>(new ResultBuilder().Build(dbResult));
        }

        public string Execute(DateTime date)
        {
            Int64 toCompare = (Int64)TimeSpan.FromTicks(date.Ticks).TotalSeconds;
            List<Transaction> dbResult = this.unitOfWork.GetAll().
                                                         Where(x => (Int64)TimeSpan.FromTicks(x.DateTime.Ticks).TotalSeconds == toCompare
                                                         && x.sum > 0).
                                                         ToList();

            ResultBuilder resultBuilder = new ResultBuilder();
            this.unitOfWork.Save();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(dbResult));
        }
        public string Execute(DateTime start, DateTime end)
        {
            Int64 toCompareStart = (Int64)TimeSpan.FromTicks(start.Ticks).TotalSeconds;
            Int64 toCompareEnd = (Int64)TimeSpan.FromTicks(end.Ticks).TotalSeconds;
            List<Transaction> dbResult = this.unitOfWork.GetAll().
                                                         Where(x =>(Int64)TimeSpan.FromTicks(x.DateTime.Ticks).TotalSeconds >= toCompareStart &&
                                                                   (Int64)TimeSpan.FromTicks(x.DateTime.Ticks).TotalSeconds <= toCompareEnd && x.sum > 0).
                                                         ToList();
            ResultBuilder resultBuilder = new ResultBuilder();
            this.unitOfWork.Save();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(dbResult));
        }
    }
}
