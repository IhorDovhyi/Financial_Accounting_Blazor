using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UseCaseResultBuilder
{
    public class OKBuilder : IBuilder
    {
        UseCaseResult useCaseResult = new UseCaseResult();

        public void BuildMessageCode()
        {
           this.useCaseResult.messageCode = 200;
        }

        public void BuildMessageText()
        {
            this.useCaseResult.messageText = "OK";
        }

        public void BuildResult(List<Transaction> transaction) 
        {
            this.useCaseResult.result = transaction;
        }

        public void BuildStatus()
        {
            this.useCaseResult.status = true;
        }

        public UseCaseResult GetResult()
        {
            return this.useCaseResult;
        }

        public void Reset()
        {
            this.useCaseResult = new UseCaseResult();
        }
    }
}
