using Financial_Accounting_Blazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.UseCaseResultBuilder
{
    public class ResultBuilder
    {
        Director director = new Director();

        public UseCaseResult Build(List<Transaction> transactions)
        {
            if(transactions.Count > 0)
            {
                BuildOkWithData(transactions);
            }
            else
            {
                BuildNotFound();
            }
            return director.Builder.GetResult();
        }

        public UseCaseResult Build(bool status)
        {
            if(status)
            {
                BuildOkWithoutData();
            }
            else
            {
                BuildNotFound();
            }
            return director.Builder.GetResult();
        }

        private void BuildOkWithData(List<Transaction> transactions)
        {
            OKBuilder okBuilder = new OKBuilder();
            this.director.Builder = okBuilder;
            this.director.Builder.BuildStatus();
            this.director.Builder.BuildMessageCode();
            this.director.Builder.BuildMessageText();
            this.director.Builder.BuildResult(transactions);
        }

        private void BuildOkWithoutData()
        {
            OKBuilder okBuilder = new OKBuilder();
            this.director.Builder = okBuilder;
            this.director.Builder.BuildStatus();
            this.director.Builder.BuildMessageCode();
            this.director.Builder.BuildMessageText();
        }

        private void BuildNotFound()
        {
            NotFoundBuilder notFoundBuilder = new NotFoundBuilder();
            this.director.Builder = notFoundBuilder;
            this.director.Builder.BuildMessageCode();
            this.director.Builder.BuildMessageText();
            this.director.Builder.BuildStatus();
        }
    }
}
