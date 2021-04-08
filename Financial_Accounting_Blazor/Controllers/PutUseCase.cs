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
    public class PutUseCase
    {
        public IUnitOfWork unitOfWork;
        public PutUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string Execute(Transaction transaction)
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            bool key = false;

            if (transaction == null)
            {
                key = true;
            }
            if (this.unitOfWork.GetAll().Any(x => x.Id == transaction.Id))
            {
                this.unitOfWork.Get(transaction.Id).sum = transaction.sum;
                this.unitOfWork.Get(transaction.Id).DateTime = transaction.DateTime;
                this.unitOfWork.Update(this.unitOfWork.Get(transaction.Id));
                key = true;
            }
            this.unitOfWork.Save();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(key));
        }
        public string Execute(List<Transaction> transactions)
        {
            bool key = false;
            foreach (var transaction in transactions)
            {
                key = (JsonSerializer.Deserialize<UseCaseResult>(Execute(transaction))).status;
            }
            ResultBuilder resultBuilder = new ResultBuilder();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(key));
        }
    }
}
