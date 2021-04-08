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
    public class DeleteUseCase
    {
        public IUnitOfWork unitOfWork;
        public DeleteUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string Execute()
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            bool key = false;
            List<Transaction> toDelete = this.unitOfWork.GetAll();
            if (toDelete.Count != 0)
            {
                key = true;
                foreach (var todelete in toDelete)
                {
                    this.unitOfWork.Delete(todelete.Id);
                }
            }
            this.unitOfWork.Save();
            toDelete.Clear();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(key));
        }

        public string Execute(int Id)
        {
            ResultBuilder resultBuilder = new ResultBuilder();
            bool key = false;

            if (this.unitOfWork.GetAll().Any(x => x.Id == Id))
            {
                this.unitOfWork.Delete(Id);
                key = true;
            }
            this.unitOfWork.Save();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(key));
        }
        public string Execute(int[] Ids)
        {
            bool key = false;
            foreach (var Id in Ids)
            {
                key = (JsonSerializer.Deserialize<UseCaseResult>(Execute(Id))).status;
            }
            ResultBuilder resultBuilder = new ResultBuilder();
            return JsonSerializer.Serialize<UseCaseResult>(resultBuilder.Build(key));
        }
    }
}
