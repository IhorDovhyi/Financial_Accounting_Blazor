using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UseCaseResultBuilder
{
      public interface IBuilder
      {
        public void BuildStatus();
        public void BuildResult(List<Transaction> transaction);
        public void BuildMessageCode();
        public void BuildMessageText();
        public void Reset();
        public UseCaseResult GetResult();
   
    }
}
