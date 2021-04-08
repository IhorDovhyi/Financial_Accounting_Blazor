using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UseCaseResultBuilder
{
    public class NotFoundBuilder : IBuilder
    {
        UseCaseResult useCaseResult = new UseCaseResult();
        public void BuildMessageCode()
        {
            this.useCaseResult.messageCode = 200;
        }

        public void BuildMessageText()
        {
            this.useCaseResult.messageText = "Not Found";
        }

        public void BuildResult(List<Transaction> transaction)
        {
            this.useCaseResult.result = transaction;
        }

        public void BuildStatus()
        {
            this.useCaseResult.status = false;
        }

        public UseCaseResult GetResult()
        {
            UseCaseResult toReturn = this.useCaseResult;
            Reset();

            return toReturn;
        }

        public void Reset()
        {
            this.useCaseResult = new UseCaseResult();
        }
    }
}
