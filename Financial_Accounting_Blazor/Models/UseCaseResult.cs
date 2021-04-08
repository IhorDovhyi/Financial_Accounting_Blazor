using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.Models
{
    public class UseCaseResult
    {
        public bool status { get; set; }
        public List<Transaction> result {get;set;}
        public int messageCode { get; set; }
        public string messageText { get; set; }
    }
}
